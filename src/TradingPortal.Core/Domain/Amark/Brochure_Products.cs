using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.Domain.Amark
{
    public class Brochure_Products : BaseEntity
    {
        [Key]
        public int Brochure_Product_Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Section { get; set; }
        public string Fineness { get; set; }
        public string Image_Path { get; set; }
        public string Back_Image_Path { get; set; }
        public int? Sort_Order { get; set; }
        public string Description { get; set; }
        public string Is_Active { get; set; }

        public virtual List<Brochure_Product_Variants> Brochure_Product_Variants { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
        public virtual List<Product> Products { get; set; }

    }
}
