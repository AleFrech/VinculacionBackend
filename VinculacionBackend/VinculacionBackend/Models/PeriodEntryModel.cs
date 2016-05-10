using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class PeriodEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        public int Number { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public int Year { get; set; }
    }
}