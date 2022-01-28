using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace SITConnect.Models
{
    public class User
    {
        public Guid id { get; set; }
        [Required]
        public string fname { get; set; }

        [Required]
        public string lname { get; set; }

        [Required]
        public string cc { get; set; } // Should be Encrypted

        [Required]
        public string email { get; set; }

        [Required]
        public string password{ get; set; }

        [Required]
        public DateTime dob { get; set; }
        
    }
}
