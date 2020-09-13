using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingPortal.Core.Domain
{
    [Table("WebWholesaleMetals")]
    public class WholesalePrices : BaseEntity
    {
        public int ID { get; set; }
        public string ComCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal WholesaleBidPrice { get; set; }
        public decimal WholesaleAskPrice { get; set; }
        [Column("ProductId")]
        public int Product_Id { get; set; }
        public DateTime Update_Date { get; set; }

        public virtual Amark.Product Products { get; set; }
    }
}
