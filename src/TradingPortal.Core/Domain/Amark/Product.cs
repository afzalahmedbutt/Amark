using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingPortal.Core.Domain.Amark
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        [Key]
        public int Product_Id { get; set; }
        public string Homepage_Description { get; set; }
        public string Homepage_Weight { get; set; }
        public double? Retail_Bid_Price { get; set; }
        public double? Retail_Ask_Price { get; set; }

        public int? BROCHURE_PRODUCT_ID { get; set; }

        public double? OZCONV { get; set; }
        public string INTERNAL_DESCRIPTION { get; set; }

        public string IS_PERCENTAGE { get; set; }
        public double? PERCENTAGE { get; set; }
        public double? AMOUNT { get; set; }
        public double SPREAD { get; set; }
        public string CODE { get; set; }
        public string COUNTRY { get; set; }
        public string PUBLIC_DESCRIPTION { get; set; }
        public int? PRODUCT_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string COMCODE { get; set; }
        public string QUANDESC { get; set; }
        public string PRODTYPE { get; set; }
        public double? WHOLESALE_BID_PRICE { get; set; }
        public double? WHOLESALE_ASK_PRICE { get; set; }
        public string RETAIL_IS_PERCENTAGE_BID { get; set; }
        public double? RETAIL_PERCENTAGE_BID { get; set; }
        public double? RETAIL_AMOUNT_BID { get; set; }
        public string RETAIL_IS_ACTIVE { get; set; }
        public string RETAIL_IS_PERCENTAGE_ASK { get; set; }
        public double? RETAIL_PERCENTAGE_ASK { get; set; }
        public double? RETAIL_AMOUNT_ASK { get; set; }
        public DateTime LAST_UPDATED { get; set; }
        public int? SORT_ORDER { get; set; }
        public int? HOMEPAGE_ORDER { get; set; }
        public string HOMEPAGE_TEXT_ALIGN { get; set; }
        public string RETAIL_DESCRIPTION { get; set; }
        public int? WHOLESALE_SORT_ORDER { get; set; }
        public int? RETAIL_SORT_ORDER { get; set; }


        public List<Products_Historical_Prices> Products_Historical_Prices { get; set; }
        public virtual Brochure_Products Brochure_Products { get; set; }
        //public virtual WholesalePrices WholesalePrices { get; set; }
    }
}
