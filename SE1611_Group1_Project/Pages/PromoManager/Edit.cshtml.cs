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

namespace SE1611_Group1_Project.PromoManager
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client;
        private readonly string PromoApiUrl = "";

        public EditModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PromoApiUrl = "https://localhost:7203/api/Promos";
        }

        [BindProperty(SupportsGet =true)]
        public PromoDTO Promo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
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
            if (id == null )
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            HttpResponseMessage responseMessage = await client.GetAsync(PromoApiUrl);
            string strdata = await responseMessage.Content.ReadAsStringAsync();
            List<PromoDTO> promos = JsonSerializer.Deserialize<List<PromoDTO>>(strdata, options);
            PromoDTO promo =  promos.FirstOrDefault(m => m.PromoCode == id);
            if (promo == null)
            {
                return NotFound();
            }
            Promo = promo;
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
            string url = PromoApiUrl + "/" + Promo.PromoCode;
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(Promo), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PutAsync(PromoApiUrl,httpContent);
            string strdata = await responseMessage.Content.ReadAsStringAsync();


            return RedirectToPage("./Index");
        }
    }
}
