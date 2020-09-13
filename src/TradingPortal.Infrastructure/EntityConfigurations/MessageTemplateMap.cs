using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    public class MessageTemplateMap : IEntityTypeConfiguration<MessageTemplate>
    {
        public void Configure(EntityTypeBuilder<MessageTemplate> builder)
        {
            builder.ToTable("MessageTemplate");
            builder.HasKey(t => t.Id);
        }
    }

}
