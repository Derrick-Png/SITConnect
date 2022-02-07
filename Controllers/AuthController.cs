using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SITConnect.Services;
using SITConnect.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;

namespace SITConnect.Controllers
{
    public class AuthController : Controller
    {
        // Storage
        private UserService _db;
        private readonly IHostingEnvironment _env;

        // Providers/Managers
        private readonly UserManager<User> _UManager;
        private readonly SignInManager<User> _SIManager;
        private readonly AesCryptoServiceProvider _AESCrypt = new ();
        private readonly HttpClient _HTTPClient;

        //Validators
        private PasswordValidator<User> passwordValidator = new();
        
        

        public AuthController(UserService user_db,
            IHostingEnvironment env,
            UserManager<User> uManager,
            SignInManager<User> siManager
            )
        {
            // Set Padding for AES Cryptor
            _AESCrypt.Padding = PaddingMode.PKCS7;

            _db = user_db;
            _env = env;
            _UManager = uManager;
            _SIManager = siManager;
            _HTTPClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.google.com")
            };
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

        // Login (i)
        [HttpPost("Login")]
        public async Task<IActionResult> Login_Post(UserLoginDTO form)
        {
            if (ModelState.IsValid)
            {
                var form_check = true; // To decide whether if form is valid
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

                var founduser = await _UManager.FindByEmailAsync(form.Email);
                if (founduser != null && !founduser.EmailConfirmed)
                {
                    ModelState.AddModelError("warning", "Please Verify Your Email First");
                    form_check = false;
                }
                else if (await _UManager.CheckPasswordAsync(founduser, form.Password) == false)
                {
                    ModelState.AddModelError("error", "Invalid Credentials");
                    form_check = false;
                }

                if(!form_check)
                {
                    return View("Login", form);
                }

                // Parameters = (username, password, isPersistent, LockoutOnFailure)
                var result = await _SIManager.PasswordSignInAsync(founduser.UserName, form.Password, true, true); // Account Lockout Feature
                if (result.Succeeded)
                {
                    string serialized_user = JsonConvert.SerializeObject(founduser);
                    string access_token = Guid.NewGuid().ToString();

                    // Init User
                    HttpContext.Session.SetString("user", serialized_user);

                    // Session Validation [To prevent session fixation]
                    HttpContext.Session.SetString("AuthToken", access_token);
                    Response.Cookies.Append("AuthToken", access_token);

                    return RedirectToAction("Index", "User");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("error", "Account Is Locked Out");
                }
            }
            return View("Login", form);

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
        public async Task<IActionResult> Register_Post([FromForm] UserRegisterDTO form, IFormFile profile_pic)
        {
            

            form.profile_pic = profile_pic;
            if (ModelState.IsValid)
            {

                User user = new User
                {
                    Email = form.Email,
                    fname = form.fname,
                    lname = form.lname,
                    //cc = form.cc, 
                    dob = form.dob,
                };
                user.UserName = Guid.NewGuid().ToString();

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

                /* Image Validation*/
                if (form.profile_pic != null)
                {
                    try
                    {
                        System.Drawing.Image.FromStream(form.profile_pic.OpenReadStream()); // Read Image to Validate
                        var image_file_ext = Path.GetExtension(form.profile_pic.FileName);
                        if (image_file_ext != ".jpg" && image_file_ext != ".png")
                        {
                            ModelState.AddModelError("", "Invalid Image");
                            form_check = false;
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Invalid Image");
                        form_check = false;
                    }
                }

                /* Password Validation */
                // Not part of the model because it is not made from data annotation
                // Password Requirements are from Identity
                var password_result = await passwordValidator.ValidateAsync(_UManager, user, form.Password);
                if (!password_result.Succeeded)
                {
                    foreach (var error in password_result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    form_check = false;
                }

                if(!form_check)
                {
                    return View("Register", form);
                }

                /* Credit Encryption */

                // Generating Key and IV by getting a part of a Guid
                Random rand = new();
                user.cc_Key = Guid.NewGuid().ToString().Substring(rand.Next(0, 3), 32);
                user.cc_IV = Guid.NewGuid().ToString().Substring(rand.Next(0, 18), 16);

                // Create AES Encryptor with previously generated key and IV
                ICryptoTransform enc = _AESCrypt.CreateEncryptor(
                    Encoding.Default.GetBytes(user.cc_Key),
                    Encoding.Default.GetBytes(user.cc_IV)
                    );

                using (MemoryStream mem_enc = new())
                {
                    using (CryptoStream crypt_enc = new(mem_enc, enc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw_enc = new(crypt_enc))
                        {
                            sw_enc.Write(form.cc);
                        }
                        user.cc = mem_enc.ToArray();
                    }
                }


                // Create User
                var result = await _UManager.CreateAsync(user, form.Password);
                if (result.Succeeded)
                {

                    // Create Profile Picture
                    if (form.profile_pic != null)
                    {
                        var pfp_filepath = Path.Combine(_env.WebRootPath, "user", "images", user.UserName.ToString() + Path.GetExtension(form.profile_pic.FileName));
                        form.profile_pic.CopyTo(new FileStream(pfp_filepath, FileMode.Create));
                    }

                    //* Email Verification *//

                    // Generate Confirmation Token and Set up Link
                    string confirmationToken = await _UManager.GenerateEmailConfirmationTokenAsync(user);
                    string confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = confirmationToken }, HttpContext.Request.Scheme);

                    // Send Email through SendGrid
                    var apiKey = Environment.GetEnvironmentVariable("SITConnect.SendGrid", EnvironmentVariableTarget.Machine);
                    var fromEmail = Environment.GetEnvironmentVariable("SITConnect.SendGrid.FromEmail", EnvironmentVariableTarget.Machine);
                    Console.WriteLine($"Using key {apiKey}\nUsing email {fromEmail}");
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress(fromEmail, "SITConnect");
                    var subject = "SITConnect by Derrick | Verification Link";
                    var to = new EmailAddress(user.Email, user.fname);
                    var plainTextContent = "and easy to do anywhere, even with C#";
                    var htmlContent = $"Verification Link of your account For SITConnect is <a href='{confirmationLink}'>here</a>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                    Console.WriteLine(response.Body.ReadAsStringAsync().Result);

                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    return View("Register", form);
                }

            }
            else
            {
                return View("Register", form);
            }
        }
        [HttpGet("Verify")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            User user = _UManager.FindByIdAsync(userId).Result;
            IdentityResult res = await _UManager.ConfirmEmailAsync(user, token);
            if (res.Succeeded)
            {
                ViewBag.Message = "Email Confirmed Successfully, Please Proceed to Login.";
                return RedirectToAction("Login", "Auth", new { status = "Success" });
            }
            else
            {
                ViewBag.Message = "Error while confirming your email";
                return RedirectToAction("Login", "Auth", new { status = "Error" });
            }
        }

        public IActionResult Sign_Out()
        {

            HttpContext.Session.Clear();

            // Remove Session Cookie
            if (Request.Cookies[".AspNetCore.Session"] != null)
            {
                Response.Cookies.Delete(".AspNetCore.Session");
            }
            // Remove Auth Token
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies.Delete("AuthToken");
            }

            if (Request.Cookies[".AspNetCore.Identity.Application"] != null)
            {
                Response.Cookies.Delete(".AspNetCore.Identity.Application");
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
