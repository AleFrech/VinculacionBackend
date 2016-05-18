using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class RoleEmailModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string Email { get; set; }
    }
}