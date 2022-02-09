using Newtonsoft.Json.Linq;
using SITConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SITConnect.Services
{
    public class AuthyService
    {
        private readonly HttpClient _HTTPClient;
        public AuthyService()
        {
            _HTTPClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.authy.com")
            };
            _HTTPClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _HTTPClient.DefaultRequestHeaders.Add("user-agent", "SITConnect");

            _HTTPClient.DefaultRequestHeaders.Add("X-Authy-API-Key", Environment.GetEnvironmentVariable("SITConnect.Authy", EnvironmentVariableTarget.Machine));
        }

        public async Task<string> registerUser(string email, UserAdd2FADTO form)
        {
            // Create User Obj for Authy API
            var user_obj = new Dictionary<string, string>()
            {
                { "email", email },
                { "country_code", form.country_code },
                { "cellphone", form.phone_no }
            };

            var post_data = new Dictionary<string, object>()
            {
                { "user" , user_obj}
            };

            var result = await _HTTPClient.PostAsJsonAsync("/protected/json/users/new", post_data);

            result.EnsureSuccessStatusCode();
            
            var res = await result.Content.ReadFromJsonAsync<Dictionary<string, object>>();

            var user_data = JObject.Parse(res["user"].ToString());

            return user_data["id"].ToString();
        }

        public async Task<string> phoneTokenVerificationRequest(string phone_no, string country_code)
        {
            var res = await _HTTPClient.PostAsync(
                  $"/protected/json/phones/verification/start?via=sms&country_code={country_code}&phone_number={phone_no}",
                  null
            );

            return await res.Content.ReadAsStringAsync();
            
        }

        public async Task<bool> verifyPhoneToken(string phone_no, string country_code, string code)
        {
            var res = await _HTTPClient.GetAsync(
                    $"/protected/json/phones/verification/check?phone_number={phone_no}&country_code={country_code}&verification_code={code}"
            );
            if(res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
