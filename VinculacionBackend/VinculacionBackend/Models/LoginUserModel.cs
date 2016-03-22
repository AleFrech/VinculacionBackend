using System.ComponentModel.DataAnnotations;

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