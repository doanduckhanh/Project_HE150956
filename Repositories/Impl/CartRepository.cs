using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class CartRepository : ICartRepository
    {
        public void AddToCart(int foodId, string OrderCartId) => CartDAO.AddToCart(foodId, OrderCartId);

        public List<Cart> GetCart(string CartId) => CartDAO.GetCart(CartId);

        public int GetCount(string CartId) => CartDAO.GetCount(CartId);

        public decimal GetTotal(string CartId, string Code) => CartDAO.GetTotal(CartId, Code);

        public void IncreaseFromCart(string CartId, int recordId) => CartDAO.IncreaseFromCart(CartId, recordId);

        public void RemoveCart(string CartId, int recordId) => CartDAO.RemoveCart(CartId, recordId);

        public void RemoveFromCart(string CartId, int recordId) => CartDAO.RemoveFromCart(CartId, recordId);
    }
}
