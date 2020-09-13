using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public decimal Weight { get; set; }
        public string Sku { get; set; }
        public int CommodityID { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
