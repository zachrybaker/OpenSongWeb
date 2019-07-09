using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace OpenSongWeb.Data
{
    public partial class AppConfiguration
    {
        /// <summary>
        /// Name of the configuration entry
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Value of the configuration entry
        /// </summary>
        public string Value { get; set; }
    }



}
