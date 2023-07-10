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
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.Pages.Auth
{
    public class CreateModel : PageModel
    {
		private readonly HttpClient client = null;
		private readonly string UserApiUrl = "";
		public CreateModel()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			UserApiUrl = "https://localhost:7203/api/Users/";
		}

		[BindProperty]
        public NewUserDTO User { get; set; } = default!;


        public IActionResult OnGet()
        { 
            return Page();
        }




        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(User), Encoding.UTF8, "application/json");
			HttpResponseMessage responseMessage = await client.PostAsync(UserApiUrl,httpContent);
            return RedirectToPage("/Auth/Login");
        }
    }
}
