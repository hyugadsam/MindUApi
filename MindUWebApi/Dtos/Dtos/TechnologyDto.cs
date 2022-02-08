using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dtos.Dtos
{
    public class TechnologyDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int TechnologyId { get; set; }
        [Required]
        [StringLength(maximumLength:100)]
        public string Description { get; set; }

    }
}
