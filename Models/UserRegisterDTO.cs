using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Models
{
    public class UserRegisterDTO 
    {
        public IFormFile profile_pic;

        public Guid id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "First Name can only contain alphabets")]
        public string fname { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Last Name can only contain alphabets")]
        public string lname { get; set; }

        [Required]
        [CreditCard]
        public string cc { get; set; } // Should be Encrypted

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime dob { get; set; }

        [Required]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Format for Email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        
    }
}
