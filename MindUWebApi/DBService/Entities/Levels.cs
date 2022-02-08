using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DBService.Entities
{
    public partial class Levels
    {
        public Levels()
        {
            Collaborators = new HashSet<Collaborators>();
        }

        public int LevelId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Collaborators> Collaborators { get; set; }
    }
}
