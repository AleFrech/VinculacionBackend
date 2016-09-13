using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class ProfessorUpdateModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Campus { get; set; }
        [Required(ErrorMessage = "*requerido")]
        [ValidDomain(ErrorMessage = "Correo no valido")]
        public string Email { get; set; }
    }
}