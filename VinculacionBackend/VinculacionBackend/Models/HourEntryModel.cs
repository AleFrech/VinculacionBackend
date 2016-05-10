using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class HourEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public long SectionId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public long ProjectId{ get; set; }
        [Required(ErrorMessage = "*requerido")]
        public int Hour { get; set; }
    }
}