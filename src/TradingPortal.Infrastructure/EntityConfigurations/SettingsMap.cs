using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Configuration;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    class SettingsMap : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Setting");
            builder.HasKey(t => t.Id);
        }
    }
}
