using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenSongWeb.Data;

namespace OpenSongWeb.Models
{
    /// <summary>
    /// Eventually we will support chordii, not just OpenSong
    /// </summary>
    public enum SongFormat
    {
        OpenSong
    }

    public partial class SongBrief
    {
        public SongBrief(OSSong song)
        {
            this.ID = song.ID.Value;
            this.Format = SongFormat.OpenSong;
            this.Title = song.Title;
            this.Author = song.Author;
            this.Key = song.Key;
            this.Presentation = song.Presentation;
            this.Capo = song.Capo;
            this.HymnNumber = song.HymnNumber;
            this.Themes = song.Themes?.Split(';');
            this.CreatedDateUTC = song.CreatedDateUTC;
            this.LastUpdatedDateUTC = song.LastUpdatedDateUTC;

            this.CreatedBy = song.CreatedBy != null ? new AppUserBrief(song.CreatedBy) : null;
        }
        public int ID { get; set; }
        public SongFormat Format { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Key { get; set; }
        public string Presentation { get; set; }
        public int Capo { get; set; }
        public string HymnNumber { get; set; }
        public string[] Themes { get; set; }

        public DateTime CreatedDateUTC { get; set; }
        public DateTime? LastUpdatedDateUTC { get; set; }
        public AppUserBrief CreatedBy { get; set; }
    }
}
