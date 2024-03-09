using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Cliente
    {
        [Key]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve essere di 16 caratteri.")]
        [Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }
        [Required]
        public string Cognome { get; set; }
        [Required]
        public string Nome { get; set; }

        public string Citta { get; set; }

        public string Provincia { get; set; }
        [Required]
        public string Email { get; set; }
        public int Telefono { get; set; }
        public int Cellulare { get; set; }
    }
}