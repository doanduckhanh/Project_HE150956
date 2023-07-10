using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        void SaveUser(User user);
        void UpdateUser(User user);
        User GetByUsernameAndPassword(string username, string password);
    }
}
