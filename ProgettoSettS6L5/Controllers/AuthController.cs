using ProgettoSettS6L5.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace ProgettoSettS6L5.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(Dipendente admin)
        {
            string connString = ConfigurationManager.ConnectionStrings["ProgettoSettS6L5"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            if (ModelState.IsValid)
            {

                try
                {
                    conn.Open();
                    string checkAdminQuery = "SELECT * FROM Dipendenti WHERE Username = @username AND Password = @password";

                    SqlCommand checkCmd = new SqlCommand(checkAdminQuery, conn);
                    checkCmd.Parameters.AddWithValue("username", admin.Username);
                    checkCmd.Parameters.AddWithValue("password", admin.Password);

                    SqlDataReader checkAdminReader = checkCmd.ExecuteReader();

                    if (checkAdminReader.HasRows)
                    {
                        checkAdminReader.Read();
                        FormsAuthentication.SetAuthCookie(checkAdminReader["Id"].ToString(), true);

                        return RedirectToAction("Index", "Admin");
                    }

                }
                catch (Exception ex) { }
                finally { conn.Close(); }
            }


            return View();
        }

        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}