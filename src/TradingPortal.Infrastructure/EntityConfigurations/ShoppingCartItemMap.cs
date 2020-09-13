using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingPortal.Core.Domain;


namespace TradingPortal.Infrastructure.EntityConfigurations
{
    
    public class ShoppingCartItemMap : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.ToTable("ShoppingCartItem");
            builder.HasKey(sci => sci.Id);

            builder.Property(sci => sci.CustomerEnteredPrice).HasColumnType("decimal(18,4)"); //.HasPrecision(18, 4);

            builder.Property(sci => sci.DecimalQuantity).HasColumnType("decimal(15,5)");//.HasPrecision(15, 5);

            builder.Ignore(sci => sci.ShoppingCartType);
            builder.Ignore(sci => sci.IsFreeShipping);
            builder.Ignore(sci => sci.IsShipEnabled);
            builder.Ignore(sci => sci.AdditionalShippingCharge);
            builder.Ignore(sci => sci.IsTaxExempt);

            builder.HasOne(sci => sci.Customer)
                .WithMany(c => c.ShoppingCartItems)
                .IsRequired();
            //builder.HasRequired(sci => sci.Customer)
            //    .WithMany(c => c.ShoppingCartItems)
            //    .HasForeignKey(sci => sci.CustomerId);

            builder.HasOne(sci => sci.Product)
               .WithMany()
               .HasForeignKey(sci => sci.ProductId)
               .IsRequired();

            //builder.HasRequired(sci => sci.Product)
            //    .WithMany()
            //    .HasForeignKey(sci => sci.ProductId);
        }
    }
}
