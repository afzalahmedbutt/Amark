using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class CustomerAttributes
    {
        public string AmarkTPAPIKey { get; set; }
        public string AmarkTradingPartnerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string UseRewardPointsDuringCheckout { get; set; }
    }
}
