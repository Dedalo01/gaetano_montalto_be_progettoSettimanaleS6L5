using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Dipendente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}