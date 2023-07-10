using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SE1611_Group1_Project.Pages.Authen
{
    public class ProfileModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string UserApiUrl = "";
        public ProfileModel() {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "https://localhost:7203/api/Users/";
        }

        [BindProperty]
        public UserDTO User { get; set; } = default!;
        public string Msg { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var user = JsonSerializer.Deserialize<UserDTO>(HttpContext.Session.GetString("Token"), options);
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            if (id == null || user == null)
            {
                return NotFound();
            }
            if (user == null)
            {
                return NotFound();
            }
            User = user;
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(User), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PutAsync(UserApiUrl, httpContent);

            Msg = "Update profile successfull!";
            return Page();
        }

    }
}
