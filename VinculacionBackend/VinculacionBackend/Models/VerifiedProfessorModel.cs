using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class VerifiedProfessorModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string AccountId { get; set; }

        [Required(ErrorMessage = "*requerido")]
        public string Password { get; set; }
    }
}