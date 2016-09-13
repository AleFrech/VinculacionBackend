using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class SectionProjectEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        public long SectiontId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public IList<long> ProjectIds { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Description { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public double Cost { get; set; }
    }
}