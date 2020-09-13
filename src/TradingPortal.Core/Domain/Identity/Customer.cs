using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingPortal.Core.Domain.Identity
{
    public class Customer : IdentityUser<int>,ITaggingInterface
    {
        private ICollection<ShoppingCartItem> _shoppingCartItems;

        public int PasswordFormatId { get; set; }
        public string PasswordSalt { get; set; }
        public Guid CustomerGuid { get; set; }
        public bool IsTaxExempt { get; set; }
        public bool IsSystemAccount { get; set; }
        public int AffiliateId { get; set; }
        public int VendorId { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime LastActivityDateUtc { get; set; }
        public virtual ICollection<CustomerRole> Roles { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        //[NotMapped]
        //public virtual ICollection<CustomerRole> Roles { get; set; }

        /// <summary>
        /// Gets or sets shopping cart items
        /// </summary>
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems
        {
            get { return _shoppingCartItems ?? (_shoppingCartItems = new List<ShoppingCartItem>()); }
            protected set { _shoppingCartItems = value; }
        }
    }
}
