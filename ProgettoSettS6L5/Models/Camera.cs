using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Camera
    {
        [Key]
        public int IdNumeroCamera { get; set; }
        [Required]
        public string Descrizione { get; set; }
        [Required]
        public string Tipologia { get; set; }
    }
}