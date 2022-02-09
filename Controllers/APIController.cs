using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SITConnect.Services;
using SITConnect.Models;
using Microsoft.AspNetCore.Identity;

namespace SITConnect.Controllers
{
    [Route("/api")]
    [ApiController]
    public class APIController : Controller
    {

        private readonly UserManager<User> _UManager;
        public APIController(UserManager<User> uManager)
        {
            _UManager = uManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _UManager.GetUserAsync(HttpContext.User);
            
            if (user != null)
            {
                return Ok(_UManager.Users.ToList());
            }
            else
            {
                HttpContext.Response.StatusCode = 403;
                return Forbid();
            }
        }

    }
}
