using System;
using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Prenotazione
    {
        [Key]
        public int Id { get; set; }

        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve essere di 16 caratteri.")]
        public string CodiceFiscaleCliente { get; set; }
        [Required(ErrorMessage = "Inserisci Data Prenotazione")]
        public DateTime DataPrenotazione { get; set; }
        [Required(ErrorMessage = "Inserisci Numero Progressivo")]
        public int NumeroProgressivo { get; set; }
        [Required(ErrorMessage = "Inserisci Anno")]
        public int Anno { get; set; }
        [Required(ErrorMessage = "Inserisci Periodo Soggiorno Iniziale")]
        public DateTime PeriodoSoggiornoInizio { get; set; }
        [Required(ErrorMessage = "Inserisci Periodo Soggiorno Fine")]
        public DateTime PeriodoSoggiornoFine { get; set; }
        [Required(ErrorMessage = "Inserisci Caparra")]
        public decimal Caparra { get; set; }
        [Required(ErrorMessage = "Inserisci Tariffa Applicata")]
        public decimal TariffaApplicata { get; set; }

        public string DettagliPrenotazione { get; set; } // mezza pensione, pensione completa, pernottamento con prima colazione


        public int NumeroCameraId { get; set; }
    }
}