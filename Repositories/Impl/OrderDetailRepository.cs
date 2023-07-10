using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void SaveOrderDetails(OrderDetail orderDetail) => OrderDetailDAO.SaveOrderDetail(orderDetail);
    }
}
