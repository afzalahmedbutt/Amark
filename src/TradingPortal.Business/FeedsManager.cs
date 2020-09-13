using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Repositories;
using TradingPortal.Infrastructure.Repositories.Interfaces;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Business
{
    public class FeedsManager : IFeedsManager
    {
        private readonly ICurrentUser _currentUser;
        private readonly IFeedsRepository _feedsRepository;


        public FeedsManager(
            ICurrentUser currentUser,
            IFeedsRepository feedsRepository
            )
        {
            _currentUser = currentUser;
            _feedsRepository = feedsRepository;
        }

        public async Task<List<EuropeanFixesViewModel>> FixHistory(DateTime startDate, DateTime endDate, string comCode = "G", string groupBy = "day")
        {
            // add one day to endDate so as to include endDate in the results
            endDate = endDate.AddDays(1);

            switch (comCode)
            {
                case "G":
                    var goldMinFeed = await _feedsRepository.GetEuropeanFixes()
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Gold_AM) == 1)
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Gold_PM) == 1)
                    .Where(s => s.Date >= startDate)
                    .Where(s => s.Date <= endDate)
                    .OrderBy(s => s.Date)
                    .Select(s => new EuropeanFixesViewModel { Bid = s.USD_Gold_AM, Ask = s.USD_Gold_PM, Update_Date = s.Date, })
                    .ToListAsync();

                    return goldMinFeed;

                case "S":
                    var silverMinFeed = await _feedsRepository.GetEuropeanFixes()
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Silver) == 1)
                    .Where(s => s.Date >= startDate)
                    .Where(s => s.Date <= endDate)
                    .OrderBy(s => s.Date)
                    .Select(s => new EuropeanFixesViewModel { Bid = s.USD_Silver, Ask = s.USD_Silver, Update_Date = s.Date })
                    .ToListAsync();

                    return silverMinFeed;

                case "P":
                    var platinumMinFeed = await _feedsRepository.GetEuropeanFixes()
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Platinum_AM) == 1)
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Platinum_PM) == 1)
                    .Where(s => s.Date >= startDate)
                    .Where(s => s.Date <= endDate)
                    .OrderBy(s => s.Date)
                    .Select(s => new EuropeanFixesViewModel { Bid = s.USD_Platinum_AM, Ask = s.USD_Platinum_PM, Update_Date = s.Date })
                    .ToListAsync();

                    return platinumMinFeed;

                case "L":
                    var palladiumMinFeed = await _feedsRepository.GetEuropeanFixes()
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Palladium_AM) == 1)
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Palladium_PM) == 1)
                    .Where(s => s.Date >= startDate)
                    .Where(s => s.Date <= endDate)
                    .OrderBy(s => s.Date)
                    .Select(s => new EuropeanFixesViewModel { Bid = s.USD_Palladium_AM, Ask = s.USD_Palladium_PM, Update_Date = s.Date })
                    .ToListAsync();
                    return palladiumMinFeed;

                default:
                    var defaultMinFeed = await _feedsRepository.GetEuropeanFixes()
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Gold_AM) == 1)
                    .Where(s => AMarkDbContext.ufnCheckIsNumeric(s.USD_Gold_PM) == 1)
                    .Where(s => s.Date >= startDate)
                    .Where(s => s.Date <= endDate)
                    .OrderBy(s => s.Date)
                    .Select(s => new EuropeanFixesViewModel { Bid = s.USD_Gold_AM, Ask = s.USD_Gold_PM, Update_Date = s.Date })
                    .ToListAsync();

                    return defaultMinFeed;
            }
        }

        public async Task<List<SpotHistoryViewModel>>  SpotHistory(DateTime startDate, DateTime endDate, string comCode = "G", string groupBy = "day")
        {
            // add one day to endDate so as to include endDate in the results
            endDate = endDate.AddDays(1);

            TimeSpan dateSpan = endDate.Subtract(startDate);
            // DateTime firstSunday = new DateTime(1753, 1, 6);
            // DateTime firstSaturday = new DateTime(1753, 1, 5);
            DateTime firstSaturday = new DateTime(1753, 1, 6);
            DateTime firstSunday = new DateTime(1753, 1, 7);

            if (groupBy == "min" && dateSpan.Days < 10)
            {
                //var minFeed = db.Spot_Prices
                //    .Where(s => s.ComCode == comCode)
                //    .Where(s => s.Bid != null)
                //    .Where(s => s.Update_Date >= startDate)
                //    .Where(s => s.Update_Date <= endDate)
                //    .OrderBy(s => s.Update_Date)
                //    .Select(s => new { s.Bid, s.Update_Date });

                switch (comCode)
                {
                    case "G":
                        var goldMinFeed = await _feedsRepository.GetSpotPriceHistory()
                        .Where(s => s.BID_GOLD != null)
                        .Where(s => s.SPOTDATE >= startDate)
                        .Where(s => s.SPOTDATE <= endDate)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)  //%7 != 1
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                        .OrderBy(s => s.SPOTDATE)
                        .Select(s => new SpotHistoryViewModel{ Bid = s.BID_GOLD, Ask = s.ASK_GOLD, Update_Date = s.SPOTDATE, })
                        .ToListAsync();

                        return goldMinFeed;

                    case "S":
                        var silverMinFeed = await _feedsRepository.GetSpotPriceHistory()
                        .Where(s => s.BID_SILVER != null)
                        .Where(s => s.SPOTDATE >= startDate)
                        .Where(s => s.SPOTDATE <= endDate)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                        .OrderBy(s => s.SPOTDATE)
                        .Select(s => new SpotHistoryViewModel { Bid = s.BID_SILVER, Ask = s.ASK_SILVER, Update_Date = s.SPOTDATE })
                        .ToListAsync();

                        return silverMinFeed;

                    case "P":
                        var platinumMinFeed = await _feedsRepository.GetSpotPriceHistory()
                        .Where(s => s.BID_PLATINUM != null)
                        .Where(s => s.SPOTDATE >= startDate)
                        .Where(s => s.SPOTDATE <= endDate)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                        .OrderBy(s => s.SPOTDATE)
                        .Select(s => new SpotHistoryViewModel { Bid = s.BID_PLATINUM, Ask = s.ASK_PLATINUM, Update_Date = s.SPOTDATE })
                        .ToListAsync();

                        return platinumMinFeed;

                    case "L":
                        var palladiumMinFeed = await _feedsRepository.GetSpotPriceHistory()
                        .Where(s => s.BID_PALLADIUM != null)
                        .Where(s => s.SPOTDATE >= startDate)
                        .Where(s => s.SPOTDATE <= endDate)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                        .OrderBy(s => s.SPOTDATE)
                        .Select(s => new SpotHistoryViewModel { Bid = s.BID_PALLADIUM, Ask = s.ASK_PALLADIUM, Update_Date = s.SPOTDATE })
                        .ToListAsync();

                        return palladiumMinFeed;

                    default:
                        var defaultMinFeed = await _feedsRepository.GetSpotPriceHistory()
                        .Where(s => s.BID_GOLD != null)
                        .Where(s => s.SPOTDATE >= startDate)
                        .Where(s => s.SPOTDATE <= endDate)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                        .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                        .OrderBy(s => s.SPOTDATE)
                        .Select(s => new SpotHistoryViewModel { Bid = s.BID_GOLD, Ask = s.ASK_GOLD, Update_Date = s.SPOTDATE })
                        .ToListAsync();

                        return defaultMinFeed;
                }


            }
            else
            {
                //var dayFeed = from s in db.SpotPriceDaily
                //              where s.ComCode == comCode
                //              && s.Bid_Close != null
                //              && s.Update_Date >= startDate
                //              && s.Update_Date <= endDate
                //              group s by EntityFunctions.TruncateTime(s.Update_Date) into g
                //              orderby g.Key
                //              select new { Update_Date = g.Key, Bid_Close = g.Max(x => x.Bid_Close) };

                switch (comCode)
                {
                    case "G":
                        var goldDayFeed = await _feedsRepository.GetSpotPriceDaily()
                                .Where(s => s.CLOSE_GOLD != null)
                                .Where(s => s.SPOTDATE >= startDate)
                                .Where(s => s.SPOTDATE <= endDate)
                                //.Where(s => !(s.SPOTDATE.DayOfWeek == DayOfWeek.Saturday || s.SPOTDATE.DayOfWeek == DayOfWeek.Sunday))
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                                .OrderBy(s => s.SPOTDATE)
                                .Select(s => new SpotHistoryViewModel { Update_Date = s.SPOTDATE, Bid_Close = s.CLOSE_GOLD, Bid_Open = s.OPEN_GOLD, Bid_High = s.HIGH_GOLD, Bid_Low = s.LOW_GOLD })
                                .ToListAsync();

                        return goldDayFeed;

                    case "S":
                        var silverDayFeed = await _feedsRepository.GetSpotPriceDaily()
                                .Where(s => s.CLOSE_SILVER != null)
                                .Where(s => s.SPOTDATE >= startDate)
                                .Where(s => s.SPOTDATE <= endDate)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                                .OrderBy(s => s.SPOTDATE)
                                .Select(s => new SpotHistoryViewModel { Update_Date = s.SPOTDATE, Bid_Close = s.CLOSE_SILVER, Bid_Open = s.OPEN_SILVER, Bid_High = s.HIGH_SILVER, Bid_Low = s.LOW_SILVER })
                                .ToListAsync();

                        return silverDayFeed;

                    case "P":
                        var platinumDayFeed = await _feedsRepository.GetSpotPriceDaily()
                                .Where(s => s.CLOSE_PLATINUM != null)
                                .Where(s => s.SPOTDATE >= startDate)
                                .Where(s => s.SPOTDATE <= endDate)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                                .OrderBy(s => s.SPOTDATE)
                                .Select(s => new SpotHistoryViewModel { Update_Date = s.SPOTDATE, Bid_Close = s.CLOSE_PLATINUM, Bid_Open = s.OPEN_PLATINUM, Bid_High = s.HIGH_PLATINUM, Bid_Low = s.LOW_PLATINUM })
                                .ToListAsync();

                        return platinumDayFeed;

                    case "L":
                        var palladiumDayFeed = await _feedsRepository.GetSpotPriceDaily()
                                .Where(s => s.CLOSE_PALLADIUM != null)
                                .Where(s => s.SPOTDATE >= startDate)
                                .Where(s => s.SPOTDATE <= endDate)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                                .OrderBy(s => s.SPOTDATE)
                                .Select(s => new SpotHistoryViewModel { Update_Date = s.SPOTDATE, Bid_Close = s.CLOSE_PALLADIUM, Bid_Open = s.OPEN_PALLADIUM, Bid_High = s.HIGH_PALLADIUM, Bid_Low = s.LOW_PALLADIUM })
                                .ToListAsync();

                        return palladiumDayFeed;

                    default:
                        var defaultDayFeed = await _feedsRepository.GetSpotPriceDaily()
                                .Where(s => s.CLOSE_GOLD != null)
                                .Where(s => s.SPOTDATE >= startDate)
                                .Where(s => s.SPOTDATE <= endDate)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSunday, s.SPOTDATE) % 7 != 0)
                                .Where(s => AMarkDbContext.ufnDiffDays(firstSaturday, s.SPOTDATE) % 7 != 0)
                                .OrderBy(s => s.SPOTDATE)
                                .Select(s => new SpotHistoryViewModel { Update_Date = s.SPOTDATE, Bid_Close = s.CLOSE_GOLD, Bid_Open = s.OPEN_GOLD, Bid_High = s.HIGH_GOLD, Bid_Low = s.LOW_GOLD })
                                .ToListAsync();

                        return defaultDayFeed;

                }
            }
        }

    }
}
