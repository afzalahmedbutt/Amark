using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain.Amark;

namespace TradingPortal.Business.interfaces
{
    public interface IProductManager
    {
        Task<List<WholeSaleResponseViewModel>> GetWholeSalePrices();
        Task<List<Brochure_Products>> GetAllProductDetails(string comCode = "G");
    }
}
