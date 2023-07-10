using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        public User GetByUsernameAndPassword(string username, string password) => UserDAO.GetUserByUsernameAndPassword(username, password);

        public List<User> GetUsers() => UserDAO.GetUsers();

        public void SaveUser(User user) => UserDAO.SaveUser(user);

        public void UpdateUser(User user) => UserDAO.UpdateUser(user);
        
    }
}
