using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Hangfire;
using OpenSongWeb.Data;
using OpenSongWeb.Models;
using OpenSongWeb.Data.Repos;
using System.Text.RegularExpressions;
using System.Xml;

namespace OpenSongWeb.Managers
{
    public class XMLDataImportManager : IXMLDataImportManager
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IKeyProcessor _keyProcessor;
        private IOSSongRepo _osSongRepo;
        private IRepoUnitOfWork _repoUnitOfWork;

        private DirectoryInfo _incomingFolder;
        private DirectoryInfo _successFolder;
        private DirectoryInfo _failureFolder;
        private string _ccliNumber;

        public XMLDataImportManager(
            ILogger<XMLDataImportManager> logger,
            IKeyProcessor keyProcessor,
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _keyProcessor = keyProcessor;
            var cfg = configuration.GetSection("DataImportFolder");
            if (cfg != null)
            {
                if(!string.IsNullOrEmpty(cfg.GetValue<string>("Incoming")) && 
                    !string.IsNullOrEmpty(cfg.GetValue<string>("Success")) && 
                    !string.IsNullOrEmpty(cfg.GetValue<string>("Failure")))
                {
                    _incomingFolder = new DirectoryInfo(cfg.GetValue<string>("Incoming"));
                    _successFolder = new DirectoryInfo(cfg.GetValue<string>("Success"));
                    _failureFolder = new DirectoryInfo(cfg.GetValue<string>("Failure"));
                }
            }
           
            _ccliNumber = configuration.GetValue<string>("CCLINumber");
            _serviceScopeFactory = serviceScopeFactory;
        }


        /// <summary>
		/// Set up a data import process to be run in the background
		/// </summary>
		public void EnqueueBackgroundImport()
        {
            // Assume that we don't have another one queued or running.
            // It's OK if we do as long as they aren't fighting over resources.
            BackgroundJob.Enqueue(() => PerformImport());
        }

        private void CreateDirectoryIfNeeded(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _logger.LogInformation("Created directory {0}.", path);
            }
        }

        /// <summary>
        /// Process all data currently set up to be imported.
        /// </summary>
        /// <returns></returns>
        [DisableConcurrentExecution(5)]
        public async Task PerformImport()
        {
            _logger.LogInformation("Performing XML Data Import");
            _logger.LogInformation("Incoming Files: {0}", _incomingFolder.FullName);
            _logger.LogInformation("Successful Files: {0}", _successFolder.FullName);
            _logger.LogInformation("Failure files: {0}", _failureFolder.FullName);

            if (!_incomingFolder.Exists)
            {
                try
                {
                    CreateDirectoryIfNeeded(_incomingFolder.FullName);
                    CreateDirectoryIfNeeded(Path.Combine(_incomingFolder.FullName, "OSSongs"));
                }
                catch(IOException ex)
                {
                    // This is only an error if someone actually intended to have files available.
                    _logger.LogError("Could not perform XML data import. Incoming directory {0} does not exist, could not create.", _incomingFolder.FullName);
                    return;
                }
            }
            else 
            {
                CreateDirectoryIfNeeded(Path.Combine(_incomingFolder.FullName, "OSSongs"));
            }

            if (!_successFolder.Exists)
            {
                _successFolder.Create();
            }
            CreateDirectoryIfNeeded(Path.Combine(_successFolder.FullName, "OSSongs"));

            if (!_failureFolder.Exists)
            {
                _failureFolder.Create();
            }

            CreateDirectoryIfNeeded(Path.Combine(_failureFolder.FullName, "OSSongs"));

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _osSongRepo = scope.ServiceProvider.GetService<IOSSongRepo>();
                _repoUnitOfWork = scope.ServiceProvider.GetService<IRepoUnitOfWork>();

                await ProcessSongs();
            }

            _logger.LogInformation("Completed Data Import");
        }
       
        #region song processing
        private async Task ProcessSongs()
        {
            DirectoryInfo srcFolder = new DirectoryInfo(Path.Combine(_incomingFolder.FullName, "OSSongs"));
            var successFolder = Path.Combine(_successFolder.FullName, "OSSongs");
            var failureFolder = Path.Combine(_failureFolder.FullName, "OSSongs");
            var songsImported = 0;
            var songsFailedImport = 0;

            // go thru the files in name order so that they can be 
            foreach (var incomingFile in srcFolder.EnumerateFiles().OrderBy(f => f.Name))
            {
                var result = await ImportOpenSongFromFile(incomingFile);
                if (result.RightOrDefault() != null)
                {
                    songsFailedImport++;
                    _logger.LogError($"Failed to import file {incomingFile.Name}: {result.RightOrDefault().Message}.");
                    incomingFile.MoveTo(Path.Combine(failureFolder, incomingFile.Name));// true);
                }
                else 
                {
                    var song = result.LeftOrDefault();
                    if (song != null)
                    {
                        songsImported++;
                        incomingFile.MoveTo(Path.Combine(successFolder, incomingFile.Name));//, true);
                    }
                    else
                    {
                        songsFailedImport++;
                        _logger.LogError($"Failed to import file {incomingFile.Name} for unknown reason.");
                        incomingFile.MoveTo(Path.Combine(failureFolder, incomingFile.Name));//, true);
                    }
                }
            }

            _logger.LogInformation($"Completed OSSong files Import. {songsImported} imported, {songsFailedImport} failed.");
        }
        
        private ErrorInfo InflateOSSongFromXMLDocument(OSSong song, XmlDocument doc)
        {
            song.Title = doc.SelectSingleNode("//title/text()").Value;

            XmlNode node = doc.SelectSingleNode("//author");
            song.Author = node?.InnerText;

            node = doc.SelectSingleNode("//copyright");
            song.Copyright = node?.InnerText ?? "";

            node = doc.SelectSingleNode("//key");
            song.Key = "";
            if (node?.InnerText.Trim().Length > 0)
            {
                string strKey = string.Empty;
                if (_keyProcessor.ParseKey(node.InnerText.Trim(), out strKey, out string suffix))
                {
                    song.Key = strKey;
                }
            }

            node = doc.SelectSingleNode("//lyrics");
            // The opensong format xml structure stops at "lyrics" and it becomes a format based on what the starting character of each line is.
            song.Content = node?.InnerText;

            node = doc.SelectSingleNode("//presentation");
            song.Presentation = node?.InnerText ?? string.Empty;

            node = doc.SelectSingleNode("//capo");
            if (node != null)
            {
                if (int.TryParse(node.InnerText, out int nCapo))
                {
                    song.Capo = nCapo;
                }
            }

            node = doc.SelectSingleNode("//theme");
            song.Themes = node?.InnerText;

            node = doc.SelectSingleNode("//alttheme");
            if (node != null && node.InnerText.Length > 0)
            {
                if (!string.IsNullOrEmpty(song.Themes))
                {
                    song.Themes += ";";
                }
                else
                {
                    song.Themes = string.Empty;
                }

                song.Themes += node.InnerText;
            }

            node = doc.SelectSingleNode("//ccli");
            song.CCLINumber = node?.InnerText ?? "";
            if (!string.IsNullOrEmpty(_ccliNumber))
            {
                // override it.  we assume that if the app is running in a single CCLINumber setup, then it should be used.
                song.CCLINumber = _ccliNumber;
            }

            node = doc.SelectSingleNode("//songId");
            if (int.TryParse(node?.InnerText, out int n))
            {
                song.ID = n;
            }

            return null;
        }

        /// <summary>
        /// Processes an xml file as an OpenSong-formatted document.  Saves to the database.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="successFolder"></param>
        /// <param name="failFolder"></param>
        /// <returns></returns>
        public async Task<Either<OSSong, ErrorInfo>> ImportOpenSongFromFile(FileInfo fi)
        {
            if (fi != null && fi.Exists)
            {
                string strFileContents;
                using (System.IO.StreamReader sr = fi.OpenText())
                {
                    strFileContents = await sr.ReadToEndAsync();
                }

                strFileContents = Regex.Replace(strFileContents, "\r\n?|\n", "\n", RegexOptions.Compiled);
                strFileContents = strFileContents.Replace("`", "'");//

                XmlDocument doc = new XmlDocument();
                OSSong song = null;

                try
                {
                    doc.LoadXml(strFileContents);

                    song = new OSSong();
                    song.Filename = fi.Name;
                    var error = InflateOSSongFromXMLDocument(song, doc);
                    if (error != null)
                    {
                        return error;
                    }

                    OSSong otherSong = await _osSongRepo.GetByFilename(song.Filename);
                    IEnumerable<OSSong> songsByName = await _osSongRepo.All(new SongFilterParameter { title = song.Title });

                    if (otherSong != null)
                    {
                        if (!song.ID.HasValue)
                        {
                            otherSong.CopyProps(song);
                            song = _osSongRepo.Update(otherSong);
                        }
                        else if (song.Title == otherSong.Title)
                        {
                            if (song.ID != otherSong.ID)
                            {
                                /// hmmm...?
                                song = null;
                                error = new ErrorInfo
                                {
                                    FailureReason = ErrorInfo.Reason.ValidationError,
                                    Message = $"Another song with the same title of {song.Title} and filename of {song.Filename} exists but with a different id."
                                };

                            }
                            else
                            {
                                otherSong.CopyProps(song);
                                song = _osSongRepo.Update(otherSong);
                            }
                        }
                        else
                        {
                            var sameSongDifferentFile = songsByName.FirstOrDefault(m => m.Title == song.Title && song.Author == m.Author);
                            if (sameSongDifferentFile != null)
                            {
                                // some other filename but the same song.
                                sameSongDifferentFile.CopyProps(song);
                                song = _osSongRepo.Update(sameSongDifferentFile);
                            }
                            else
                            {
                                /// Perhaps the filename is nothing to do with the song title, so.... make the filename unique.
                                /// revisit this to make sure that they are not abusing the system?
                                song.Filename += song.ID.ToString();
                                if (_osSongRepo.Get(song.ID.Value) != null)
                                {
                                    song = _osSongRepo.Update(song);
                                }
                                else
                                {
                                    song.ID = 0;
                                    song = _osSongRepo.Create(song);
                                }
                            }
                        }

                    }
                    else
                    {
                        
                        var sameSongDifferentFile = songsByName.FirstOrDefault(m => m.Title == song.Title && song.Author == m.Author);
                        if (sameSongDifferentFile != null)
                        {
                            // some other filename but the same song.
                            sameSongDifferentFile.CopyProps(song);
                            song = _osSongRepo.Update(sameSongDifferentFile);
                        }
                        else
                        {

                            if (!song.ID.HasValue)
                            {
                                song = _osSongRepo.Create(song);
                            }
                            else
                            {
                                song = _osSongRepo.Update(song);
                            }
                        }
                    }

                    if (song != null)
                    {
                        await _repoUnitOfWork.SaveAsync();
                        return song;
                    }

                    return error;
                }
                catch (Exception e)
                {
                    return new ErrorInfo
                    {
                        FailureReason = ErrorInfo.Reason.ValidationError,
                        Message = $"Failure to import song: {e.Message}"
                    };
                }
            }
            else
            {
                return new ErrorInfo
                {
                    FailureReason = ErrorInfo.Reason.NotFoundError,
                    Message = $"{fi.Name} was not found."
                };
            }
        }


        #endregion song processing

      
    }
}