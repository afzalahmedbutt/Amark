using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Repositories.Interfaces;

namespace TradingPortal.Infrastructure.Repositories
{
    public class AmarkProductRepository : EFAMarkRespository<WholesalePrices>, IAmarkProductRepository
    {
        private readonly AMarkDbContext dbContext;
        public AmarkProductRepository(AMarkDbContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<List<WholesalePrices>> GetWholesalePrices()
        {
            try
            {
                var wholeSalePrices = await GetAll().Include(wsp => wsp.Products).ToListAsync();
                return wholeSalePrices.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Brochure_Products>> GetBrochureProductsByComCode(string comCode)
        {
            try
            {
                var products = await dbContext.Brochure_Products.Include(bp => bp.Brochure_Product_Variants)
                    .Where(p => p.Is_Active == "Y" && p.Section == comCode)
                     .ToListAsync();
                return products;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



    }
}
