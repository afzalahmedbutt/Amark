using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public partial class RequestForOrderModel 
    {
        public RequestForOrderModel()
        {
            Items = new List<RequestForOrderItemModel>();
            ModifiedItems = new List<RequestForOrderItemModel>();
            Warnings = new List<string>();

        }

        public IList<RequestForOrderItemModel> Items { get; set; }
        public IList<RequestForOrderItemModel> ModifiedItems { get; set; }
        public IList<string> Warnings { get; set; }
        public string MinOrderSubtotalWarning { get; set; }
        public bool IsSellMode { get; set; }
        public bool IsEditable { get; set; }
        public bool IsHFI { get; set; }
        public bool IsDropShip { get; set; }
        public string SpecialInstructions { get; set; }
        public string TPConfirmation { get; set; }
        public int PriceExpireInterval { get; set; }
        public bool IsDiscountPricing { get; set; }
        public string SmallOrderCharge { get; set; }
        public string QuoteKey { get; set; }
        public bool MultipleTransactionGroupsPresent { get; set; }
        public string sTicketNumber { get; set; }
        public bool IsHedgedOrder { get; set; }

        public string Name1 { get; set; }
        
        public string Name2 { get; set; }
        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string Zip { get; set; }
        
        public string Country { get; set; }

        //         //
        //After Review Table fields
        public decimal DecGoldTotal { get; set; }
        public decimal DecGoldTotalQuantity { get; set; }

        public decimal DecSilverTotal { get; set; }
        public decimal DecSilverTotalQuantity { get; set; }

        public decimal DecPlatinumTotal { get; set; }
        public decimal DecPlatinumTotalQuantity { get; set; }

        public decimal DecPalladiumTotal { get; set; }
        public decimal DecPalladiumTotalQuantity { get; set; }

        public decimal DecTotal { get; set; }
        public decimal DecTotalQuantity { get; set; }

        public bool IsTradeConfirmed { get; set; }
        public bool IsSell { get; set; }

        public bool IsRequote { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        #region Nested Classes

        public partial class RequestForOrderItemModel
        {
            public RequestForOrderItemModel()
            {
                Warnings = new List<string>();
            }
            public string Commodity { get; set; }
            public int CommodityID { get; set; }

            public string Sku { get; set; }

            public int ProductId { get; set; }

            public string ProductName { get; set; }
            public string ProductDesc { get; set; }
            public decimal ProductWeight { get; set; }
            public decimal WeightSubTotal { get; set; }

            public decimal SpotPrice { get; set; }

            public bool PremiumIsPercent { get; set; }

            public decimal ProductPremium { get; set; }

            public int MinPurchase { get; set; }

            public decimal PurchaseIncrement { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal Quantity { get; set; }
            public decimal SubTotal { get; set; }

            public string TierPrices { get; set; }

            public IList<string> Warnings { get; set; }
            public bool IsFavorite { get; set; }
            public bool IsNew { get; set; }


        }
        #endregion
    }
}
