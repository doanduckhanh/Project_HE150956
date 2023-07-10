using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Configuration;
using ProjectManagementAPI;
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.Models;
using SE1611_Group1_Project.Pages.Foods;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SE1611_Group1_Project.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly FoodOrderContext _context = new FoodOrderContext();
        private readonly HttpClient client = null;
        private readonly string UserApiUrl = "";
        private readonly string CartApiUrl = "";
        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "https://localhost:7203/api/Users/";
            CartApiUrl = "https://localhost:7203/api/Carts/";
        }

        [BindProperty]
        public string UserName { get; set; }
		[BindProperty]
		public string Password { get; set; }
		public string Msg { get; set; }

		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync()
		{
            string url = UserApiUrl + "login";
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            loginRequestDTO.Username = UserName;
            loginRequestDTO.Password = Password;
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(loginRequestDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(url,httpContent);
            string strData = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            UserDTO user = JsonSerializer.Deserialize<UserDTO>(strData, options);

            if (user != null)
			{
				if (UserName.Equals(user.UserName, StringComparison.Ordinal) && Password.Equals(user.Password, StringComparison.Ordinal))
				{
					HttpContext.Session.SetString("Token", strData);
                    HttpContext.Session.SetString("Username", user.UserName);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetInt32("Role", (int)user.RoleId);
                    HttpContext.Session.SetString("Password", user.Password);
                    //-----------------------------
                    SettingsCart.UserName = HttpContext.Session.GetString("Username");
                    SettingsCart.CartId = SettingsCart.UserName;
                    //MigrateCart();
                    string urlGetCount = CartApiUrl + "GetCount/" + user.UserName;
                    responseMessage = await client.GetAsync(urlGetCount);
                    string strDataGetCount = await responseMessage.Content.ReadAsStringAsync();
                    int countItem = Int32.Parse(strDataGetCount);
                    HttpContext.Session.SetInt32("Count", countItem);
                    return RedirectToPage("/Index");
				}
				else
				{
					Msg = "Invalid email or password.";
					return Page();
				}
			}
			else
			{
				Msg = "Invalid email or password.";
				return Page();
			}
		}

    }
}
