using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using System.Linq;

namespace TradingPortal.Infrastructure.Repositories.Interfaces
{
    public interface IFeedsRepository : IAMarkRepository<WebSpotPrices>
    {
        IQueryable<WebSpotPrices> GetWebSpotPrices();
        IQueryable<Holiday> GetHolidays();
        IQueryable<SpotPriceHistory> GetSpotPriceHistory();
        IQueryable<European_Fixes> GetEuropeanFixes();
        IQueryable<SpotPriceDaily> GetSpotPriceDaily();

    }
}
