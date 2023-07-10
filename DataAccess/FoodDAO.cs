using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FoodDAO
    {
        public static List<Food> GetFoods()
        {
            var Foods = new List<Food>();
            try
            {
                using (var context = new FoodOrderContext())
                {
                    Foods = context.Foods.Include(x => x.Category).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Foods;
        }

        public static Food GetFoodById(int id)
        {
            Food Food = new Food();
            try
            {
                using (var context = new FoodOrderContext())
                {
                    Food = context.Foods.Include(x => x.Category).SingleOrDefault(x => x.FoodId == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Food;
        }

        public static void SaveFood(Food Food)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.Foods.Add(Food);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateFood(Food Food)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.Entry<Food>(Food).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteFood(Food Food)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    var p1 = context.Foods.SingleOrDefault(
                        c => c.FoodId == Food.FoodId);
                    context.Foods.Remove(p1);
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
