using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFoodRepository
    {
        List<Food> GetFoods();
        void SaveFood(Food food);
        void UpdateFood(Food food);
    }
}
