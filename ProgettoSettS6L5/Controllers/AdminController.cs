using ProgettoSettS6L5.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace ProgettoSettS6L5.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);
            List<Prenotazione> prenotazioni = new List<Prenotazione>();
            List<Cliente> clienti = new List<Cliente>();

            try
            {
                conn.Open();

                string selectFromPrenotazioniQuery = "SELECT CodiceFiscale, Cognome, Nome FROM Clienti";
                SqlCommand selectFromPrenotazioniCmd = new SqlCommand(selectFromPrenotazioniQuery, conn);

                SqlDataReader selectReader = selectFromPrenotazioniCmd.ExecuteReader();

                if (selectReader.HasRows)
                {
                    while (selectReader.Read())
                    {
                        Cliente cliente = new Cliente();
                        cliente.CodiceFiscale = (string)selectReader["CodiceFiscale"];
                        cliente.Cognome = (string)selectReader["Cognome"];
                        cliente.Nome = (string)selectReader["Nome"];

                        clienti.Add(cliente);


                    }

                    selectReader.Close();


                    ViewBag.Clienti = clienti;
                    return View();
                }
            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return View();
        }


        [HttpPost]
        public ActionResult Index(int selectIdPrenotazione)
        {
            // TODO - FARE Roba per checkout qui o in checkout!
            // Prendi tutta prenotazione per id
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            Prenotazione prenotazione = new Prenotazione();
            // List<object> serviziTotaliPerPrenotazione = new List<object>();
            List<DettagliServizio> serviziTotaliPerPrenotazione = new List<DettagliServizio>();

            try
            {
                conn.Open();

                string selectPrenotazioneByIdQuery = "SELECT * FROM Prenotazioni WHERE Id = @id";
                SqlCommand selectCmd = new SqlCommand(selectPrenotazioneByIdQuery, conn);
                selectCmd.Parameters.AddWithValue("id", selectIdPrenotazione);

                SqlDataReader prenotazioneReader = selectCmd.ExecuteReader();

                if (prenotazioneReader.HasRows)
                {
                    prenotazioneReader.Read();
                    prenotazione.Id = (int)prenotazioneReader["Id"];
                    prenotazione.CodiceFiscaleCliente = prenotazioneReader["CodiceFiscaleCliente"].ToString();
                    prenotazione.PeriodoSoggiornoInizio = (DateTime)prenotazioneReader["PeriodoSoggiornoInizio"];
                    prenotazione.PeriodoSoggiornoFine = (DateTime)prenotazioneReader["PeriodoSoggiornoFine"];
                    prenotazione.NumeroCameraId = (int)prenotazioneReader["NumeroCameraId"];
                    prenotazione.Caparra = (decimal)prenotazioneReader["Caparra"];
                    prenotazione.TariffaApplicata = (decimal)prenotazioneReader["TariffaApplicata"];

                }
                prenotazioneReader.Close();

                string selectAllServiziByPrenotazioneIdQuery = @"SELECT PrenotazioneId, Tipologia, SUM(Quantita) AS TotaleQuantita, 
                    SUM(Prezzo) AS TotalePrezzo
                    FROM Servizi
                    WHERE PrenotazioneId = @prenId
                    GROUP BY PrenotazioneId, Tipologia
                    ";
                SqlCommand selectAllServiziCmd = new SqlCommand(selectAllServiziByPrenotazioneIdQuery, conn);
                selectAllServiziCmd.Parameters.AddWithValue("prenId", selectIdPrenotazione);

                SqlDataReader serviziReader = selectAllServiziCmd.ExecuteReader();
                decimal prezzoTotaleServizi = 0;
                if (serviziReader.HasRows)
                {
                    while (serviziReader.Read())
                    {
                        DettagliServizio servizio = new DettagliServizio()
                        {
                            PrenotazioneId = (int)serviziReader["PrenotazioneId"],
                            Tipologia = (string)serviziReader["Tipologia"],
                            TotaleQuantita = (int)serviziReader["TotaleQuantita"],
                            TotalePrezzo = (decimal)serviziReader["TotalePrezzo"]
                        };

                        serviziTotaliPerPrenotazione.Add(servizio);
                        prezzoTotaleServizi += (decimal)serviziReader["TotalePrezzo"];
                    }
                    serviziReader.Close();
                }

                int numeroTotaleGiorni = (int)(prenotazione.PeriodoSoggiornoFine - prenotazione.PeriodoSoggiornoInizio).TotalDays + 1;
                decimal importoTotale = ((numeroTotaleGiorni * prenotazione.TariffaApplicata) - prenotazione.Caparra) + prezzoTotaleServizi;

                TempData["Prenotazione"] = prenotazione;
                TempData["ListaServizi"] = serviziTotaliPerPrenotazione;


                return RedirectToAction("Checkout", new { importoTotale });
            }
            catch
            {
                return RedirectToAction("Index");
            }
            finally { conn.Close(); }


        }

        public ActionResult AggiungiUtenteEPrenotazione()
        {
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            List<Camera> camere = new List<Camera>();

            try
            {
                conn.Open();
                string selectAllCameraQuery = "SELECT IdNumeroCamera FROM Camere";
                SqlCommand selectAllCameraCmd = new SqlCommand(selectAllCameraQuery, conn);

                SqlDataReader allCameraReader = selectAllCameraCmd.ExecuteReader();

                if (allCameraReader.HasRows)
                {
                    while (allCameraReader.Read())
                    {
                        Camera camera = new Camera();
                        camera.IdNumeroCamera = (int)allCameraReader["IdNumeroCamera"];

                        camere.Add(camera);
                    }
                    ViewBag.Camere = camere;
                    return View();
                }

            }
            catch (Exception ex)
            {

            }
            finally { conn.Close(); }
            return View();
        }

        public ActionResult AggiungiUtente() { return View(); }
        [HttpPost]
        public ActionResult AggiungiUtente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                TempData["Cliente"] = cliente;
                return RedirectToAction("RegistraCliente", "Prenotazione");
            }
            return View();
        }
        public ActionResult AggiungiPrenotazione()
        {
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            List<Camera> camere = new List<Camera>();
            List<Cliente> clienti = new List<Cliente>();

            try
            {
                conn.Open();
                string selectAllCameraQuery = "SELECT IdNumeroCamera FROM Camere";
                SqlCommand selectAllCameraCmd = new SqlCommand(selectAllCameraQuery, conn);

                SqlDataReader allCameraReader = selectAllCameraCmd.ExecuteReader();

                if (allCameraReader.HasRows)
                {
                    while (allCameraReader.Read())
                    {
                        Camera camera = new Camera();
                        camera.IdNumeroCamera = (int)allCameraReader["IdNumeroCamera"];

                        camere.Add(camera);
                    }
                    ViewBag.Camere = camere;

                }
                allCameraReader.Close();

                string selectAllCodiceFiscaleQuery = "SELECT CodiceFiscale, Nome, Cognome FROM Clienti";
                SqlCommand selectCmd = new SqlCommand(selectAllCodiceFiscaleQuery, conn);

                SqlDataReader selectReader = selectCmd.ExecuteReader();
                if (selectReader.HasRows)
                {
                    while (selectReader.Read())
                    {
                        Cliente cliente = new Cliente();
                        cliente.CodiceFiscale = (string)selectReader["CodiceFiscale"];
                        cliente.Cognome = (string)selectReader["Cognome"];
                        cliente.Nome = (string)selectReader["Nome"];

                        clienti.Add(cliente);
                    }
                    selectReader.Close();
                    ViewBag.Clienti = clienti;
                    return View();
                }
            }
            catch (Exception ex)
            {

            }
            finally { conn.Close(); }


            return View();
        }
        public ActionResult AggiungiServizi()
        {
            //carica utenti in qualche modo nella view
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            List<Prenotazione> prenotazioni = new List<Prenotazione>();

            try
            {
                conn.Open();

                string selectFromPrenotazioniQuery = "SELECT Id, CodiceFiscaleCliente FROM Prenotazioni";
                SqlCommand selectFromPrenotazioniCmd = new SqlCommand(selectFromPrenotazioniQuery, conn);

                SqlDataReader selectReader = selectFromPrenotazioniCmd.ExecuteReader();

                if (selectReader.HasRows)
                {
                    while (selectReader.Read())
                    {
                        Prenotazione prenotazione = new Prenotazione();
                        prenotazione.Id = (int)selectReader["Id"];
                        prenotazione.CodiceFiscaleCliente = selectReader["CodiceFiscaleCliente"].ToString();

                        prenotazioni.Add(prenotazione);
                    }

                    selectReader.Close();

                    ViewBag.Prenotazioni = prenotazioni;
                    return View();
                }


            }
            catch (Exception ex)
            {

            }
            finally { conn.Close(); }

            return View();
        }


        public ActionResult Checkout(decimal importoTotale)
        {

            Prenotazione prenotazione = TempData["Prenotazione"] as Prenotazione;
            List<DettagliServizio> listaServizi = TempData["ListaServizi"] as List<DettagliServizio>;

            ViewBag.Prenotazione = prenotazione;
            ViewBag.ListaServizi = listaServizi;
            ViewBag.ImportoTotale = importoTotale;

            return View();
        }


    }
}