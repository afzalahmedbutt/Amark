using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.Domain
{
    public class SpotPriceHistory : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public DateTime SPOTDATE { get; set; }
        public decimal? BID_GOLD { get; set; }
        public decimal? ASK_GOLD { get; set; }
        public decimal? BID_SILVER { get; set; }
        public decimal? ASK_SILVER { get; set; }
        public decimal? BID_PLATINUM { get; set; }
        public decimal? ASK_PLATINUM { get; set; }
        public decimal? BID_PALLADIUM { get; set; }
        public decimal? ASK_PALLADIUM { get; set; }
    }
}
