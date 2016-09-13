using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class StudentAddManyEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        [AccountNumberExist(ErrorMessage = "El numero de cuenta ya existe")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Major { get; set; }
        [Required(ErrorMessage = "*requerido")]
        [EmailExist(ErrorMessage = "El correo ya existe en la base de datos")]
        [ValidDomain(ErrorMessage = "Correo no valido")]
        public string Email { get; set; }
    }
}