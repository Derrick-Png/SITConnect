using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SITConnect.Models;

namespace SITConnect.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var user = new User();
            try
            {
                user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return View(user);
        }

        public void images(string id)
        {

            HttpContext.Response.Redirect($"/user/images/{id}.png");
            return;
        }
    }
}
