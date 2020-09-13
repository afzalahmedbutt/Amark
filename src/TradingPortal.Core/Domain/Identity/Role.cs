using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradingPortal.Core.Domain.Identity
{
    public class Role : IdentityRole<int>
    {
        //private ICollection<PermissionRecord> _permissionRecords;
        //public int CustomerRoleId { get; set; }
        public bool FreeShipping { get; set; }
        public bool TaxExempt { get; set; }
        public bool Active { get; set; }
        public bool IsSystemRole { get; set; }
        public string SystemName { get; set; }
        public ICollection<CustomerRole> Users { get; set; }
        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        //public virtual ICollection<Customer> Users { get; set; }
        //public virtual ICollection<IdentityUserRole<int>> Users { get; set; }
        public virtual ICollection<PermissionRecordCustomerRole> Permissions { get; set; }
        /// <summary>
        /// Gets or sets the permission records
        /// </summary>
        //[NotMapped]
        //public ICollection<PermissionRecord> PermissionRecords
        //{
        //    get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
        //    protected set { _permissionRecords = value; }
        //}

        //public ICollection<PermissionRecordCustomerRole> CustomerRolePermissions { get; set; }
    }
}
