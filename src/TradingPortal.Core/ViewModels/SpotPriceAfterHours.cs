using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class SpotPriceAfterHoursPrices
    {
        public int ID { get; set; }
        public string ComCode { get; set; }
        public decimal? Bid { get; set; }
        public decimal? Ask { get; set; }
        public decimal? VsClose { get; set; }
        public DateTime Update_Date { get; set; }

    }

    public class SpotPriceAfterHoursStatus
    {
        public string IsAfterHours;
        public string MarketText;
        public string AmarkText;
        //public string AmarkDeskText;
        public string IsClosed;
    }

    //    public class SpotPriceAfterHoursStatus1
    //{
    //    public string IsAfterHours;
    //    public string MarketText;
    //    public string AmarkText;
    //    //public string AmarkDeskText;
    //    public string IsClosed;
    //    //private FeedsController feed = new FeedsController();

    //    public SpotPriceAfterHoursStatus1()
    //    {
    //        bool bAfterHours = false;
    //        TimeSpan OpenAmark;
    //        TimeSpan OpenMarket;
    //        this.IsClosed = "NO";

    //        //Dummy Data
    //        OpenAmark = new TimeSpan();
    //        OpenMarket = new TimeSpan();

    //        //check after hours
    //        //bAfterHours = feed.IsAfterHours();
    //        //OpenAmark = feed.AmarkOpens();
    //        //OpenMarket = feed.SpotMarketOpens();

    //        // after hours time delays until opening
    //        var CountdownAmark = new
    //        {
    //            Hours = (OpenAmark.Days * 24) + OpenAmark.Hours,
    //            Minutes = OpenAmark.Minutes,
    //        };
    //        var CountdownMarket = new
    //        {
    //            Hours = (OpenMarket.Days * 24) + OpenMarket.Hours,
    //            Minutes = OpenMarket.Minutes,
    //        };

    //        //after hour message
    //        if (CountdownMarket.Hours + CountdownMarket.Minutes != 0)
    //        {
    //            this.MarketText = "SPOT MARKET CLOSED";
    //            this.AmarkText = "Trading Desk Closed | Opens in " + (CountdownAmark.Hours != 0 ? "<span>" + CountdownAmark.Hours + "</span> Hrs " : "") + (CountdownAmark.Minutes != 0 ? "<span>" + CountdownAmark.Minutes + "</span> Mins " : "");
    //            //this.AmarkDeskText = "Trading Desk Closed";
    //            this.IsClosed = "YES";
    //        }
    //        else if (CountdownAmark.Hours + CountdownAmark.Minutes != 0)
    //        {
    //            this.MarketText = (CountdownMarket.Hours + CountdownMarket.Minutes != 0 ? "SPOT MARKET CLOSED" : "SPOT MARKET OPEN<span style=\"color:#cccccc;\"> (indications only)</span>");
    //            this.AmarkText = "Trading Desk Closed | Opens in " + (CountdownAmark.Hours != 0 ? "<span>" + CountdownAmark.Hours + " </span> Hrs " : "") + (CountdownAmark.Minutes != 0 ? "<span>" + CountdownAmark.Minutes + " </span> Mins " : "");
    //            //this.AmarkDeskText = "Trading Desk Closed";
    //        }
    //        else
    //        {
    //            //this.MarketText = "SPOT MARKET OPEN (indications only)";
    //            //this.AmarkText = "A-Mark Trading Desk Opens in " + (CountdownAmark.Hours != 0 ? CountdownAmark.Hours + " Hrs " : "") + (CountdownAmark.Minutes != 0 ? CountdownAmark.Minutes + " Mins " : "");
    //            this.MarketText = "";
    //            this.AmarkText = "";
    //            //this.AmarkDeskText = "";
    //        }
    //        //afterhours
    //        this.IsAfterHours = (bAfterHours) ? "YES" : "NO";

    //    }

    //}

    public class SpotPriceAfterHoursData
    {
        public virtual List<SpotPriceAfterHoursPrices> Prices { get; set; }
        public SpotPriceAfterHoursStatus Status { get; set; }
    }
}
