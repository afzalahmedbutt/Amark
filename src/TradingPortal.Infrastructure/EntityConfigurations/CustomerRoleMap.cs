using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Infrastructure.EntityConfigurations
{
    public class CustomerRoleMap : IEntityTypeConfiguration<CustomerRole>
    {
        public void Configure(EntityTypeBuilder<CustomerRole> builder)
        {
            builder.ToTable("Customer_CustomerRole_Mapping");
            builder.Property(r => r.UserId).HasColumnName("Customer_Id");
            builder.Property(r => r.RoleId).HasColumnName("CustomerRole_Id");
            //builder.Property(r => r.)
          
        }
    }
}
