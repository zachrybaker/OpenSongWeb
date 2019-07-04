using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace OpenSongWeb.Data
{
    public class SetType
    {
        /// <summary>
        /// The database ID of the set type, created via saving.
        /// </summary>
        public Guid ID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [Range(1, 50000)]
        public int TypicalAudienceSize { get; set; }
    }
}
