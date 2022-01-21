using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dtos.Dtos
{
    public class CollaboratorBase
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string TimeZone { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Levelid { get; set; }
        
    }
}
