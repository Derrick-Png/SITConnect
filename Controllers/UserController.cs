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
using System.Net.Http;
using SITConnect.Services;
using System.Text.RegularExpressions;

namespace SITConnect.Controllers
{
    [Route("User/")]
    public class UserController : Controller
    {
        
        // Storage
        private readonly IHostingEnvironment _env;
        private readonly UserDbContext _db;
        
        // Managers & Providers
        private readonly UserManager<User> _UManager;
        private readonly AesCryptoServiceProvider _AESCrypt = new();
        private readonly AuthyService _Authy;

        // Validators
        private PasswordValidator<User> passwordValidator = new();
        private HttpClient _HTTPClient;

        public UserController(
            IHostingEnvironment env,
            UserManager<User> uManager,
            AuthyService authy,
            UserDbContext db
            ) : base()
        {
            _AESCrypt.Padding = PaddingMode.PKCS7;
            _env = env;
            _UManager = uManager;
            _HTTPClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.google.com")
            };
            _Authy = authy;
            _db = db;
        }
        
        public async Task<IActionResult> Index()
        {

            
            try
            {
                var user = await _UManager.GetUserAsync(HttpContext.User);
                // Password Age Validation
                if (user.LastPasswordChangedDate.AddMinutes(2) < DateTime.Now)
                {
                    ModelState.AddModelError("Password Age", "Password needs to be changed after 2 Minutes");
                    return RedirectToAction("Change_Password");
                }

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

        [HttpGet("change_password")]
        public async Task<IActionResult> Change_Password()
        {
            var user = await _UManager.GetUserAsync(HttpContext.User);
            /* Authenticated Validation */
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost("change_password")]
        public async Task<IActionResult> Change_Password_Post(UserChangePasswordDTO form)
        {
            
            if (ModelState.IsValid)
            {
                var user = await _UManager.GetUserAsync(HttpContext.User);
                /* Authenticated Validation */
                if(user == null) 
                {
                    return RedirectToAction("Login", "Auth");
                }

                var form_check = true;
                /* CAPTCHA Validation */

                // Form POST data
                var post_data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", Environment.GetEnvironmentVariable("SITConnect.CAPTCHA", EnvironmentVariableTarget.Machine)),
                    new KeyValuePair<string, string>("response", form.token)
                };

                // Get Response
                HttpResponseMessage res_msg = await _HTTPClient.PostAsync("/recaptcha/api/siteverify", new FormUrlEncodedContent(post_data));
                res_msg.EnsureSuccessStatusCode();
                ReCAPTCHAResponse res = JsonConvert.DeserializeObject<ReCAPTCHAResponse>(await res_msg.Content.ReadAsStringAsync());
                Console.WriteLine($"Score: {res.Score}\nSuccess:{res.Success.ToString()}");
                if (res.Score < 0.5)
                {
                    ModelState.AddModelError("CAPTCHA", "Failed CAPTCHA");
                    form_check = false;
                }



                
                
                // Current Password Validation
                if (await _UManager.CheckPasswordAsync(user, form.Password) == false)
                {
                    ModelState.AddModelError("Password", "Current Password given is incorrect");
                    form_check = false;
                }

                // New Password Requirement Check
                var password_res = await passwordValidator.ValidateAsync(_UManager, user, form.Password);
                if(!password_res.Succeeded)
                {
                    ModelState.AddModelError("New Password", "New Password does not meet the requirements");
                    form_check = false;
                }
                
                // Confirm New Password Check
                if(!form.New_Password.Equals(form.Confirm_New_Password))
                {
                    ModelState.AddModelError("Confirm New Password", "Confirm New Password does not match");
                    form_check = false;
                }

                // Password Age Validation
                if(user.LastPasswordChangedDate.AddMinutes(2) > DateTime.Now)
                {
                    ModelState.AddModelError("Password Age", "Password cannot be change within 2 Minutes");
                    form_check = false;
                }

                /* Password History Validation */
                var user_hashs = _db.Hashs
                    .Where(item => item.user_id == user.Id)
                    .OrderBy(item => item.created_date)
                    .Take(2);
                foreach(var hash in user_hashs.ToList())
                {
                    var his_check = _UManager.PasswordHasher.VerifyHashedPassword(user, hash.hash, form.New_Password);
                    Console.WriteLine(his_check.ToString());
                    if (his_check == PasswordVerificationResult.Success)
                    {
                        ModelState.AddModelError("Password History", "Password cannot be same as the previous 2 passwords");
                        form_check = false;
                        break;
                    }
                }

                

                if(!form_check)
                {
                    return View("Change_Password", form);
                }

                // Reset Password
                var token = await _UManager.GeneratePasswordResetTokenAsync(user);
                var result = await _UManager.ResetPasswordAsync(user, token, form.New_Password);
                if(result.Succeeded)
                {
                    // Find User & Store Generated Password Hash in PasswordHash Db
                    var generated_user = await _UManager.FindByNameAsync(user.UserName);
                    await _db.Hashs.AddAsync(new PasswordHash()
                    {
                        id = Guid.NewGuid().ToString(),
                        user_id = user.Id,
                        hash = generated_user.PasswordHash,
                        created_date = DateTime.Now
                    });
                    await _db.SaveChangesAsync();

                    user.LastPasswordChangedDate = DateTime.Now;
                    await _UManager.UpdateAsync(user);

                    return RedirectToAction("Index", "User", new { status="success", message="Password has been reseted successfully"});
                }
                else
                {
                    ModelState.AddModelError("Generating Problem", "Error has occured while resetting password");
                    return View("Change_Password", form);
                }

            }
            else
            {
                return View("Change_Password", form);
            }
        }

        [HttpGet("images/{id}")]
        public async Task<IActionResult> images(string id)
        {
            var user = await _UManager.GetUserAsync(HttpContext.User);
            /* Authenticated Validation */
            if (user == null)
            {
                 return RedirectToAction("Login", "Auth");
            }
            try
            {
                var file = "/user/images/" + id + Path.GetExtension(Directory.GetFiles(Path.Combine(_env.WebRootPath, "user/images/"), $"{id}.*")[0]);
                HttpContext.Response.Redirect(file);
                
            }
            catch
            {
                
            }
            return Ok();
        }
        [HttpGet("Crash")]
        public IActionResult Crash()
        {
            User user = null;
            // Accessing a property of an object (in this case email) that does not have an instance
            Console.WriteLine(user.Email);
            return View();
        }

        [HttpGet("Add_2FA")]
        public async Task<IActionResult> Add_2FA()
        {
            var user = await _UManager.GetUserAsync(HttpContext.User);
            /* Authenticated Validation */
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost("Add_2FA")]
        public async Task<IActionResult> Add_2FA_Post(UserAdd2FADTO form)
        {
            var user = await _UManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            // Password Age Validation
            if(user.LastPasswordChangedDate.AddMinutes(2) > DateTime.Now)
            {
                return RedirectToAction("Change_Password");
            }
            if (ModelState.IsValid)
            {
                var form_check = true;

                /* CAPTCHA Validation */

                // Form POST data
                var post_data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", Environment.GetEnvironmentVariable("SITConnect.CAPTCHA", EnvironmentVariableTarget.Machine)),
                    new KeyValuePair<string, string>("response", form.token)
                };

                // Get Response
                HttpResponseMessage res_msg = await _HTTPClient.PostAsync("/recaptcha/api/siteverify", new FormUrlEncodedContent(post_data));
                res_msg.EnsureSuccessStatusCode();
                ReCAPTCHAResponse res = JsonConvert.DeserializeObject<ReCAPTCHAResponse>(await res_msg.Content.ReadAsStringAsync());
                Console.WriteLine($"Score: {res.Score}\nSuccess:{res.Success.ToString()}");
                if (res.Score < 0.5)
                {
                    ModelState.AddModelError("CAPTCHA", "Failed CAPTCHA");
                    form_check = false;
                }

                /* Country Code Validation*/
                if (UserAdd2FADTO.countries.Where(item => item.Value == form.country_code).FirstOrDefault() == null)
                {
                    ModelState.AddModelError("Country Code", "Invalid country code");
                    form_check = false;
                }

                /* Phone Number Validation*/
                if (!(form.phone_no.Length >= 4 && form.phone_no.Length <= 12))
                {
                    ModelState.AddModelError("Phone number", "Phone Number's length is between 4 and 12");
                    form_check = false;
                }
                if (!(Regex.Match(form.phone_no, @"^[0-9]*$").Success))
                {
                    ModelState.AddModelError("Phone number", "This is not a phone number");
                    form_check = false;
                }
                if (!form_check)
                {
                    return View("Add_2FA", form);
                }

                // Creating user on Authy
                user.authy_id = await _Authy.registerUser(user.Email, form);
                user.country_code = form.country_code;
                user.phone_no = form.phone_no;

                // Updating user with details
                await _UManager.UpdateAsync(user);

                return RedirectToAction("Index", "User");
            }
            else
            {
                return View("Add_2FA", form);
            }
        }
    }
}
