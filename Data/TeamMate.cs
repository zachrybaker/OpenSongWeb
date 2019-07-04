using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OpenSongWeb.Data
{
    public partial class TeamMate
    {
        [Required]
        public Int64 Id { get; set; }

        public string UserId { get; set; }

        public AppUserBrief Teammate { get; set; }
        

    }
}
