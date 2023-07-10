using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
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
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string FoodApiUrl = "";

        public DeleteModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FoodApiUrl = "https://localhost:7203/api/Foods";
        }

        [BindProperty]
      public FoodDTO Food { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            HttpResponseMessage responseMessage = await client.GetAsync(FoodApiUrl);
            string strDataFood = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var foodLists = JsonSerializer.Deserialize<List<FoodDTO>>(strDataFood, options);
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                Response.Redirect("/Auth/Login");
            }
            else if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("Role") != 1)
            {
                Response.Redirect("/Auth/403");
            }
            if (id == null || foodLists == null)
            {
                return NotFound();
            }

            var food = foodLists.FirstOrDefault(m => m.FoodId == id);

            if (food == null)
            {
                return NotFound();
            }
            else 
            {
                Food = food;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            string url = FoodApiUrl + "/" + id;
            Food.FoodStatus = 0;
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(Food), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PutAsync(url, httpContent);
            return RedirectToPage("./Index");
        }
    }
}
