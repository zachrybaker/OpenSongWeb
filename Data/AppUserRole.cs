using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OpenSongWeb.Data
{
    // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.0
    public class AppUserRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
