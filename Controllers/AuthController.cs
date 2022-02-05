using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SITConnect.Controllers
{
    public class AuthController : Controller
    {

        private UserService _db;
        private readonly IHostingEnvironment _env;

        private readonly UserManager<User> _UManager;
        private readonly SignInManager<User> _SIManager;

        public AuthController(UserService user_db, 
            IHostingEnvironment env, 
            UserManager<User> uManager,
            SignInManager<User> siManager)
        {
            _db = user_db;
            _env = env;
            _UManager = uManager;
            _SIManager = siManager;
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
                var founduser = await _UManager.FindByEmailAsync(form.Email);
                if (founduser != null && !founduser.EmailConfirmed)
                {
                    ModelState.AddModelError("warning", "Please Verify Your Email First");
                }
                else if (await _UManager.CheckPasswordAsync(founduser, form.Password) == false)
                {
                    ModelState.AddModelError("error", "Invalid Credentials");
                }
                var result = await _SIManager.PasswordSignInAsync(founduser.UserName, form.Password, true, true);
                if(result.Succeeded)
                {
                    string serialized_user = JsonConvert.SerializeObject(founduser);
                    HttpContext.Session.SetString("user", serialized_user);
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
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = form.Email,
                    fname = form.fname,
                    lname = form.lname,
                    cc = form.cc, // TODO: Encrypt (With Key IV)
                    dob = form.dob,
                };
                user.UserName = Guid.NewGuid().ToString();
                form.profile_pic = profile_pic;
                if (form.profile_pic != null)
                {

                    Console.WriteLine($"Test test {form.profile_pic.FileName}");
                    // Console.WriteLine($"id:{user.id}\nemail:{user.email}\npassword:{user.password}");
                    try
                    {
                        System.Drawing.Image.FromStream(form.profile_pic.OpenReadStream()); // Read Image to Validate
                        var pfp_filepath = Path.Combine(_env.WebRootPath, "user", "images", user.UserName.ToString() + Path.GetExtension(form.profile_pic.FileName));
                        form.profile_pic.CopyTo(new FileStream(pfp_filepath, FileMode.Create));
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Invalid Image");
                        return View("Register", form);
                    }

                }
                    return View("Register", form);
                //}
                //var result = await _UManager.CreateAsync(user, form.Password);
                //if (result.Succeeded)
                //{

                //    string confirmationToken =  await _UManager.GenerateEmailConfirmationTokenAsync(user);

                //    string confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = confirmationToken }, HttpContext.Request.Scheme);

                //    var apiKey = Environment.GetEnvironmentVariable("SITConnect.SendGrid", EnvironmentVariableTarget.Machine);
                //    var fromEmail = Environment.GetEnvironmentVariable("SITConnect.SendGrid.FromEmail", EnvironmentVariableTarget.Machine);
                //    Console.WriteLine($"Using key {apiKey}\nUsing email {fromEmail}");
                //    var client = new SendGridClient(apiKey);
                //    var from = new EmailAddress(fromEmail, "SITConnect");
                //    var subject = "SITConnect by Derrick | Verification Link";
                //    var to = new EmailAddress(user.Email, user.fname);
                //    var plainTextContent = "and easy to do anywhere, even with C#";
                //    var htmlContent = $"Verification Link of your account For SITConnect is <a href='{confirmationLink}'>here</a>";
                //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                //    var response = await client.SendEmailAsync(msg);
                //    Console.WriteLine(response.Body.ReadAsStringAsync().Result);

                //    return RedirectToAction("Login", "Auth");
                //}
                //else
                //{
                //    if(result.Errors.Count() > 0)
                //    {
                //        foreach(var error in result.Errors)
                //        {
                //            ModelState.AddModelError("", error.Description);
                //        }
                //    }
                //    return View("Register", form);
                //}
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
                return RedirectToAction("Login","Auth", new { status="Error" });
            }
        }

        public IActionResult Sign_Out()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
