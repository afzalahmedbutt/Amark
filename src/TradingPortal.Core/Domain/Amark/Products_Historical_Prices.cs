using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingPortal.Core.Domain.Amark
{
    public class Products_Historical_Prices : BaseEntity
    {
        [Key]
        [Column]
        public int Product_Historical_Price_Id { get; set; }
        public int Product_Id { get; set; }
        public DateTime Close_Date { get; set; }
        public double? Ask { get; set; }

        public virtual Product Product { get; set; }
    }
}
