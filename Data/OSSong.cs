using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace OpenSongWeb.Data
{
    public enum EmbedLinkType
    {
        Youtube,
        Vimeo
    };

    /// <summary>
    /// Open Song Song 
    /// </summary>
    public partial class OSSong
    {
        /// <summary>
        /// The database ID, also found in the file under the "songID" tag
        /// </summary>
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }


        [StringLength(255, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Writer or primary performer of the song.
        /// </summary>
        [StringLength(255, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 3)]
        [Required]
        public string Author { get; set; }

        [StringLength(255, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 1)]
        [Required]
        public string Copyright { get; set; }

        /**
         * The key the song is in.  In time not require this but instead attempt to infer it.
         **/
        [StringLength(6, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 1)]
        [Required]
        public string Key { get; set; }

        /**
         * comma-separated list of section acronyms to represent the sequence of the song.
         * Ex: V1,V2,C,V3,C,T,C would translate to verse 1, verse 2, chorus, verse 3, chorus, tag, chorus
         **/
        [StringLength(100, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 0)]
        [Required]
        public string Presentation { get; set; }

        /// <summary>
        /// The lines of chords, comments, lyrics, and stanza indicators
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Capo, on guitar. For now all sets are relative to guitar.
        /// </summary>
        [Required]
        public int Capo { get; set; }

        [StringLength(10, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 0)]
        public string HymnNumber { get; set; }

        [StringLength(12, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 0)]
        public string CCLINumber { get; set; }
        /// <summary>
        /// A searchable description of themes of the song.
        /// </summary>
        public string Themes { get; set; }

        [StringLength(120, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 0)]
        public string Tempo { get; set; }

     

        public EmbedLinkType VideoLinkType { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 0)]
        [RegularExpression(@"^[a-zA-Z0-9]{1,20}$",
         ErrorMessage = "Invalid imbed values are not allowed.")]
        public string VideoEmbedID { get; set; }

        /* TODO...
        public string aka { get; set; }
        public string key_line { get; set; }
        public string user1 { get; set; }
        public string user2 { get; set; }
        public string user2 { get; set; }
        public string time_sig { get; set; }*/

        /// <summary>
        /// Filename of the corresponding OpenSong-formatted XML file on disk,
        /// to maintain OpenSong desktop app compatibility.
        /// This will always be combined with the config setting for the songs folder location.
        /// Must be unique.
        /// Does not have to actually exist on disk yet.
        /// </summary>
        [StringLength(255, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 3)]
        [Required]
        public string Filename { get; set; }

        /// ///////////////////////
        // Non-opensong meta:
        /// ///////////////////////

        [Required]
        public DateTime CreatedDateUTC { get; set; }
        public DateTime? LastUpdatedDateUTC { get; set; }

        
        public string CreatedByID { get; set; }

        /// <summary>
        /// Be very careful to NOT send this over the wire
        /// </summary>
        public AppUser CreatedBy { get; set; }
        

        public void CopyProps(OSSong otherSong)
        {
            Author = otherSong.Author;
            Capo = otherSong.Capo;
            CCLINumber = otherSong.CCLINumber;
            Copyright = otherSong.Copyright;

            HymnNumber = otherSong.HymnNumber;
            Key = otherSong.Key;
            Content = otherSong.Content;
            Presentation = otherSong.Presentation;
            Themes = otherSong.Themes;
            Title = otherSong.Title;
            CreatedDateUTC = otherSong.CreatedDateUTC;
            LastUpdatedDateUTC = otherSong.LastUpdatedDateUTC;
            CreatedByID = otherSong.CreatedByID;
            CreatedBy = otherSong.CreatedBy;
            VideoEmbedID = otherSong.VideoEmbedID;
            VideoLinkType = otherSong.VideoLinkType;
            Tempo = otherSong.Tempo;
        }

        public OSSong Clone()
        {
            var song = new OSSong();
            song.CopyProps(this);
            return song;
        }
    }



}
