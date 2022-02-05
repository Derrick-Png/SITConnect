﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace SITConnect.Models
{
    public class User : IdentityUser
    {
        [Required]
        [PersonalData]
        public string fname { get; set; }

        [Required]
        [PersonalData]
        public string lname { get; set; }

        [Required]
        [PersonalData]
        public string cc { get; set; } // Should be Encrypted

        [Required]
        [PersonalData]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime dob { get; set; }
        
    }
}
