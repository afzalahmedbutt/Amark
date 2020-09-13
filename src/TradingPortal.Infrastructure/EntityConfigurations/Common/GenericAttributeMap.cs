using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Common;

namespace TradingPortal.Infrastructure.EntityConfigurations.Common
{
    public class GenericAttributeMap : IEntityTypeConfiguration<GenericAttribute>
    {
        public void Configure(EntityTypeBuilder<GenericAttribute> builder)
        {
            builder.ToTable("GenericAttribute");
            builder.HasKey(ga => ga.Id);

            builder.Property(ga => ga.KeyGroup).IsRequired().HasMaxLength(400);
            builder.Property(ga => ga.Key).IsRequired().HasMaxLength(400);
            builder.Property(ga => ga.Value).IsRequired();
        }
    }
}
