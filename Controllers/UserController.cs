using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SITConnect.Models;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SITConnect.Controllers
{
    public class UserController : Controller
    {
        private readonly AesCryptoServiceProvider _AESCrypt = new();

        private readonly IHostingEnvironment _env;

        private readonly UserManager<User> _UManager;
        public UserController(IHostingEnvironment env, UserManager<User> uManager) : base()
        {
            _AESCrypt.Padding = PaddingMode.PKCS7;
            _env = env;
            _UManager = uManager;
        }
        
        public async Task<IActionResult> Index()
        {

            
            try
            {
                var user = await _UManager.GetUserAsync(HttpContext.User);
                Console.WriteLine(user.UserName + " is here");

                if (HttpContext.Session.GetString("user") != null && Request.Cookies["AuthToken"] == HttpContext.Session.GetString("AuthToken"))
                {
                    user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));

                    // Credit Card Decrypt [Output into Audit log, for demo]
                    ICryptoTransform dec = _AESCrypt.CreateDecryptor(
                        Encoding.Default.GetBytes(user.cc_Key),
                        Encoding.Default.GetBytes(user.cc_IV)
                        );
                    using (MemoryStream mem_enc = new (user.cc))
                    {
                        using (CryptoStream crypt_enc = new(mem_enc, dec, CryptoStreamMode.Read))
                        {
                            using (StreamReader sw_enc = new(crypt_enc))
                            {
                                string decrypted_cc = sw_enc.ReadToEnd();
                                Console.WriteLine($"Decrypted Credit Card: {decrypted_cc}");
                            }
                            
                        }
                    }

                    return View(user);
                }
            }
            catch(Exception ex)
            {

                Console.WriteLine(ex);
            }
            return RedirectToAction("Login", "Auth");
        }

        public void images(string id)
        {
            var file = "/user/images/" + id + Path.GetExtension(System.IO.Directory.GetFiles(Path.Combine(_env.WebRootPath, "user/images/"), $"{id}.*")[0]);
            HttpContext.Response.Redirect(file);
            return;
        }
    }
}
