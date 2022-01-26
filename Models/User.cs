using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Models
{
    public class User
    {
        public string fname { get; set; }
        public string lname { get; set; }

        public string cc { get; set; } // Should be Encrypted
        public string email { get; set; }
        public string password{ get; set; }
        public DateTime dob { get; set; }
        
    }
}
