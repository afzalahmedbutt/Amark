using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Common;

namespace TradingPortal.Core.ViewModels
{
    public class SpotPricePreviewViewModel
    {
        public SpotPricePreviewViewModel()
        {
            VsWhich = "NY";
        }
        public List<WebSpotPrices> Spots { get; set; }
        public string IsAfterHours { get; set; }
        public string VsWhich { get; set; }
        public string ServerTime { get; set; }
    }

    public class AppInitData 
    {
        public UpdateSpotsViewModel UpdateSpotsViewModel { get; set; }
        public TopicViewModel ServiceContract { get; set; }
        public DateTime ServerTime { get; set; }
        public int MaxProductQuantity { get; set; }
        public ContactFormData ContactFormData { get; set; }
        public bool IsPortalClosed { get; set; }


    }
}
