using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICartRepository
    {
        void AddToCart(int foodId, String OrderCartId);
        decimal GetTotal(string CartId, string Code);
        int GetCount(string CartId);
        void RemoveCart(string CartId, int recordId);
        List<Cart> GetCart(string CartId);
        void IncreaseFromCart(string CartId, int recordId);
        void RemoveFromCart(string CartId, int recordId);
    }
}
