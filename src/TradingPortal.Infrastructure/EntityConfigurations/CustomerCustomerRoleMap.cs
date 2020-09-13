using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    class CustomerCustomerRoleMap : IEntityTypeConfiguration<CustomerRole>
    {
        public void Configure(EntityTypeBuilder<CustomerRole> builder)
        {
            builder.ToTable("Customer_CustomerRole_Mapping");
            //builder.HasKey(cr => new { cr.CustomerId, cr.RoleId });
            builder.Property(r => r.UserId).HasColumnName("Customer_Id");
            builder.Property(r => r.RoleId).HasColumnName("CustomerRole_Id");
            //builder.Ignore("Discriminator");
            //builder.HasMany(u => u.Roles).WithOne(r => r.Customer);
            //builder.HasOne(u => u.Customer).WithMany(u => u.Roles)
            //    .HasForeignKey(r => r.RoleId)
            //    .IsRequired();

            //builder.HasOne(u => u.CustomerRole).WithMany(r => r.Customers);
            //builder.HasOne(ccr => ccr.Customer)
            //    .WithMany(c => c.CustomerRoles)
            //    .HasForeignKey(ccr => ccr.UserId);

            //builder.HasOne(ccr => ccr.CustomerRole)
            //    .WithMany(c => c.CustomerRoles)
            //    .HasForeignKey(ccr => ccr.RoleId);
        }
    }
}
