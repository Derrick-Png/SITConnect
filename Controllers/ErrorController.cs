using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Controllers
{
    [Route("/Error")]
    public class ErrorController : Controller
    {
        [HttpGet("404")]
        public IActionResult Not_Found()
        {
            return View();
        }

        [HttpGet("403")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpGet("500")]
        public IActionResult ISE()
        {
            return View();
        }
    }
}
