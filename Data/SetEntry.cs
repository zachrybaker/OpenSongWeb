using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OpenSongWeb.Data
{
    public class SetEntry
    {
        /// <summary>
        /// The database ID of the set, created via saving.
        /// </summary>
        public Int64 ID { get; set; }

        [Required]
        public Int64 SongSetID { get; set; }

        public SongSet SongSet { get; set; }

        [Required]
        public int OSSongID { get; set; }

        public OSSong OSSong { get; set; }

        /// <summary>
        /// Possibly in the future we will attempt to determine this by looking at the chords,
        /// But for now this is required.
        /// </summary>
        [Required]
        [StringLength(6, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Key { get; set; }
        
        /// <summary>
        /// What fret this should be played in, on a guitar. Defaults to zero
        /// </summary>
        public int Capo { get; set; }
        
        /// <summary>
        /// The order of the song in the set. one-based.
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// The abbreviations of the presentation sequence of the different stanzas of the song.
        /// Ex: V1 C V2 C V3 C
        /// TBD: Should we make these values user-configurable, or a hard-coded set?
        /// </summary>
        [StringLength(100, ErrorMessage = "The {0} must be at at most {1} characters long.", MinimumLength = 0)]
        public string Presentation { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        public bool IsKeyKnown()
        {
            return !string.IsNullOrEmpty(this.Key) && !(this.Key == "---") && !(this.Key == "--");
        }
    }
}
