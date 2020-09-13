using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    public class TopicMap : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("Topic");
            builder.HasKey(t => t.Id);
        }
    }
}
