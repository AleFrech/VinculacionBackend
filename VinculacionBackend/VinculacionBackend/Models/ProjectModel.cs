using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class ProjectModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Description { get; set; }
    }
}