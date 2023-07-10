using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        public static void SaveOrderDetail(OrderDetail orderdetail)
        {
            try
            {
                using (var context = new FoodOrderContext())
                {
                    context.OrderDetails.Add(orderdetail);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
