using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dtos.Request
{
    public class GetCitiesRequest
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }
    }
}
