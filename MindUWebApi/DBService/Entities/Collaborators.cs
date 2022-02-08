using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DBService.Entities
{
    public partial class Collaborators
    {
        public Collaborators()
        {
            CollaboratorsTechnologies = new HashSet<CollaboratorsTechnologies>();
        }

        public int CollaboratorId { get; set; }
        public string FullName { get; set; }
        public string TimeZone { get; set; }
        public int? Levelid { get; set; }
        public bool IsGraduated { get; set; }
        public bool? IsActive { get; set; }

        public virtual Levels Level { get; set; }
        public virtual ICollection<CollaboratorsTechnologies> CollaboratorsTechnologies { get; set; }
    }
}
