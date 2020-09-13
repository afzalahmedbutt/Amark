using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Amark;

namespace TradingPortal.Infrastructure.Repositories.Interfaces
{
    public interface IAmarkProductRepository : IAMarkRepository<WholesalePrices>
    {
        Task<List<WholesalePrices>> GetWholesalePrices();
        Task<List<Brochure_Products>> GetBrochureProductsByComCode(string comCode);
    }
}
