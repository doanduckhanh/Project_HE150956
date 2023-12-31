﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? PromoCode { get; set; }
        public string? UserName { get; set; }
        public decimal? Total { get; set; }
        public int? UserId { get; set; }

        public virtual Promo? PromoCodeNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
