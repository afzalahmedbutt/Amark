using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.Property(up => up.PasswordHash).HasColumnName("Password");
            //builder.HasMany<CustomerRole>().WithOne().HasForeignKey(ur => ur.Cu)
            builder.HasMany(u => u.Roles).WithOne(u => u.Customer)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }
}
