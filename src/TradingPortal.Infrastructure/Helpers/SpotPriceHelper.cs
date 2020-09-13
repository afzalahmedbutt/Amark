using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingPortal.Infrastructure.Repositories.Interfaces;

namespace TradingPortal.Infrastructure.Helpers
{
    public class SpotPriceHelper
    {
        private readonly IFeedsRepository _spotPriceRepository;
        public SpotPriceHelper(IFeedsRepository spotPriceRepository)
        {
            _spotPriceRepository = spotPriceRepository;
        }

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
            var holidays = from hol in _spotPriceRepository.GetHolidays()
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
    }
}
