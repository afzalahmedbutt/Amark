using System.Collections.Generic;
using System.Threading.Tasks;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;
using WindowsServiceEndPoint;

namespace TradingPortal.Business.interfaces
{
    public interface IShoppingCartManager
    {
        Task<RequestForOrderModel> RequestForOrder(bool isSell);
        Task<RequestForOrderModel> ReviewOrderRequest(RequestForOrderModel requestForOrderModel);
        Task<bool> ConfirmOrderRequest(bool isSell);
        Task<RequestForOrderModel> MarkUnMarkProductsAsFavorite(RequestForOrderModel requestForOrderModel);
        void DeleteShoppingCartItem(ShoppingCartItem item);
        Task<List<SelectListItem>> GetVendors();
        Task DeleteShoppingCartItems();
        MTS_Api MtsApi { get;}
    }
}