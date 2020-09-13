using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.Domain.Catalog
{
    public enum AttributeValueType
    {
        /// <summary>
        /// Simple attribute value
        /// </summary>
        Simple = 0,
        /// <summary>
        /// Associated to a product (used when configuring bundled products)
        /// </summary>
        AssociatedToProduct = 10,
    }
}
