using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class FoodRepository : IFoodRepository
    {
        public List<Food> GetFoods() => FoodDAO.GetFoods();

        public void SaveFood(Food food) => FoodDAO.SaveFood(food);

        public void UpdateFood(Food food) => FoodDAO.UpdateFood(food);
    }
}
