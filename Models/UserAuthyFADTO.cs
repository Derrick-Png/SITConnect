using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Models
{
    public class UserChangePasswordDTO
    {

        [Required]
        public string Password { get; set; }

        [Required]
        public string New_Password { get; set; }

        [Required]
        public string Confirm_New_Password { get; set; }

        [Required]
        public string token { get; set; }


    }
}
