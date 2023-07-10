using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1611_Group1_Project.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using BusinessObjects.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using ProjectManagementAPI.DTO;
using System.Collections.Generic;

namespace SE1611_Group1_Project.Pages.Authen
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string UserApiUrl = "";

        [BindProperty]   
        public string InputEmail { get; set; }
		public string MsgErr { get; set; }
        public ForgotPasswordModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "https://localhost:7203/api/Users/";
        }
		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPost()
		{
			string url = UserApiUrl + "?$filter=email eq '" + InputEmail+"'";
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            string strData = await responseMessage.Content.ReadAsStringAsync();
            if (!strData.Equals("[]"))
			{
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                List<UserDTO>  user = JsonSerializer.Deserialize<List<UserDTO>>(strData,options);
                // Generate a new password for the user
                var newPassword = GenerateNewPassword();

				// Save the new password to the database
				UserDTO userDTO = user.FirstOrDefault(x => x.Email.Equals(InputEmail));
				userDTO.Password = newPassword;
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(userDTO), Encoding.UTF8, "application/json");
                responseMessage = await client.PostAsync(UserApiUrl, httpContent);
                // Send the new password to the user's email address
                SendPasswordEmail(userDTO.Email, newPassword);

				return RedirectToPage("/Auth/Login");
			}
			MsgErr = "Email not valid!!";
			return Page();
		}

		private string GenerateNewPassword()
		{
			const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=";
			const int passwordLength = 8;

			var random = new Random();
			var newPassword = new StringBuilder();

			for (int i = 0; i < passwordLength; i++)
			{
				int index = random.Next(allowedChars.Length);
				newPassword.Append(allowedChars[index]);
			}

			return newPassword.ToString();
		}
		private void SendPasswordEmail(string email, string newPassword)
		{
			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential("managementcoffeeG1@gmail.com", "xdswzxbizecjpskr"),
				//chatgpt.1202@gmail.com
				EnableSsl = true,
				UseDefaultCredentials = false
			};

			var message = new MailMessage(
				"managementcoffeeG1@gmail.com",
				email,
				"Reset Password Management Coffee",
				$"{newPassword} là mật khẩu mới được cung cấp dành riêng cho bạn từ hệ thống Management Coffee. Hãy sử dụng mật khẩu này để đăng nhập và thay đổi mật khẩu của bạn!");

			smtpClient.Send(message);
		}
	}
}
