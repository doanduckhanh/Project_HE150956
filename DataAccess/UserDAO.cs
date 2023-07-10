using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO
    {
        public static List<User> GetUsers()
        {
            var users = new List<User>();
            try
            {
                using (var context = new FoodOrderContext())
                {
                    users = context.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }


        public static void SaveUser(User User)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.Users.Add(User);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateUser(User User)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.Entry<User>(User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User GetUserByUsernameAndPassword(string username, string password)
        {
            User user = new User();
            try
            {
                using (var context = new FoodOrderContext())
                {
                    user = context.Users.FirstOrDefault(u => u.UserName.Equals(username) && u.Password.Equals(password));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
    }
}
