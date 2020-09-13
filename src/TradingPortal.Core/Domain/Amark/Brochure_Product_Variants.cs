using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.Domain.Amark
{
    public class Brochure_Product_Variants : BaseEntity
    {
        [Key]
        public int Brochure_Product_Variant_Id { get; set; }
        public int Brochure_Product_Id { get; set; }
        public string Size { get; set; }
        public string Diameter { get; set; }
        public string Thickness { get; set; }
        public string Dimensions { get; set; }
        public decimal Weight { get; set; }
        [JsonIgnore]
        public virtual Brochure_Products Brochure_Products { get; set; }
    }
}
