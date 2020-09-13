using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Infrastructure.DatabaseContexts
{
    public class AMarkDbContext : DbContext
    {
        private readonly ICurrentUser _currentUser;
        public AMarkDbContext(DbContextOptions<AMarkDbContext> options, ICurrentUser currentUser)
            :base(options)
        {
            _currentUser = currentUser;
        }

        public AMarkDbContext()
        {

        }

        public DbSet<WebSpotPrices> WebSpotPrices { get; set; }
        public DbSet<SpotPriceHistory> SpotPriceHistory { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<European_Fixes> European_Fixes { get; set; }
        public DbSet<SpotPriceDaily> SpotPriceDaily { get; set; }
        public DbSet<WholesalePrices> WholesalePrices { get; set; }
        public DbSet<Brochure_Products> Brochure_Products { get; set; }
        public DbSet<Content> Content { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Core.Domain.Amark.Product>().Ignore(p => p.Id);
            builder.Entity<Core.Domain.Amark.Brochure_Products>().Ignore(p => p.Id);
            builder.Entity<Core.Domain.Amark.Brochure_Product_Variants>().Ignore(p => p.Id);
            builder.Entity<Content>().Ignore(c => c.Id);
            builder.Entity<WholesalePrices>().HasKey(wsp => wsp.Id);
           
            builder.Entity<WholesalePrices>()
                .HasOne(asp => asp.Products)
                .WithOne()
                .HasForeignKey<WholesalePrices>(wsp => wsp.Product_Id);

            builder.Entity<Brochure_Products>()
                .HasMany(bp => bp.Brochure_Product_Variants)
                .WithOne(bpv => bpv.Brochure_Products);
        }


        [DbFunction]
        public static int ufnCheckIsNumeric(string field)
        {
            throw new Exception();
        }

        [DbFunction]
        public static int ufnDiffDays(DateTime endDate,DateTime startDate)
        {
            throw new Exception();
        }
    }
}
