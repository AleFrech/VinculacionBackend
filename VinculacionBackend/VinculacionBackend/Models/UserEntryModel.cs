using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class UserEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        [AccountNumberExist(ErrorMessage = "El numero de cuenta ya existe")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string MajorId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Campus { get; set; }
        [Required(ErrorMessage = "*requerido")]
        [EmailExist(ErrorMessage = "El correo ya existe en la base de datos")]
        [ValidDomain(ErrorMessage ="Correo no valido")]
        public string Email { get; set; }
    }
}