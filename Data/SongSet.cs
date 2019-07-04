using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace OpenSongWeb.Data
{
    public class SongSet
    {
        /// <summary>
        /// The database ID of the set type, created via saving.
        /// </summary>
        public Int64 ID { get; set; }

        [Required]
        public DateTime CreatedDateUTC { get; set; }
        public DateTime? LastUpdatedDateUTC { get; set; }
        /// <summary>
        /// To be presentable, a date will be required.
        /// </summary>
        public DateTime? EventDateUTC { get; set; }
        
        public Guid CreatedByID { get; set; }

        /// <summary>
        /// Shortcut to who created this, without having to go to users tables
        /// </summary>
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string CreatedByEmail { get; set; }
        /// <summary>
        /// Shortcut to who created this, without having to go to users tables
        /// </summary>
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string CreatedByName { get; set; }


        /// <summary>
        /// When true, anyone can see the set.
        /// </summary>
        [Required]
        public bool IsPublic { get; set; }

        [StringLength(255, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        public string Notes { get; set; }
        
        /// <summary>
        /// Set Type will be required to present, and for reporting, at least.  
        /// </summary>
        public Guid? SetTypeId { get; set; }
        /// <summary>
        /// Set Type will be required to present, and for reporting, at least.  
        /// </summary>
        public SetType SetType { get; set; }
        
        public List<SetEntry> SetEntries { get; set; }
    }
}
