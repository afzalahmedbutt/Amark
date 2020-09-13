using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingPortal.Core.Domain
{
    public class WebSpotPrices : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public string VsMarket { get; set; }
        public string ComCode { get; set; }
        public decimal? Bid { get; set; }
        public decimal? Ask { get; set; }
        public DateTime? BidAskDate { get; set; }
        public decimal? VsClose { get; set; }
        public decimal? VsChange { get; set; }
        public int? NyTradeId { get; set; }
        public string NyTradeSymbol { get; set; }
        public DateTime? NyTradeDate { get; set; }
        public DateTime Update_Date { get; set; }

        [NotMapped]
        public string ChangeVal { get; set; }
        [NotMapped]
        public string ChangePer { get; set; }
        [NotMapped]
        public string RoundedAsk { get; set; }
        [NotMapped]
        public string RoundedBid { get; set; }
        [NotMapped]
        public string RoundedClose { get; set; }
        [NotMapped]
        public string MetalName { get; set; }
    }
}
