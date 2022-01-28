using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SITConnect.Models
{
    public class UserDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public UserDbContext(IConfiguration conf, DbContextOptions<UserDbContext> options) : base(options)
        {
            _config = conf;
        }

        public DbSet<User> Users { get; set; }
    }
}
