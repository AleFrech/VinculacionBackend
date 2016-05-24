using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class SectionStudentModel
    {
        [Required(ErrorMessage = "*requerido")]
        public long SectionId { get; set; }
        [Required(ErrorMessage ="*requerido")]
        public List<string> StudenstIds { get; set; }
    }
}