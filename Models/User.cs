using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace SITConnect.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string fname { get; set; }

        [PersonalData]
        public string lname { get; set; }

        [PersonalData]
        public byte[] cc { get; set; }

        [PersonalData]
        public string cc_Key { get; set; }

        [PersonalData]
        public string cc_IV { get; set; }

        [PersonalData]
        public DateTime dob { get; set; }

        [PersonalData]
        public string country_code { get; set; }

        [PersonalData]
        public string phone_no { get; set; }

        [PersonalData]
        public string authy_id { get; set; }

        [PersonalData]
        public DateTime LastPasswordChangedDate { get; set; }


    }
}
