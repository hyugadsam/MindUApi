using Dtos.CustomValidations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class CollaboratorTechnologiesDto
    {
        public CollaboratorTechnologiesDto()
        {
            ProyectsUrl = new List<string>();
            Certificates = new List<string>();
        }

        [Required]
        public int TechnologyId { get; set; }

        [Required]
        [Range(0, 100)]
        public int YearsExperience { get; set; }

        //[StringListValidation("ExpresionRegular", "Url")] //Pendiente
        public List<string> ProyectsUrl { get; set; }

        public List<string> Certificates { get; set; }

    }
}
