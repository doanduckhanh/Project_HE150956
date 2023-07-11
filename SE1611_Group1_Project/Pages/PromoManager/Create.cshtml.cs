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
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.PromoManager
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient client;
        private readonly string PromoApiUrl = "";

        public CreateModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PromoApiUrl = "https://localhost:7203/api/Promos";
        }

        public IActionResult OnGet()
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
            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public Promo Promo { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || Promo == null)
            {
                return Page();
            }

            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(Promo), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(PromoApiUrl, httpContent);
            string strdata = await responseMessage.Content.ReadAsStringAsync();

            return RedirectToPage("./Index");
        }
    }
}
