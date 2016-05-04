using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class ProjectSectionModel
    {
        [Required(ErrorMessage = "*requerido")]
        public long ProjectId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public long SectionId { get; set; }
    }
}