using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("CustomerRole");
            builder.HasMany(r => r.Users)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            //builder.Ignore(p => p.PermissionRecords);
            //builder.HasKey(r => r.Id);
            //builder.Property(r => r.Id).HasColumnName("Id");
            //builder.Ignore(r => r.Id);
            //builder.HasKey(r => r.);
            //builder.Property(r => r.CustomerRoleId).HasColumnName("Id");

            //builder.Property(r => r.Id).HasColumnName("Id");
            //builder.Property(r => r.Id).HasColumnName("Id");
            //builder.Property(r => r.Id).HasColumnName("Id");
            //builder.Property(r => r.Id).HasColumnName("Id");
        }
    }
}
