using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Models
{
    public class UserAuthyFADTO
    {

        [Required]
        public string code { get; set; }


    }
}
