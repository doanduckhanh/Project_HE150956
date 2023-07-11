using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class PromoRepository : IPromoRepository
    {
        public List<Promo> GetPromos() => PromoDAO.GetPromos();

        public void SavePromo(Promo promo) => PromoDAO.SavePromo(promo);

        public void UpdatePromo(Promo promo) => PromoDAO.UpdatePromo(promo);
    }
}
