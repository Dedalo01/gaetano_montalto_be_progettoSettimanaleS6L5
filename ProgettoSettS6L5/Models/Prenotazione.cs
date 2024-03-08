using System;
using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Prenotazione
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve essere di 16 caratteri.")]
        public string CodiceFiscaleCliente { get; set; }
        [Required]
        public DateTime DataPrenotazione { get; set; }
        [Required]
        public int NumeroProgressivo { get; set; }
        [Required]
        public DateTime Anno { get; set; }
        [Required]
        public DateTime PeriodoSoggiornoInizio { get; set; }
        [Required]
        public DateTime PeriodoSoggiornoFine { get; set; }
        [Required]
        public decimal Caparra { get; set; }
        [Required]
        public decimal TariffaApplicata { get; set; }
        [Required]
        public string DettagliPrenotazione { get; set; } // mezza pensione, pensione completa, pernottamento con prima colazione
        [Required]

        public int NumeroCameraId { get; set; }
    }
}