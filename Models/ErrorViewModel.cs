using System;
using System.Diagnostics;
using OpenSongWeb.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenSongWeb.Models
{
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorViewModel : PageModel
    {
        public ErrorViewModel(ErrorInfo info, HttpContext httpContext)
        {
            RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            FriendlyMessage = info.Message;
        }
        public ErrorViewModel() { }
        public ErrorViewModel(string requestID)
        {
            RequestId = requestID;
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string FriendlyMessage { get; set; }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}