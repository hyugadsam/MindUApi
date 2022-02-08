using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class CollaboratorDto : CollaboratorBase
    {
        [Required]
        public int CollaboratorId { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public bool IsGraduated { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
