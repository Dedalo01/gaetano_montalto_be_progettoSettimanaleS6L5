using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "Inserire Codice Fiscale (16 caratteri).")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve essere di 16 caratteri.")]
        [Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }
        [Required(ErrorMessage = "Inserire Cognome.")]
        public string Cognome { get; set; }
        [Required(ErrorMessage = "Inserire Nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Inserire Città.")]
        public string Citta { get; set; }
        [Required(ErrorMessage = "Inserire Provincia.")]
        public string Provincia { get; set; }
        [Required(ErrorMessage = "Inserire Email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Inserire Telefono.")]
        public int Telefono { get; set; }
        [Required(ErrorMessage = "Inserire Cellulare.")]
        public int Cellulare { get; set; }
    }
}