using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class RejectedModel
    {
        [Required(ErrorMessage = "*requerido")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public string Message { get; set; }
    }
}