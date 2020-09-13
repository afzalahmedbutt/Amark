using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Repositories.Interfaces;

namespace TradingPortal.Infrastructure.Repositories
{
    class ProductRepository : EfRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            dbContext = context;
        }
        public void UpdatePortalProducts(string name, string shortDescription, decimal weight, int commodityId)
        {
            var pName = new SqlParameter("@Name",name);
            var pShortDescription = new SqlParameter("@ShortDescription", shortDescription);
            var pWeight = new SqlParameter("@Weight", weight);
            var pCommodityID = new SqlParameter("@CommodityID", commodityId);
            dbContext.Database.ExecuteSqlCommand("EXEC [spUpdatePortalProducts] @Name, @ShortDescription, @Weight, @CommodityID", 600, pName, pShortDescription, pWeight, pCommodityID);

        }

        public IList<Product> GetAllActiveProducts()
        {
            var query = from p in GetAll()
                        orderby p.CommodityID, p.DisplayOrder, p.ShortDescription
                        where p.Published &&
                        !p.Deleted
                        select p;
            var products = query.ToList();
            return products;
        }

        public async Task<IList<ProductViewModel>> GetAllActiveProducts(int customerId)
        {
            try
            {
                var query = (from p in GetAll()
                             join cfp in dbContext.CustomerFavoriteProducts.Where(cfp => cfp.CustomerId == customerId)
                             on p.Name equals cfp.ProductName into CustomerProducts
                             from cf in CustomerProducts.DefaultIfEmpty()
                             orderby p.CommodityID, p.DisplayOrder, p.ShortDescription
                             where p.Published &&
                             !p.Deleted
                             select new ProductViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 ShortDescription = p.ShortDescription,
                                 Weight = p.Weight,
                                 Sku = p.Sku,
                                 CommodityID = p.CommodityID,
                                 IsFavorite = cf != null,
                                 CreatedOnUtc = p.CreatedOnUtc
                              }).OrderBy(p => p.CommodityID)
                               .ThenByDescending(p => p.IsFavorite).ThenBy(p => p.DisplayOrder).ThenBy(p => p.ShortDescription);

                var products = await query.ToListAsync();
                //products.GroupBy(p => p.CommodityID);
                return products;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFavoriteProducts(Product favProducts)
        {
            using (var connection = dbContext.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Table [CustomerFavoriteProduct]";
                    var result = await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteCustomerFavoriteProducts(List<CustomerFavoriteProduct> products)
        {
            dbContext.CustomerFavoriteProducts.RemoveRange(products);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCustomerFavoriteProducts(List<CustomerFavoriteProduct> products)
        {
            try
            {
                dbContext.CustomerFavoriteProducts.AddRange(products);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        


    }
}
