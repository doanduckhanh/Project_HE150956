using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class OrderRepository : IOrderRepository
    {
        public void SaveOrder(Order order) => OrderDAO.SaveOrder(order);
    }
}
