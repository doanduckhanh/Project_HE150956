using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CartDAO
    {       
        public static void AddToCart(int foodId, String OrderCartId)
        {
            FoodOrderContext context = new FoodOrderContext();
            // Get the matching cart and album instances
            var cartItem = context.Carts.SingleOrDefault(
                c => c.CartId == OrderCartId
                && c.FoodId == foodId);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    FoodId = foodId,
                    CartId = OrderCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                context.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            context.SaveChanges();
        }

        public static decimal GetTotal(string CartId, string Code)
        {
            FoodOrderContext context = new FoodOrderContext();
            // Multiply album price by count of that album to get
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in context.Carts
                              where cartItems.CartId == CartId
                              select cartItems.Count * cartItems.Food.FoodPrice).Sum();

            var promo = context.Promos.SingleOrDefault(
                c => c.PromoCode.Equals(Code) && c.PromoStatus == true);

            if (promo != null)
            {
                if (promo.PromoValue.Contains('%'))
                {
                    string numStr = new string(promo.PromoValue.Where(c => Char.IsDigit(c)).ToArray());
                    int num = int.Parse(numStr);
                    var value = ((decimal)num / 100) * total;
                    total -= value;
                }
                if (Regex.IsMatch(promo.PromoValue, @"^\d+$"))
                {
                    total -= int.Parse(promo.PromoValue);
                }
                //HttpContext.Session.SetString("CodePromo", Code.ToString());
            }
            else
            {
                //HttpContext.Session.Remove("CodePromo");
            }

            return total ?? 0;
        }
        public static int GetCount(string CartId)
        {
            FoodOrderContext context = new FoodOrderContext();
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in context.Carts
                          where cartItems.CartId == CartId
                          select (int?)cartItems.Count).Count();
            // Return 0 if all entries are null
            return count ?? 0;

        }
        public static void RemoveCart(string CartId, int recordId)
        {
            FoodOrderContext context = new FoodOrderContext();
            var cartItem = context.Carts.SingleOrDefault(
                c => c.CartId.Equals(CartId)
                && c.RecordId == recordId);
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                }
                else
                {
                    context.Carts.Remove(cartItem);
                }
                // Save changes
                context.SaveChangesAsync();

            }
        }
        public static List<Cart> GetCart(string CartId)
        {
            FoodOrderContext context = new FoodOrderContext();
            List<Cart> carts = context.Carts
                .Where(x => x.CartId.Equals(CartId))
                .Include(a => a.Food)
                .Include(a => a.Food.Category).ToList();
            return carts;
        }
        public static void IncreaseFromCart(string CartId,int recordId)
        {
            FoodOrderContext context = new FoodOrderContext();
            var cartItem = context.Carts.SingleOrDefault(
                c => c.CartId == CartId
                && c.RecordId == recordId);

            if (cartItem != null)
            {
                cartItem.Count++;
                // Save changes
                context.SaveChangesAsync();

            }
        }
        public static void RemoveFromCart(string CartId, int recordId)
        {
            FoodOrderContext context = new FoodOrderContext();
            var cartItem = context.Carts.SingleOrDefault(
                c => c.CartId.Equals(CartId)
                && c.RecordId == recordId);
            context.Carts.Remove(cartItem);
                // Save changes
            context.SaveChangesAsync();
        }
    }
}
