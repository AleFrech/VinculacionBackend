using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Models
{
    public class SectionStudentModel
    {
        [Required(ErrorMessage = "*requerido")]
        public long SectionId { get; set; }
        [Required(ErrorMessage ="*requerido")]
        public string StudentId { get; set; }
    }
}