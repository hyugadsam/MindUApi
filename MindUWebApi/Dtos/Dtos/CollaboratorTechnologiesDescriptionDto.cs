using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class CollaboratorTechnologiesDescriptionDto : CollaboratorTechnologiesDto
    {
        public CollaboratorTechnologiesDescriptionDto() : base()
        {
        }
        [Required]
        public string Technology { get; set; }

    }
}
