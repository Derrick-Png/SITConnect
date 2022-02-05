using SITConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Services
{
    public class UserService
    {
        private readonly UserDbContext _db;
        public UserService(UserDbContext userdb)
        {
            _db = userdb;
        }

        public async Task<bool> insertUser(User user)
        {
            // TODO: Implement creating user
            try
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public User findUserByEmail(string email)
        {
            // Used in verifying duplicated users in registration
            // 
            return _db.Users.FirstOrDefault(user => user.Email == email);
            
        }

        public List<User> retrieveUsers() { return _db.Users.ToList();  }
    }
}
