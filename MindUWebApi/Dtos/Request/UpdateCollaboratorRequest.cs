using Dtos.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Request
{
    public class UpdateCollaboratorRequest : CollaboratorBase
    {
        public UpdateCollaboratorRequest()
        {
            Technologies = new List<CollaboratorTechnologiesDto>();
        }
        [Required]
        public int CollaboratorId { get; set; }
        
        [Required]
        public bool IsActive { get; set; }

        public List<CollaboratorTechnologiesDto> Technologies { get; set; }

    }
}
