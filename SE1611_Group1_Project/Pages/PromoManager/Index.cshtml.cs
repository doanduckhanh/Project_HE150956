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

namespace SE1611_Group1_Project.PromoManager
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client;
        private readonly string PromoApiUrl = "";

        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PromoApiUrl = "https://localhost:7203/api/Promos";
        }

        public List<PromoDTO> Promo { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            HttpResponseMessage responseMessage = await client.GetAsync(PromoApiUrl);
            string strdata = await responseMessage.Content.ReadAsStringAsync();
            Promo = JsonSerializer.Deserialize<List<PromoDTO>>(strdata,options);
            return Page();
        }
    }
}
