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

            try
            {
                conn.Open();
                string selectFromClienti = "SELECT CodiceFiscale, Cognome, Nome FROM Clienti";

            }
            catch (Exception ex)
            {

            }
            finally { conn.Close(); }

            return View();
        }


    }
}