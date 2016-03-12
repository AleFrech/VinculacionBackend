using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string User { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Password { get;  set; }
    }
}