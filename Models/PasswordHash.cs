using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Models
{
    [Table("PasswordHash")]
    public class PasswordHash
    {
        [Key]
        public string id { get; set; }
        public string user_id { get; set; }
        public string hash { get; set; }
        public DateTime created_date{ get; set; }

    }
}
