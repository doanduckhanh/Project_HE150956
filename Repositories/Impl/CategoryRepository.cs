using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class CategoryRepository : ICategoryRepository
    {

        public List<Category> GetCategories() => CategoryDAO.GetCategorys();

        public void SaveCategory(Category category) => CategoryDAO.SaveCategory(category);

        public void UpdateCategory(Category category) => CategoryDAO.UpdateCategory(category);
    }
}
