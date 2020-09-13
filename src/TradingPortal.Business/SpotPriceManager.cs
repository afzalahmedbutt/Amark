using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Repositories.Interfaces;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Business
{
    public class SpotPriceManager : ISpotPriceManager
    {
        private readonly ICurrentUser _currentUser;
        private readonly IFeedsRepository _feedsRepository;
        public SpotPriceManager(ICurrentUser currentUser, IFeedsRepository feedsRepository)
        {
            _currentUser = currentUser;
            _feedsRepository = feedsRepository;
        }

        public async Task<SpotPricePreviewViewModel> SpotPricePreview(string VsWhich = "NY")
        {
            DateTime dtNow = DateTime.Now;
            var spotPrices = await _feedsRepository.GetAll()
                .OrderBy(wsp => wsp.ID)
                .Where(wsp => wsp.VsMarket == VsWhich)
                .ToListAsync();
            SpotPricePreviewViewModel spotPricePreviewViewModel = new SpotPricePreviewViewModel();
            spotPricePreviewViewModel.ServerTime = dtNow.ToLongDateString();
            spotPricePreviewViewModel.Spots = spotPrices;
            if (dtNow.Hour >= 17 || dtNow.Day == 6 || dtNow.Day == 0)
            {
                spotPricePreviewViewModel.IsAfterHours = "Yes";
            }
            else
            {
                spotPricePreviewViewModel.IsAfterHours = "No";
            }
            return spotPricePreviewViewModel;
        }

        public async Task<UpdateSpotsViewModel> UpdateSpots(DateTime LastDate, string VsWhich = "NY", string IsAfterHours = "NO")
        {
             SpotPriceAfterHoursStatus status = GetSpotPriceAfterhoursStatus();
            if (status.IsAfterHours == "NO")
            {
                if (status.IsAfterHours != IsAfterHours)
                {
                    var spots = await _feedsRepository.GetAll()
                        .OrderBy(wmn => wmn.ID)
                        .Where(wmn => wmn.VsMarket == VsWhich)
                        .ToListAsync();

                    return
                     new UpdateSpotsViewModel
                     {
                         Spots = spots,
                         IsAfterHours = status.IsAfterHours,
                         IsClosed = status.IsClosed,
                         MarketText = status.MarketText,
                         AmarkText = status.AmarkText
                         //,DeskText = status.AmarkDeskText
                     };

                }
                else
                {
                    var spots = await _feedsRepository.GetAll()
                        .OrderBy(wmn => wmn.ID)
                        .Where((wmn => wmn.VsMarket == VsWhich && (wmn.Update_Date > LastDate)))
                        .ToListAsync();

                    return
                     new UpdateSpotsViewModel
                     {
                         Spots = spots,
                         IsAfterHours = status.IsAfterHours,
                         IsClosed = status.IsClosed,
                         MarketText = status.MarketText,
                         AmarkText = status.AmarkText
                         //,DeskText = status.AmarkDeskText
                     };


                }

            }
            else
            {
                VsWhich = "NY"; //override

                //Spot Data - get NY Close values
                decimal goldclose = await (from sp in _feedsRepository.GetAll()
                                           where sp.VsMarket == VsWhich && sp.ComCode == "G"
                                           select sp.VsClose.Value).FirstAsync();
                decimal silverclose = await (from sp in _feedsRepository.GetAll()
                                             where sp.VsMarket == VsWhich && sp.ComCode == "S"
                                             select sp.VsClose.Value).FirstAsync();
                decimal platclose = await (from sp in _feedsRepository.GetAll()
                                           where sp.VsMarket == VsWhich && sp.ComCode == "P"
                                           select sp.VsClose.Value).FirstAsync();
                decimal pallclose = await (from sp in _feedsRepository.GetAll()
                                           where sp.VsMarket == VsWhich && sp.ComCode == "L"
                                           select sp.VsClose.Value).FirstAsync();
                int maxSpot = await (from sp in _feedsRepository.GetSpotPriceHistory()
                                     orderby sp.ID descending
                                     select sp.ID).FirstAsync();

                var spotsAfterHour = (from c in _feedsRepository.GetSpotPriceHistory()
                                      where c.ID == maxSpot //&& c.SPOTDATE > LastDate
                                      select new
                                      {
                                          ComCode = "G",
                                          Bid = (decimal)(c.BID_GOLD.HasValue ? c.BID_GOLD : 0),
                                          Ask = (decimal)(c.ASK_GOLD.HasValue ? c.ASK_GOLD : 0),
                                          VsClose = (decimal)(goldclose),
                                          UpdateDate = c.SPOTDATE,
                                          ID = 1
                                      }).Union(

                                         (from c in _feedsRepository.GetSpotPriceHistory()
                                          where c.ID == maxSpot //&& c.SPOTDATE > LastDate
                                          select new
                                          {
                                              ComCode = "S",
                                              Bid = (decimal)(c.BID_SILVER.HasValue ? c.BID_SILVER : 0),
                                              Ask = (decimal)(c.ASK_SILVER.HasValue ? c.ASK_SILVER : 0),
                                              VsClose = (decimal)(silverclose),
                                              UpdateDate = c.SPOTDATE,
                                              ID = 2
                                          })
                                      ).Union(

                                                (from c in _feedsRepository.GetSpotPriceHistory()
                                                 where c.ID == maxSpot //&& c.SPOTDATE > LastDate
                                                 select new
                                                 {
                                                     ComCode = "P",
                                                     Bid = (decimal)(c.BID_PLATINUM.HasValue ? c.BID_PLATINUM : 0),
                                                     Ask = (decimal)(c.ASK_PLATINUM.HasValue ? c.ASK_PLATINUM : 0),
                                                     VsClose = (decimal)(platclose),
                                                     UpdateDate = c.SPOTDATE,
                                                     ID = 3
                                                 })
                                       ).Union(

                                                (from c in _feedsRepository.GetSpotPriceHistory()
                                                 where c.ID == maxSpot //&& c.SPOTDATE > LastDate
                                                 select new
                                                 {
                                                     ComCode = "L",
                                                     Bid = (decimal)(c.BID_PALLADIUM.HasValue ? c.BID_PALLADIUM : 0),
                                                     Ask = (decimal)(c.ASK_PALLADIUM.HasValue ? c.ASK_PALLADIUM : 0),
                                                     VsClose = (decimal)(pallclose),
                                                     UpdateDate = c.SPOTDATE,
                                                     ID = 4
                                                 })

                                         ).OrderBy(a => a.ID);

                var spotAfterHoursList = await spotsAfterHour.Select(sah => new WebSpotPrices
                {
                    ComCode = sah.ComCode,
                    Bid = sah.Bid,
                    Ask = sah.Ask,
                    VsClose = sah.VsClose,
                    Update_Date = sah.UpdateDate
                }).ToListAsync();


                return
                   new UpdateSpotsViewModel
                   {
                       Spots = spotAfterHoursList,
                       IsAfterHours = status.IsAfterHours,
                       IsClosed = status.IsClosed,
                       MarketText = status.MarketText,
                       AmarkText = status.AmarkText
                       //,DeskText = status.AmarkDeskText
                   };

            }
        }


        //Spot Price Helper Methods

        public TimeSpan addHolidayCloseTime()
        {
            DateTime today = DateTime.Now.Date;
            DateTime today7 = DateTime.Now.Date.AddDays(7);
            DateTime todayTime = DateTime.Now;
            //
            TimeSpan holidayTime = new TimeSpan(0, 0, 0);
            bool addingHolidays = false;
            int WeekDayNoHoliday = 0;

            //This whole thing is not a good effect, performance wise since its done on every refresh due
            //to needing to update site every x seconds. This was coded in a loop to break out asap on no
            //holiday
            if (!isHoliday(today) && todayTime.Hour < 17 && !((int)today.DayOfWeek == 6 || (int)today.DayOfWeek == 0))  //if we open and its not a holi today - get out
                return holidayTime;

            //check the next week;
            //REMEMBER this is just calcing any ADDITIONAL time to normal close hours.
            DateTime todayLoop = today;
            while (todayLoop <= today7)
            {
                //sat or sun - skip thru
                if ((int)todayLoop.DayOfWeek == 6 || (int)todayLoop.DayOfWeek == 0)
                {
                    WeekDayNoHoliday = 0;
                    if (addingHolidays && todayLoop != today) //if we in middle of adding holiday time then keep adding (ie - 4 day weekend) 
                    {
                        holidayTime += new TimeSpan(24, 0, 0);
                        addingHolidays = true;
                    }
                    todayLoop = todayLoop.AddDays(1);
                    continue;
                }

                //if we did add holiday time, and we back on a weekday thats not a holiday - we are done
                if (!isHoliday(todayLoop) && addingHolidays)
                    break;

                //holiday 
                if (isHoliday(todayLoop))
                {
                    WeekDayNoHoliday = 0;
                    if (todayLoop == today && todayTime.Hour < 17)
                    {
                        //if before normal closing time of 5pm show full span til midnite as after 5pm it will be normal close timespan
                        holidayTime += new TimeSpan(23 - todayTime.Hour, 60 - todayTime.Minute, 0);
                    }
                    else //its just a whole day
                    {
                        holidayTime += new TimeSpan(24, 0, 0);
                    }
                    addingHolidays = true;
                }
                else
                {
                    ++WeekDayNoHoliday;
                    if (WeekDayNoHoliday >= 2)
                        break;
                }

                todayLoop = todayLoop.AddDays(1);
            }

            return holidayTime;
        }

        public bool isHoliday(DateTime dt)
        {
            //check date for holiday
            var holidays = from hol in _feedsRepository.GetHolidays()
                           where hol.DateOf == dt
                           select new
                           {
                               dt = hol.DateOf
                           };

            return (holidays.Count() > 0 ? true : false);
        }

        public TimeSpan SpotMarketOpens()
        {
            DateTime current = DateTime.Now;
            DateTime openmarket;
            TimeSpan openwait = new TimeSpan(0, 0, 0);
            TimeSpan blank = new TimeSpan(0, 0, 0);
            TimeSpan tsSundayOpen = new TimeSpan(15, 0, 0);

            //Friday after 5
            if (current.Hour >= 17 && (int)current.DayOfWeek == 5) //Friday after 5p
            {
                openmarket = current.AddDays(2).Date + tsSundayOpen;
                openwait = openmarket - current;
            }
            else if ((int)current.DayOfWeek == 6 || (int)current.DayOfWeek == 0)   // Saturday, or Sunday
            {
                int diffdays = ((int)current.DayOfWeek == 6 ? 1 : 0);
                openmarket = current.AddDays(diffdays).Date + tsSundayOpen;
                openwait = openmarket - current;
            }
            if ((openwait.Days * 24) + openwait.Hours + openwait.Minutes <= 0)
                openwait = blank;


            return openwait;
        }

        public TimeSpan AmarkOpens()
        {
            DateTime current = DateTime.Now;
            DateTime openamark;
            TimeSpan openwait = new TimeSpan(0, 0, 0);
            TimeSpan blank = new TimeSpan(0, 0, 0);
            TimeSpan tsMidnight = new TimeSpan(0, 0, 0);
            TimeSpan addHoliday = new TimeSpan(0, 0, 0);

            addHoliday = addHolidayCloseTime();

            //Friday after 5   
            if (current.Hour >= 17 && (int)current.DayOfWeek == 5) //Friday after 5p
            {
                openamark = current.AddDays(3).Date + tsMidnight;
                openwait = openamark - current;
            }
            else if ((int)current.DayOfWeek == 6 || (int)current.DayOfWeek == 0)   // Saturday, or Sunday
            {
                int diffdays = ((int)current.DayOfWeek == 6 ? 2 : 1);
                openamark = current.AddDays(diffdays).Date + tsMidnight;
                openwait = openamark - current;
            }
            else if // (current.Hour >= 4 && current.Minute>=40)  //
                (current.Hour >= 17) //after 5p Mon-Thurs
            {
                openamark = current.AddDays(1).Date + tsMidnight;
                openwait = openamark - current;
            }
            if ((openwait.Days * 24) + openwait.Hours + openwait.Minutes <= 0)
                openwait = blank;

            openwait += addHoliday;

            return openwait; // +addHoliday;
        }

        public bool IsAfterHours()
        {
            bool closed = false;
            DateTime current = DateTime.Now;

            if (
                  (current.Hour >= 17) ||  // after 5pm
                  ((int)current.DayOfWeek == 6) ||  // Saturday
                  ((int)current.DayOfWeek == 0)     // Sunday
                )
            {
                closed = true;
            }

            //holiday patch test
            TimeSpan OpenAmark;
            OpenAmark = AmarkOpens();
            // after hours time delay until opening
            if (((OpenAmark.Days * 24) + OpenAmark.Hours + OpenAmark.Minutes) != 0)
                closed = true;


            return closed;
        }

        public SpotPriceAfterHoursStatus GetSpotPriceAfterhoursStatus()
        {
            SpotPriceAfterHoursStatus status = new SpotPriceAfterHoursStatus();
            bool bAfterHours = false;
            TimeSpan OpenAmark;
            TimeSpan OpenMarket;
            status.IsClosed = "NO";

            //check after hours
            bAfterHours = IsAfterHours();
            OpenAmark = AmarkOpens();
            OpenMarket = SpotMarketOpens();

            // after hours time delays until opening
            var CountdownAmark = new
            {
                Hours = (OpenAmark.Days * 24) + OpenAmark.Hours,
                Minutes = OpenAmark.Minutes,
            };
            var CountdownMarket = new
            {
                Hours = (OpenMarket.Days * 24) + OpenMarket.Hours,
                Minutes = OpenMarket.Minutes,
            };

            //after hour message
            if (CountdownMarket.Hours + CountdownMarket.Minutes != 0)
            {
                status.MarketText = "SPOT MARKET CLOSED";
                status.AmarkText = "Trading Desk Closed | Opens in " + (CountdownAmark.Hours != 0 ? "<span>" + CountdownAmark.Hours + "</span> Hrs " : "") + (CountdownAmark.Minutes != 0 ? "<span>" + CountdownAmark.Minutes + "</span> Mins " : "");
                //this.AmarkDeskText = "Trading Desk Closed";
                status.IsClosed = "YES";
            }
            else if (CountdownAmark.Hours + CountdownAmark.Minutes != 0)
            {
                status.MarketText = (CountdownMarket.Hours + CountdownMarket.Minutes != 0 ? "SPOT MARKET CLOSED" : "SPOT MARKET OPEN<span style=\"color:#cccccc;\"> (indications only)</span>");
                status.AmarkText = "Trading Desk Closed | Opens in " + (CountdownAmark.Hours != 0 ? "<span>" + CountdownAmark.Hours + " </span> Hrs " : "") + (CountdownAmark.Minutes != 0 ? "<span>" + CountdownAmark.Minutes + " </span> Mins " : "");
                //this.AmarkDeskText = "Trading Desk Closed";
            }
            else
            {
                //this.MarketText = "SPOT MARKET OPEN (indications only)";
                //this.AmarkText = "A-Mark Trading Desk Opens in " + (CountdownAmark.Hours != 0 ? CountdownAmark.Hours + " Hrs " : "") + (CountdownAmark.Minutes != 0 ? CountdownAmark.Minutes + " Mins " : "");
                status.MarketText = "";
                status.AmarkText = "";
                //this.AmarkDeskText = "";
            }
            //afterhours
            status.IsAfterHours = (bAfterHours) ? "YES" : "NO";
            return status;
        }
    }
}

