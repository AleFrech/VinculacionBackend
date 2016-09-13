using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class StudentChangePasswordModel
    {
        [Required(ErrorMessage = "*requerido")]
        [StudentIsInactive(ErrorMessage = "El estudiante ya esta activo o no existe")]
        [AccountNumberDosentExist(ErrorMessage = "El numero de cuenta no existe")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        [EmailDosentExist(ErrorMessage = "El correo no existe en la base de datos")]
        [ValidDomain(ErrorMessage = "Correo no valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Password { get; set; }
       
    }
}