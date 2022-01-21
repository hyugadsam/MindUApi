using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class CollaboratorFullDto : CollaboratorDto
    {
        public CollaboratorFullDto()
        {
            Technologies = new List<CollaboratorTechnologiesDescriptionDto>();
        }
        public List<CollaboratorTechnologiesDescriptionDto> Technologies { get; set; }

    }
}
