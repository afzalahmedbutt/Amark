using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Core.ViewModels
{
    public class UpdateSpotsViewModel
    {
        public List<WebSpotPrices> Spots { get; set; }
        public string IsAfterHours { get; set; }
        public string IsClosed { get; set; }
        public string MarketText { get; set; }
        public string AmarkText { get; set; }
    
    }
}
