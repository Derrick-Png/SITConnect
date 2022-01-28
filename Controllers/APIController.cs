using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SITConnect.Services;
using SITConnect.Models;

namespace SITConnect.Controllers
{
    [Route("/api")]
    [ApiController]
    public class APIController : Controller
    {

        private UserService _db;
        public APIController(UserService user_db)
        {
            _db = user_db;
        }
        public List<User> Index()
        {
            return _db.retrieveUsers();
        }

    }
}
