using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class ShippingInfo
    {
        public int iRequestID { get; set; }
        public int iOrderHdrID { get; set; }
        public string sShippingName1 { get; set; }
        public string sShippingName2 { get; set; }
        public string sShippingAddress1 { get; set; }
        public string sShippingAddress2 { get; set; }
        public string sShippingCity { get; set; }
        public string sShippingState { get; set; }
        public string sShippingZipCode { get; set; }
        public string sShippingCountry { get; set; }
        public string sShippingPhoneNumber { get; set; }
        public string rqs { get; set; }
    }

    public class UpdateShippingInfoResult
    {
        public bool IsValid { get; set; }
        public string QValue { get; set; }
    }
}
