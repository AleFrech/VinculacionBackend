using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class VerifiedModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string AccountId { get; set; }
    }
}