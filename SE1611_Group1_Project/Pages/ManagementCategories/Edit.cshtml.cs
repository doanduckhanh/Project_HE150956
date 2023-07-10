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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.Pages.ManagementCategories
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string CategoryApiUrl = "";

        public EditModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CategoryApiUrl = "https://localhost:7203/api/Categories";
        }

        [BindProperty]
        public CategoryDTO Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            if (id == null)
            {
                return NotFound();
            }
            string url = CategoryApiUrl + "?$filter=CategoryId eq " + id;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            string strData = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<CategoryDTO> categoryList = JsonSerializer.Deserialize<List<CategoryDTO>>(strData,options);
            CategoryDTO category = categoryList.FirstOrDefault();
            
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string url = CategoryApiUrl + "/" + Category.CategoryId;
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(Category), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PutAsync(url, httpContent);

            return RedirectToPage("./Index");
        }
    }
}
