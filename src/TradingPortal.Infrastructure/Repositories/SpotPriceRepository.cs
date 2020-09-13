using System.Linq;
using TradingPortal.Core.Domain;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Repositories.Interfaces;

namespace TradingPortal.Infrastructure.Repositories
{
    public class FeedsRepository : EFAMarkRespository<WebSpotPrices>,IFeedsRepository
    {
        private readonly AMarkDbContext dbContext;
        public FeedsRepository(AMarkDbContext context) : base(context)
        {
            dbContext = context;
        }

        public IQueryable<Holiday> GetHolidays()
        {
            return dbContext.Holidays.AsQueryable();
        }

        public IQueryable<WebSpotPrices> GetWebSpotPrices()
        {
            return GetAll();
        }

        public IQueryable<SpotPriceHistory> GetSpotPriceHistory()
        {
            return dbContext.SpotPriceHistory.AsQueryable();
        }

        public IQueryable<European_Fixes> GetEuropeanFixes()
        {
            return dbContext.European_Fixes.AsQueryable();
        }

        public IQueryable<SpotPriceDaily> GetSpotPriceDaily()
        {
            return dbContext.SpotPriceDaily.AsQueryable();
        }
    }
}
