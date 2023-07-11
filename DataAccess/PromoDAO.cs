using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PromoDAO
    {
            public static List<Promo> GetPromos()
            {
                var Promos = new List<Promo>();
                try
                {
                    using (var context = new FoodOrderContext())
                    {
                        Promos = context.Promos.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return Promos;
            }
            public static void SavePromo(Promo Promo)
            {
                try
                {
                    using (var context = new FoodOrderContext())
                    {
                        context.Promos.Add(Promo);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public static void UpdatePromo(Promo Promo)
            {
                try
                {
                    using (var context = new FoodOrderContext())
                    {
                        context.Entry<Promo>(Promo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
