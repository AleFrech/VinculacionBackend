using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class ProjectModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Description { get; set; }
        [Required(ErrorMessage = "*requerido")]
        [MajorListIsNotEmpty(ErrorMessage = "*lista no puede ir vacia")]
        public List<string> MajorIds { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string BeneficiarieOrganization {get;set;}
    }
}
