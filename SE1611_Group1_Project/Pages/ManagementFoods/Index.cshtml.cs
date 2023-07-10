using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.Pages.ManagementFoods
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string FoodApiUrl = "";
        private readonly string CategoryApiUrl = "";
        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FoodApiUrl = "https://localhost:7203/api/Foods";
            CategoryApiUrl = "https://localhost:7203/api/Categories";
        }
        public string searchInput { get; set; } = string.Empty;
        [BindProperty]
        public int  categoryChoose { get; set; } = default!;

        public IList<FoodDTO> Food { get;set; } = default!;
        public IList<CategoryDTO> Category { get; set; } = default;
        public int sort { get; set; } = 0;

        public async Task OnGetAsync(string searchInput,int categoryChoose,int sort)
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                Response.Redirect("/Auth/Login");
            }
            else if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("Role") != 1)
            {
                Response.Redirect("/Auth/403");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(FoodApiUrl);
            string strDataFood = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var foodLists = JsonSerializer.Deserialize<List<FoodDTO>>(strDataFood, options);
            responseMessage = await client.GetAsync(CategoryApiUrl);
            string strDataCate = await responseMessage.Content.ReadAsStringAsync();
            var cateLists = JsonSerializer.Deserialize<List<CategoryDTO>>(strDataCate, options);
            if (foodLists != null && cateLists != null)
            {
                Food = foodLists;
                Category = cateLists;
                if (!string.IsNullOrEmpty(searchInput))
                {
                    Food = Food.Where(food => food.FoodName.Contains(searchInput)).ToList();
                }
                var cc = categoryChoose;
                if (cc != 0)
                {
                    Food = Food.Where(f => f.CategoryId == cc).ToList();
                }
                if (sort == 0)
                {
                    sort = 1;
                    Food = Food.OrderBy(p => p.FoodPrice).ToList();

                        Food = Food.OrderByDescending(p => p.FoodPrice).ToList();
                    
                }
            }

        }
        public void SortByPrice()
        {
            Food = Food.OrderBy(p => p.FoodPrice).ToList();
        }

    }
}
