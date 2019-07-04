using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Managers
{
    public class ErrorInfo
    {
        /// <summary>
        /// Enumeration of possible error reasons.
        /// Helps classify what additional data is useful
        /// </summary>
        public enum Reason
        {
            ValidationError,
            PermissionError,
            NotImplementedError,
            AuthenticationError,
            AuthorizationError,
            NotFoundError
        }

        /// <summary>
        /// The reason the action failed
        /// </summary>
        public Reason FailureReason { get; set; }

        /// <summary>
        /// A human readable message for the error
        /// </summary>
        public string Message { get; set; }
    }
}
