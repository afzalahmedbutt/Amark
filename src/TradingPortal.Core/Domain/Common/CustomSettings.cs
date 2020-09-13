
using TradingPortal.Core.Configuration;

namespace TradingPortal.Core.Domain.Common
{
    public class CustomSettings : ISettings
    {
        /// <summary>
        /// Gets or sets Portal start business hours
        /// </summary>
        public string dtStartBusiness { get; set; }

        /// <summary>
        /// Gets or sets Portal end business hours
        /// </summary>
        public string dtEndBusiness { get; set; }

        /// <summary>
        /// Gets or sets Price Expired seconds
        /// </summary>
        public decimal PriceExpiredSeconds { get; set; }

        /// <summary>
        /// Gets or sets Portal Trade Notification Email
        /// </summary>
        public string TradeNotificationEmail { get; set; }

        /// <summary>
        /// Gets or sets Portal Trade Notification Name
        /// </summary>
        public string TradeNotificationName { get; set; }

        public int NewProductNoOfDays { get; set; }

        public int MaxProductQuantity { get; set; }
    }
}
