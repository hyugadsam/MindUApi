using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DBService.Entities
{
    public partial class CollaboratorsTechnologies
    {
        public int Id { get; set; }
        public int Collaboratorid { get; set; }
        public int TechnologyId { get; set; }
        public int YearsExperience { get; set; }
        public string ProyectsUrl { get; set; }
        public string Certificates { get; set; }

        public virtual Collaborators Collaborator { get; set; }
        public virtual Technologies Technology { get; set; }
    }
}
