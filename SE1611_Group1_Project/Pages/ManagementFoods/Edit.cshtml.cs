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
using SE1611_Group1_Project.FileUploadService;
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.Pages.ManagementFoods
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string FoodApiUrl = "";
        private readonly string CategoryApiUrl = "";
        private readonly IFileUploadService fileUploadService;
        private readonly ILogger<IndexModel> _logger;

        public EditModel(IFileUploadService fileUploadService, ILogger<IndexModel> logger)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FoodApiUrl = "https://localhost:7203/api/Foods";
            CategoryApiUrl = "https://localhost:7203/api/Categories";
            _logger = logger;
            this.fileUploadService = fileUploadService;
        }

        [BindProperty]
        public FoodDTO Food { get; set; } = default!;
        public List<CategoryDTO> listCategories { get; set; } = default!;

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
            string url = FoodApiUrl + "?$filter=FoodId eq " + id;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            string strDataFood = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var foodLists = JsonSerializer.Deserialize<List<FoodDTO>>(strDataFood, options);
            FoodDTO food = foodLists.FirstOrDefault();
            responseMessage = await client.GetAsync(CategoryApiUrl);
            string strDataCate = await responseMessage.Content.ReadAsStringAsync();
            var cateLists = JsonSerializer.Deserialize<List<CategoryDTO>>(strDataCate, options);
            if (foodLists == null)
            {
                return NotFound();
            }
            Food = food;
            ViewData["CategoryId"] = new SelectList(cateLists, "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file != null)
            {
                string filePath = await fileUploadService.UploadFileAsync(file);
                Food.FoodImage = filePath.Substring(filePath.IndexOf(@"\images"));
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(Food), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PutAsync(FoodApiUrl, httpContent);
                return RedirectToPage("./Index");
            }
            else
            {
                string url = FoodApiUrl + "/" + Food.FoodId;
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(Food), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PutAsync(url, httpContent);
                return RedirectToPage("./Index");
            }
           
        }
    }
}
