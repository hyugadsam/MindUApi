using System;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class LevelDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int LevelId { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Description { get; set; }

    }
}
