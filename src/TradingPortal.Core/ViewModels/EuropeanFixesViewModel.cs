using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class EuropeanFixesViewModel
    {
        public string Bid { get; set; }
        public string Ask { get; set; }
        public DateTime Update_Date { get; set; }
        //public decimal? CLOSE_GOLD { get; set; }
        //public decimal? Bid_Open { get; set; }
        //public decimal? Bid_High { get; set; }
        //public decimal? Bid_Low { get; set; }
    }

    public class SpotHistoryViewModel
    {
        public decimal? Bid { get; set; }
        public decimal? Ask { get; set; }
        public DateTime Update_Date { get; set; }
        public decimal? CLOSE_GOLD { get; set; }
        public decimal? Bid_Open { get; set; }
        public decimal? Bid_Close { get; set; }
        public decimal? Bid_High { get; set; }
        public decimal? Bid_Low { get; set; }
    }
}
