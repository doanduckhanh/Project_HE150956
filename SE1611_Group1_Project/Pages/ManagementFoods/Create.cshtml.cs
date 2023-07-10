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
using Microsoft.Extensions.Options;
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.FileUploadService;
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.Pages.ManagementFoods
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string FoodApiUrl = "";
        private readonly string CategoryApiUrl = "";
        private readonly IFileUploadService fileUploadService;
        private readonly ILogger<IndexModel> _logger;
        public string filePath { get; set; } = default!;

        public CreateModel(IFileUploadService fileUploadService, ILogger<IndexModel> logger)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FoodApiUrl = "https://localhost:7203/api/Foods";
            CategoryApiUrl = "https://localhost:7203/api/Categories";
            this.fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(CategoryApiUrl);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string strDataCate = await responseMessage.Content.ReadAsStringAsync();
            var cateLists = JsonSerializer.Deserialize<List<CategoryDTO>>(strDataCate, options);
            ViewData["CategoryId"] = new SelectList(cateLists, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public FoodDTO Food { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile file)
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
            if (!ModelState.IsValid || foodLists == null || Food == null || Food.FoodPrice <=0)
            {
                return Page();
            }
          if(file!= null)
            {
                filePath = await fileUploadService.UploadFileAsync(file);
                Food.FoodImage = filePath.Substring(filePath.IndexOf(@"\images"));
            }
            else
            {
                return Page();
            }
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(Food), Encoding.UTF8, "application/json");
            responseMessage = await client.PostAsync(FoodApiUrl, httpContent);
    
            return RedirectToPage("./Index");
        }
    }
}
