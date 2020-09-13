using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class OpenModel
    {
                
        public Int32 iOrder_Hdr_ID { get; set; }
                
        public Int32 iTrade_ID { get; set; }
                
        public string sdtTrade { get; set; }
                
        public string sdtValue { get; set; }
                
        public string sTicketNo { get; set; }
                
        public string sTPConfirmNo { get; set; }
                
        public string sTradeType { get; set; }
                
        public string sProductDescription { get; set; }
                
        public string iQuantity { get; set; }
                
        public decimal decProductBalance { get; set; }
                
        public decimal decCashBalance { get; set; }
                
        public List<string> sTrackingNumbers { get; set; }
                
        public string sReceived { get; set; }
               
        public string sShipping { get; set; }
                
        public decimal decPrice { get; set; }
                
        public decimal decTotalAmount { get; set; }

        public string iTotalRecords { get; set; }

        public string sCurrencySymbol { get; set; }
        
    }

    public class GridOpenModel
    {
        public List<OpenModel> OpenModels { get; set; }
        public int Total { get; set; }
    }

   
}
