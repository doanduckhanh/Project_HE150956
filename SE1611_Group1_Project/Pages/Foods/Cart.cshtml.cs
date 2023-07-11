using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using ProjectManagementAPI.DTO;
using SE1611_Group1_Project.Models;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace SE1611_Group1_Project.Pages.Foods
{
    public class CartModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly string CartApiUrl = "";
        private readonly string PromoApiUrl = "";
        [BindProperty(SupportsGet = true)]
        public decimal total { get; set; }
        public int countItem { get; set; }

        [BindProperty(SupportsGet =true)]
        public string Code { get; set; } = "NOGV";
        public CartModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CartApiUrl = "https://localhost:7203/api/Carts";
            PromoApiUrl = "https://localhost:7203/api/Promos";
        }
        [BindProperty]
        public List<CartDTO> Cart { get; set; } = default;
        public async Task<IActionResult> OnGet()
        {
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                Response.Redirect("/Auth/Login");
            }
            var cartid = HttpContext.Session.GetString("Username");
            string url = CartApiUrl + "/GetCart/" + cartid;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string strCart = await responseMessage.Content.ReadAsStringAsync();
            Cart = JsonSerializer.Deserialize<List<CartDTO>>(strCart, options);
            string urlTotal = CartApiUrl + "/GetTotal" + "/" + cartid + "/" + Code;
            responseMessage = await client.GetAsync(urlTotal);
            string strTotal = await responseMessage.Content.ReadAsStringAsync();
            total = Decimal.Parse(strTotal);
            string urlGetCount = CartApiUrl + "/GetCount/" + cartid;
            responseMessage = await client.GetAsync(urlGetCount);
            string strDataGetCount = await responseMessage.Content.ReadAsStringAsync();
            countItem = Int32.Parse(strDataGetCount);
            HttpContext.Session.SetInt32("Count", countItem);
            HttpContext.Session.SetString("Total", total.ToString());
            ViewData["Total"] = HttpContext.Session.GetString("Total");
            return Page();
        }
        public async Task<IActionResult> OnPostRemove(int id)
        {
            var cartid = HttpContext.Session.GetString("Username");
            string url = CartApiUrl + "/RemoveFromCart/" + cartid + "/" + id;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            return RedirectToPage("/Foods/Cart");
        }
        public async Task<IActionResult> OnPostRemoveFromCart(int id)
        {
            //var cartItem = _context.Carts.SingleOrDefault(
            //    c => c.CartId.Equals(SettingsCart.CartId)
            //    && c.RecordId == id);
            //if (cartItem != null)
            //{
            //    if (cartItem.Count > 1)
            //    {
            //        cartItem.Count--;
            //    }
            //    else
            //    {
            //        _context.Carts.Remove(cartItem);
            //    }
            //    // Save changes
            //    await _context.SaveChangesAsync();

            //}
            var cartid = HttpContext.Session.GetString("Username");
            string url = CartApiUrl + "/RemoveCart/" + cartid + "/" + id;
            HttpResponseMessage responseMessage = await client.GetAsync(url);           
            string urlGetCount = CartApiUrl + "/GetCount/" + cartid;
            responseMessage = await client.GetAsync(urlGetCount);
            string strDataGetCount = await responseMessage.Content.ReadAsStringAsync();
            countItem = Int32.Parse(strDataGetCount);
            HttpContext.Session.SetInt32("Count", countItem);

            return RedirectToPage("/Foods/Cart");
        }

        public async Task<IActionResult> OnPostGiveCode()
        {
            // Retrieve session data
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            ViewData["Total"] = HttpContext.Session.GetString("Total");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string url = PromoApiUrl + "?$filter=PromoCode eq '" + Code + "'";
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            string strData = await responseMessage.Content.ReadAsStringAsync();
            PromoDTO promo = JsonSerializer.Deserialize<List<PromoDTO>>(strData,options).FirstOrDefault();

            var cartid = HttpContext.Session.GetString("Username");
            string urlCart = CartApiUrl + "/GetCart/" + cartid;
            responseMessage = await client.GetAsync(urlCart);
            string strCart = await responseMessage.Content.ReadAsStringAsync();
            Cart = JsonSerializer.Deserialize<List<CartDTO>>(strCart, options);
            string urlTotal = CartApiUrl + "/GetTotal" + "/" + cartid + "/" + Code;
            responseMessage = await client.GetAsync(urlTotal);
            string strTotal = await responseMessage.Content.ReadAsStringAsync();
            total = Decimal.Parse(strTotal);
            HttpContext.Session.SetString("Total", total.ToString());
            ViewData["Total"] = HttpContext.Session.GetString("Total");


            if (promo == null)
            {
                ViewData["MyString"] = "Coupon code has expired or is incorrect";
            }
            else
            {
                ViewData["MyString"] = promo.PromoDescribe;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostIncreaseFromCart(int id)
        {
            var cartid = HttpContext.Session.GetString("Username");
            string url = CartApiUrl + "/IncreaseFromCart/"+cartid+"/"+id;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            string urlGetCount = CartApiUrl + "/GetCount/" + cartid;
            responseMessage = await client.GetAsync(urlGetCount);
            string strDataGetCount = await responseMessage.Content.ReadAsStringAsync();
            countItem = Int32.Parse(strDataGetCount);
            HttpContext.Session.SetInt32("Count", countItem);

            return RedirectToPage("/Foods/Cart");
        }

        public async Task<IActionResult> OnPostCheckOut()
        {
            var cartid = HttpContext.Session.GetString("Username");
            string url = CartApiUrl + "/GetCart/" + cartid;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string strCart = await responseMessage.Content.ReadAsStringAsync();
            Cart = JsonSerializer.Deserialize<List<CartDTO>>(strCart, options);
            // HttpContext.Session.SetString("Total", total.ToString());
            List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();
            foreach (CartDTO cart in Cart)
            {
                OrderDetailDTO orderDetailDTO = new OrderDetailDTO();
                orderDetailDTO.FoodId = cart.FoodId;
                orderDetailDTO.Quantity = cart.Count;
                orderDetailDTO.UnitPrice = cart.Food.FoodPrice;
                orderDetailDTOs.Add(orderDetailDTO);
            }
            HttpContext.Session.SetString("OrderDetailList", JsonSerializer.Serialize(orderDetailDTOs));
            HttpContext.Session.SetString("CodePromo", Code);
            return RedirectToPage("/Foods/Checkout");
        }
    }
}
