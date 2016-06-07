using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class ClassEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Code { get; set; }
    }
}