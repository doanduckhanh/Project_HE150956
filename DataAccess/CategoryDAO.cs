using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        public static List<Category> GetCategorys()
        {
            var Categorys = new List<Category>();
            try
            {
                using (var context = new FoodOrderContext())
                {
                    Categorys = context.Categories.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Categorys;
        }


        public static void SaveCategory(Category Category)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.Categories.Add(Category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateCategory(Category Category)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.Entry<Category>(Category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteCategory(Category Category)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.Entry<Category>(Category).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
