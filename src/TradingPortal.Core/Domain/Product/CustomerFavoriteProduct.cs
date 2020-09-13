using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.Domain
{
    public class CustomerFavoriteProduct : BaseEntity
    {
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public bool IsFavorite { get; set; }
    }
}
