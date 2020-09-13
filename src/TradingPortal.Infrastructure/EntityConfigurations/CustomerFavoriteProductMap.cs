using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    class CustomerFavoriteProductMap : IEntityTypeConfiguration<CustomerFavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<CustomerFavoriteProduct> builder)
        {
            builder.ToTable("CustomerFavoriteProduct");
            builder.Ignore(cfp => cfp.Id);
            builder.HasKey(cfp => new { cfp.CustomerId,cfp.ProductName });

            builder.Ignore(cfp => cfp.IsFavorite);
            
        }
    }
}
