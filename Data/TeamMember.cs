using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OpenSongWeb.Data
{
    public class TeamMember
    {
        enum TeamStatus
        {
            Envited,
            Joined,
            Left
        };

        /// <summary>
        /// The person who is building their team
        /// </summary>
        [Required]
        public AppUser AppUser { get; set; }

        /// <summary>
        ///  The person envited.  The relationship is not necessairly bi-directional.
        /// </summary>
        [Required]
        public AppUser TeamMate { get; set; }



        public DateTime EnvitedDateUTC { get; set; }

        public DateTime JoinedDateUTC { get; set; }

        public DateTime EmailedDateUTC { get; set; }
        public DateTime LeftDateUTC { get; set; }

        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Email { get; set; }

    }
}
