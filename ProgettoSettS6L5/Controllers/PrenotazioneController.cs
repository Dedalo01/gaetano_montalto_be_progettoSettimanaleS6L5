using ProgettoSettS6L5.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace ProgettoSettS6L5.Controllers
{
    public class PrenotazioneController : Controller
    {
        // GET: Prenotazione
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistraUtenteEPrenotazione(RegistraClientePrenotazione clienteEprenotazione, int numeroCameraId)
        {
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                string insertClienteGetCFQuery = @"INSERT INTO Clienti (CodiceFiscale, Cognome, Nome, Citta, Provincia, Email, Telefono, Cellulare)
                    VALUES (@codiceFiscale, @cognome, @nome, @citta, @provincia, @email, @tel, @cel);
                  
                    ";

                SqlCommand insertClienteCmd = new SqlCommand(insertClienteGetCFQuery, conn);
                insertClienteCmd.Parameters.AddWithValue("codiceFiscale", clienteEprenotazione.Cliente.CodiceFiscale);
                insertClienteCmd.Parameters.AddWithValue("cognome", clienteEprenotazione.Cliente.Cognome);
                insertClienteCmd.Parameters.AddWithValue("nome", clienteEprenotazione.Cliente.Nome);
                insertClienteCmd.Parameters.AddWithValue("citta", clienteEprenotazione.Cliente.Citta);
                insertClienteCmd.Parameters.AddWithValue("provincia", clienteEprenotazione.Cliente.Provincia);
                insertClienteCmd.Parameters.AddWithValue("email", clienteEprenotazione.Cliente.Email);
                insertClienteCmd.Parameters.AddWithValue("tel", clienteEprenotazione.Cliente.Telefono);
                insertClienteCmd.Parameters.AddWithValue("cel", clienteEprenotazione.Cliente.Cellulare);

                //string codiceFiscaleCliente = insertClienteCmd.ExecuteScalar().ToString();
                int nRowsCliente = insertClienteCmd.ExecuteNonQuery();

                if (nRowsCliente > 0)
                {
                    int annoCorrente = clienteEprenotazione.Prenotazione.DataPrenotazione.Year;

                    // Ottieni numero progressivo
                    string getNumeroProgressivoQuery = @"SELECT ISNULL(MAX(NumeroProgressivo),0) + 1
                        FROM Prenotazioni WHERE Anno = @annoCorrente
                        ";
                    SqlCommand getNumeroProgressivoCmd = new SqlCommand(getNumeroProgressivoQuery, conn);
                    getNumeroProgressivoCmd.Parameters.AddWithValue("annoCorrente", annoCorrente);

                    int nuovoNumeroProgressivo = (int)getNumeroProgressivoCmd.ExecuteScalar();

                    string insertPrenotazioneQuery = @"INSERT INTO Prenotazioni (CodiceFiscaleCliente, DataPrenotazione, NumeroProgressivo, Anno, PeriodoSoggiornoInizio, PeriodoSoggiornoFine, Caparra, TariffaApplicata, DettagliPrenotazione, NumeroCameraId)
                        VALUES (@codFisCliente, @dataPrenotazione, @numeroProgressivo, @anno, @periodoSoggiornoInizio, @periodoSoggiornoFine, @caparra, @tariffaApplicata, @dettagliPrenotazione, @numeroCameraId)
                        ";
                    SqlCommand insertPrenotazioneCmd = new SqlCommand(insertPrenotazioneQuery, conn);
                    insertPrenotazioneCmd.Parameters.AddWithValue("codFisCliente", clienteEprenotazione.Cliente.CodiceFiscale);
                    insertPrenotazioneCmd.Parameters.AddWithValue("dataPrenotazione", clienteEprenotazione.Prenotazione.DataPrenotazione);
                    insertPrenotazioneCmd.Parameters.AddWithValue("numeroProgressivo", nuovoNumeroProgressivo);
                    insertPrenotazioneCmd.Parameters.AddWithValue("anno", annoCorrente);
                    insertPrenotazioneCmd.Parameters.AddWithValue("periodoSoggiornoInizio", clienteEprenotazione.Prenotazione.PeriodoSoggiornoInizio);
                    insertPrenotazioneCmd.Parameters.AddWithValue("periodoSoggiornoFine", clienteEprenotazione.Prenotazione.PeriodoSoggiornoFine);
                    insertPrenotazioneCmd.Parameters.AddWithValue("caparra", clienteEprenotazione.Prenotazione.Caparra);
                    insertPrenotazioneCmd.Parameters.AddWithValue("tariffaApplicata", clienteEprenotazione.Prenotazione.TariffaApplicata);
                    insertPrenotazioneCmd.Parameters.AddWithValue("dettagliPrenotazione", clienteEprenotazione.Prenotazione.DettagliPrenotazione);
                    insertPrenotazioneCmd.Parameters.AddWithValue("numeroCameraId", numeroCameraId);

                    int nRowsPrenotazione = insertPrenotazioneCmd.ExecuteNonQuery();

                    if (nRowsPrenotazione > 0)
                    {
                        TempData["IsSuccess"] = "Sono state effettuate correttamente la prenotazione e registrazione del Cliente.";
                        return RedirectToAction("AggiungiUtenteEPrenotazione", "Admin");

                    }

                }

            }
            catch (Exception ex)
            {

            }
            finally { conn.Close(); }

            return RedirectToAction("AggiungiUtenteEPrenotazione", "Admin");
        }

        [HttpPost]
        public ActionResult RegistraServizoPerCliente(Servizio servizio, int selectIdPrenotazione, string tipoDiServizio)
        {
            Servizio nuovoServizio = servizio;
            nuovoServizio.PrenotazioneId = selectIdPrenotazione;
            nuovoServizio.Tipologia = tipoDiServizio;

            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                string insertServizioQuery = @"INSERT INTO Servizi (PrenotazioneId, Data, Quantita, Prezzo, Tipologia)
                    VALUES (@prenotazioneId, @data, @quantita, @prezzo, @tipologia)
                    ";
                SqlCommand insertServizioCmd = new SqlCommand(insertServizioQuery, conn);
                insertServizioCmd.Parameters.AddWithValue("prenotazioneId", nuovoServizio.PrenotazioneId);
                insertServizioCmd.Parameters.AddWithValue("Data", nuovoServizio.Data);
                insertServizioCmd.Parameters.AddWithValue("prezzo", nuovoServizio.Prezzo);
                insertServizioCmd.Parameters.AddWithValue("quantita", nuovoServizio.Quantita);
                insertServizioCmd.Parameters.AddWithValue("tipologia", nuovoServizio.Tipologia);

                int nRowServizi = insertServizioCmd.ExecuteNonQuery();

                if (nRowServizi > 0)
                {
                    TempData["IsSuccess"] = $"Servizio aggiunto con successo.";
                    return RedirectToAction("AggiungiServizi", "Admin");
                }

            }
            catch (Exception ex)
            {

            }
            finally { conn.Close(); }


            return View();
        }



        [HttpPost]
        public JsonResult GetAllPrenotazioniByCodiceFiscale(string codiceFiscale)
        {
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);
            List<Prenotazione> prenotazioni = new List<Prenotazione>();
            try
            {
                conn.Open();

                string selectAllPrenotazioniByCodiceFiscaleQuery = "SELECT * FROM Prenotazioni WHERE CodiceFiscaleCliente = @codFisCliente";
                SqlCommand selectAllPrenotazioniCmd = new SqlCommand(selectAllPrenotazioniByCodiceFiscaleQuery, conn);
                selectAllPrenotazioniCmd.Parameters.AddWithValue("codFisCliente", codiceFiscale);

                SqlDataReader selectReader = selectAllPrenotazioniCmd.ExecuteReader();

                if (selectReader.HasRows)
                {
                    while (selectReader.Read())
                    {
                        Prenotazione prenotazione = new Prenotazione();
                        prenotazione.Id = (int)selectReader["Id"];
                        prenotazione.CodiceFiscaleCliente = (string)selectReader["CodiceFiscaleCliente"];
                        prenotazione.DataPrenotazione = (DateTime)selectReader["DataPrenotazione"];
                        prenotazione.NumeroProgressivo = (int)selectReader["NumeroProgressivo"];
                        prenotazione.PeriodoSoggiornoInizio = (DateTime)selectReader["PeriodoSoggiornoInizio"];
                        prenotazione.PeriodoSoggiornoFine = (DateTime)selectReader["PeriodoSoggiornoFine"];
                        prenotazione.Caparra = (decimal)selectReader["Caparra"];
                        prenotazione.TariffaApplicata = (decimal)selectReader["TariffaApplicata"];
                        prenotazione.DettagliPrenotazione = (string)selectReader["DettagliPrenotazione"];
                        prenotazione.NumeroCameraId = (int)selectReader["NumeroCameraId"];
                        prenotazione.Anno = (int)selectReader["Anno"];

                        prenotazioni.Add(prenotazione);
                    }
                    selectReader.Close();
                }


            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return Json(prenotazioni, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTotalePrenotazioniPensioneCompleta()
        {
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);
            int totalePrenotazioniPensioneCompleta = 0;
            try
            {
                conn.Open();

                string findTotalQuery = @"
                SELECT COUNT(*) AS TotalePrenotazioni
                FROM Prenotazioni
                WHERE DettagliPrenotazione = 'Pensione Completa'
                GROUP BY DettagliPrenotazione
                ";

                SqlCommand findTotalCmd = new SqlCommand(findTotalQuery, conn);
                totalePrenotazioniPensioneCompleta = (int)findTotalCmd.ExecuteScalar();
            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return Json(totalePrenotazioniPensioneCompleta, JsonRequestBehavior.AllowGet);
        }
    }
}