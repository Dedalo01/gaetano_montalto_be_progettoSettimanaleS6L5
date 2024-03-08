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
            catch (Exception ex) { }
            finally { conn.Close(); }

            return View();
        }


        [HttpPost]
        public ActionResult Index(int idPrenotazione)
        {
            // TODO - FARE Roba per checkout qui o in checkout!
            return View();
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

        public ActionResult Checkout()
        {

            return View();
        }


    }
}