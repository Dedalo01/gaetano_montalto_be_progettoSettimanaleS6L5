using System;
using System.ComponentModel.DataAnnotations;

namespace ProgettoSettS6L5.Models
{
    public class Servizio
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PrenotazioneId { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int Quantita { get; set; }
        [Required]
        public decimal Prezzo { get; set; }
        [Required]
        public string Tipologia { get; set; }
    }
}