using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OpenSongWeb.Data
{
    /// <summary>
    /// Other customization options here:
    // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.0
    /// </summary>
    public partial class AppUser : IdentityUser
    {
        [StringLength(100, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 3)]
        [PersonalData]
        [Required]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be between {1} and {2} characters long.", MinimumLength = 3)]
        [PersonalData]
        [Required]
        public string DisplayName { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

        
        public List<OSSong> SongsCreated { get; set; }
    }
}
