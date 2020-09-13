using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Infrastructure.Extensions
{
    public static class ShoppingCartExtensions
    {
        /// <summary>
        /// Indicates whether the shopping cart requires shipping
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <returns>True if the shopping cart requires shipping; otherwise, false.</returns>
        public static bool RequiresShipping(this IList<ShoppingCartItem> shoppingCart)
        {
            foreach (var shoppingCartItem in shoppingCart)
                if (shoppingCartItem.IsShipEnabled)
                    return true;
            return false;
        }
    }
}
