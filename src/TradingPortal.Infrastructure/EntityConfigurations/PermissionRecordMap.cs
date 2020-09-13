using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Identity;


namespace TradingPortal.Infrastructure.EntityConfigurations
{
    public class PermissionRecordMap : IEntityTypeConfiguration<PermissionRecord>
    {
        public void Configure(EntityTypeBuilder<PermissionRecord> builder)
        {
            builder.ToTable("PermissionRecord");
            builder.HasKey(pr => pr.Id);
            builder.Property(pr => pr.Name).IsRequired();
            builder.Property(pr => pr.SystemName).IsRequired().HasMaxLength(255);
            builder.Property(pr => pr.Category).IsRequired().HasMaxLength(255);

            //builder.HasMany(pr => pr.CustomerRoles)
            //    .WithMany(cr => cr.PermissionRecords)
            //    .Map(m => m.ToTable("PermissionRecord_Role_Mapping"));
        }
    }
}
