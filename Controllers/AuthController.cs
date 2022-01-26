using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SITConnect.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("Register2")]
        public IActionResult Register2()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register_Post()
        {

            return RedirectToAction("Login","Auth");
        }
    }
}
