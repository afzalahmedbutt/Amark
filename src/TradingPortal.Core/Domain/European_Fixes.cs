using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradingPortal.Core.Domain
{
    public class European_Fixes
    {
        [Key]
        public int European_Fixes_Id { get; set; }

        public DateTime Date { get; set; }
        public string USD_Gold_AM { get; set; }
        public string USD_Gold_PM { get; set; }
        public string USD_Silver { get; set; }
        public string USD_Platinum_AM { get; set; }
        public string USD_Platinum_PM { get; set; }
        public string USD_Palladium_AM { get; set; }
        public string USD_Palladium_PM { get; set; }
    }
}
