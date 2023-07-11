using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Promo
    {
        public Promo()
        {
            Orders = new HashSet<Order>();
        }

        public string PromoCode { get; set; } = null!;
        public string? PromoValue { get; set; }
        public string? PromoDescribe { get; set; }
        public bool? PromoStatus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
