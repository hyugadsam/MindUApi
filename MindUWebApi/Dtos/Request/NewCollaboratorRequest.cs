using Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Request
{
    public class NewCollaboratorRequest : CollaboratorBase
    {
        public NewCollaboratorRequest()
        {
            Technologies = new List<CollaboratorTechnologiesDto>();
        }

        public List<CollaboratorTechnologiesDto> Technologies { get; set; }

    }
}
