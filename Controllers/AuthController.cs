using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SITConnect.Services;
using SITConnect.Models;

namespace SITConnect.Controllers
{
    public class AuthController : Controller
    {

        private UserService _db;
        public AuthController(UserService user_db)
        {
            _db = user_db;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login_Post(string email, string password)
        {
            User founduser = _db.findUserByEmail(email);
            if (founduser != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
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
        public async Task<IActionResult> Register_Post(User user)
        {
            user.id = Guid.NewGuid();
            // Create user
            await _db.insertUser(user);
            return RedirectToAction("Login","Auth");
        }

    }
}
