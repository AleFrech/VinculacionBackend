using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    [StudentIsNotInSectionAttribute]
    [StudentIsNotInClassAttribute]
    public class SectionStudentModel
    {
        [Required(ErrorMessage = "*requerido")]
        public long SectionId { get; set; }
        [Required(ErrorMessage ="*requerido")]
        public List<string> StudenstIds { get; set; }
    }
}