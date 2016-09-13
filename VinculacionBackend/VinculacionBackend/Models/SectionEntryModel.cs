using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class SectionEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string Code { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public long ClassId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string ProffesorAccountId { get; set; }
    }
}