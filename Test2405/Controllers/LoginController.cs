using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Test2405.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Test2405.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View(UserData.Users);
        }

        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }
    }
}