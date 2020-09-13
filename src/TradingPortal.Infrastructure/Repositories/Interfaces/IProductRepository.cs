using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;
//using TradingPortal.Core.Domain.Catalog;

namespace TradingPortal.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
       
        void UpdatePortalProducts(string name,string shortDescription,decimal weight,int commodityId);
        IList<Product> GetAllActiveProducts();
        Task<IList<ProductViewModel>> GetAllActiveProducts(int customerId);
        //IList<ProductViewModel> GetAllActiveProducts(int customerId);
        Task<bool> DeleteCustomerFavoriteProducts(List<CustomerFavoriteProduct> products);
        Task<bool> AddCustomerFavoriteProducts(List<CustomerFavoriteProduct> products);
    }
}
