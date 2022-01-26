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

        public bool insertUser(User user)
        {
            // TODO: Implement creating user
            try
            {
                _db.Users.Add(user);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public User? findUserByEmail(string email)
        {
            // Used in verifying duplicated users in registration
            // 

            User user = null;
            user = _db.Users.FirstOrDefault(user => user.email == email);
            return user;
        }
    }
}
