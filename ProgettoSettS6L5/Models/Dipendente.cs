using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Dipendente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Inserire Username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Inserire Password.")]
        public string Password { get; set; }
    }
}