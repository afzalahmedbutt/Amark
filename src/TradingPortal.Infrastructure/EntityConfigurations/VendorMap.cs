using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    class VendorMap : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("Vendor");
            builder.HasKey(t => t.Id);

            builder.Property(v => v.Name).IsRequired().HasMaxLength(400);
            builder.Property(v => v.Email).HasMaxLength(400);
        }
    }
}
