using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Test2405.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Test2405.Controllers
{
    public class LoginController : Controller
    {
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Test2405.Models.UserModel userModel)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connected");
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("SELECT * FROM [User] WHERE Username = @user AND Password = @pass AND Domain = @domain",sqlCon);
                SqlParameter pUser = new SqlParameter("@user", userModel.Username);
                SqlParameter pPass = new SqlParameter("@pass", userModel.Password);
                SqlParameter pDomain = new SqlParameter("@domain", userModel.Domain);
                StrQuer.Parameters.Add(pUser);
                StrQuer.Parameters.Add(pPass);
                StrQuer.Parameters.Add(pDomain);
                SqlDataReader dr = StrQuer.ExecuteReader();
                if (dr.HasRows)
                {
                    //userModel.Username == "admin" && userModel.Password == "pass"
                    sqlCon.Close();
                    dr.Close();
                    HttpContext.Session.Set("Username",System.Text.Encoding.Unicode.GetBytes(userModel.Username));
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    sqlCon.Close();
                    userModel.LoginErrorMessage = "Wrong username or password";
                    return View("Index", userModel);
                }
            }
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }
    }
}