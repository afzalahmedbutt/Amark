using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    class RequestForOrderItemModel
    {
        public RequestForOrderItemModel()
        {
            Warnings = new List<string>();
        }
        public string Commodity { get; set; }

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
    }
}
