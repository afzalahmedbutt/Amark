using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.Domain
{
    public class SpotPriceDaily
    {
        [Key]
        public int ID { get; set; }
        public DateTime SPOTDATE { get; set; }
        public decimal? OPEN_GOLD { get; set; }
        public decimal? HIGH_GOLD { get; set; }
        public decimal? LOW_GOLD { get; set; }
        public decimal? CLOSE_GOLD { get; set; }
        public decimal? OPEN_SILVER { get; set; }
        public decimal? HIGH_SILVER { get; set; }
        public decimal? LOW_SILVER { get; set; }
        public decimal? CLOSE_SILVER { get; set; }
        public decimal? OPEN_PLATINUM { get; set; }
        public decimal? HIGH_PLATINUM { get; set; }
        public decimal? LOW_PLATINUM { get; set; }
        public decimal? CLOSE_PLATINUM { get; set; }
        public decimal? OPEN_PALLADIUM { get; set; }
        public decimal? HIGH_PALLADIUM { get; set; }
        public decimal? LOW_PALLADIUM { get; set; }
        public decimal? CLOSE_PALLADIUM { get; set; }
    }
}
