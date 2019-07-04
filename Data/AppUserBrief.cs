using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenSongWeb.Data;
using System.ComponentModel.DataAnnotations;

namespace OpenSongWeb.Data
{
    /// <summary>
    /// Our less risky (when public-facing), more simple Appuser.
    /// </summary>
    public partial class AppUserBrief
    {

        public string Id { get; private set; }

        public string DisplayName { get; private set; }

        public int SongCount { get; set; }

        ///public List<TeamMate> TeamMates { get; set; }

        /// <summary>
        /// relationship. Not sure if we need this?
        /// </summary>
        //public AppUser AppUser { get; set; }

        public List<OSSong> SongsCreated { get; set; }
        public AppUserBrief(AppUser user)
        {
            this.Id = user.Id;
            this.DisplayName = user.DisplayName;
            this.SongsCreated = user.SongsCreated;
            this.SongCount = user.SongsCreated.Count;
        }

        public AppUserBrief()
        {

        }
    }
}
