using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingPortal.Core.Domain.Identity
{
    public class CustomerRole : IdentityUserRole<int>
    {
        //public int CustomerId { get; set; }
        //public int RoleId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Role Role { get; set; }
        //[NotMapped]
        //public string Discriminator { get; set; }
    }
}
