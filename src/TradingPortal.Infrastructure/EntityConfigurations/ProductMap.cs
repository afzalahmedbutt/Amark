using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(400);
            builder.Property(p => p.MetaKeywords).HasMaxLength(400);
            builder.Property(p => p.MetaTitle).HasMaxLength(400);
            builder.Property(p => p.Sku).HasMaxLength(400);
            builder.Property(p => p.ManufacturerPartNumber).HasMaxLength(400);
            builder.Property(p => p.Gtin).HasMaxLength(400);
            builder.Property(p => p.AdditionalShippingCharge).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Price).HasColumnType("decimal(18,4)");
            builder.Property(p => p.OldPrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.ProductCost).HasColumnType("decimal(18,4)");
            builder.Property(p => p.SpecialPrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.MinimumCustomerEnteredPrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.MaximumCustomerEnteredPrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Weight).HasColumnType("decimal(19,5)");
            builder.Property(p => p.Length).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Width).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Height).HasColumnType("decimal(18,4)");
            builder.Property(p => p.RequiredProductIds).HasMaxLength(1000);
            builder.Property(p => p.AllowedQuantities).HasMaxLength(1000);

            //builder.Ignore(p => p.ProductType);
            //builder.Ignore(p => p.BackorderMode);
            //builder.Ignore(p => p.DownloadActivationType);
            //builder.Ignore(p => p.GiftCardType);
            //builder.Ignore(p => p.LowStockActivity);
            //builder.Ignore(p => p.ManageInventoryMethod);
            //builder.Ignore(p => p.RecurringCyclePeriod);
            //AMark custom
            //builder.Ignore(p => p.ProductCommodityType);
        }
    }
}
