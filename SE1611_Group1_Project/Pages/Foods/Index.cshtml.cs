using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SE1611_Group1_Project.Pages.Foods
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string FoodApiUrl = "";
        private readonly string CategoryApiUrl = "";
        private readonly string CartApiUrl = "";
        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FoodApiUrl = "https://localhost:7203/api/Foods";
            CategoryApiUrl = "https://localhost:7203/api/Categories";
            CartApiUrl = "https://localhost:7203/api/Carts";
        }
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CategoryId { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public int IndexPaging { get; set; } = 1;

        [BindProperty(Name = "id", SupportsGet = true)]
        public int Id { get; set; }

        public int TotalPage { get; set; }
        public async Task<IActionResult> OnGet(int categoryId, string searchString, int indexPaging)
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                Response.Redirect("/Auth/Login");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(FoodApiUrl);
            string strDataFood = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<FoodDTO> foodDTOs = JsonSerializer.Deserialize<List<FoodDTO>>(strDataFood,options);
            PaginatedList<FoodDTO> foods = new PaginatedList<FoodDTO>(foodDTOs, foodDTOs.Count(), 1, 6);
            if (categoryId != 0)
            {
                var listFoods = String.IsNullOrEmpty(searchString) ? foodDTOs.Where(x => x.CategoryId == categoryId && x.FoodStatus != 0).ToList() : foodDTOs.Where(x => x.CategoryId == categoryId && x.FoodName.Contains(searchString) && x.FoodStatus != 0).ToList();
                foods = new PaginatedList<FoodDTO>(listFoods, listFoods.Count, 1, 6);
            }
            else
            {
                var listFoods = String.IsNullOrEmpty(searchString) ? foodDTOs.ToList() : foodDTOs.Where(x => x.FoodName.Contains(searchString) && x.FoodStatus != 0).ToList();
                foods = new PaginatedList<FoodDTO>(listFoods, listFoods.Count, 1, 6);
            }
            TotalPage = foods.TotalPages;
            responseMessage = await client.GetAsync(CategoryApiUrl);
            string strDataCate = await responseMessage.Content.ReadAsStringAsync();
            var cateLists = JsonSerializer.Deserialize<List<CategoryDTO>>(strDataCate,options);
            ViewData["categoryList"] = cateLists;
            ViewData["Product"] = PaginatedList<FoodDTO>.Create(foods.AsQueryable<FoodDTO>(), indexPaging, 6);
            return Page();
        }
        public async Task<IActionResult> OnPostAddToCart(int id)
        {
            var cartId = HttpContext.Session.GetString("Username");

            string url = CartApiUrl + "/AddToCart/" + id + "/" + cartId;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            string urlGetCount = CartApiUrl + "/GetCount/" + cartId;
            responseMessage = await client.GetAsync(urlGetCount);
            string strDataGetCount = await responseMessage.Content.ReadAsStringAsync();
            HttpContext.Session.SetInt32("Count", Int32.Parse(strDataGetCount));
            return RedirectToPage("/Foods/Index");
        }
    }
}
