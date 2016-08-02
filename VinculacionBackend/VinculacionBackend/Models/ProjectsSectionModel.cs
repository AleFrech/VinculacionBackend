using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class ProjectsSectionModel
    {
        [Required(ErrorMessage = "*requerido")]
        public List<long> ProjectIds { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public long SectionId { get; set; }
    }
}