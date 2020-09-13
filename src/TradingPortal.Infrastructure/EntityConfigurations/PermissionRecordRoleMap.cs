using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    class PermissionRecordRoleMap : IEntityTypeConfiguration<PermissionRecordCustomerRole>
    {
        
        public void Configure(EntityTypeBuilder<PermissionRecordCustomerRole> builder)
        {
            builder.ToTable("PermissionRecord_Role_Mapping");
            builder.HasKey(p => new {p.PermissionId,p.RoleId });
            builder.Property(r => r.PermissionId).HasColumnName("PermissionRecord_Id");
            builder.Property(r => r.RoleId).HasColumnName("CustomerRole_Id");
        }
    
}
}
