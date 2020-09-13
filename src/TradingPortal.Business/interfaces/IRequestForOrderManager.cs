using MTSWebApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.Enums;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Business.interfaces
{
    public interface IRequestForOrderManager
    {
        Task DeleteShoppingCartItems();
        Task<MetalsTotal> GetMetalsTotal(IList<ShoppingCartItem> cart);
        Task<ProductsDiscountsInfo> GetAMarkWebProductsDiscounts();
        Task<ProductsPricingInfo> GetAMarkWebProductsPricing(MetalsTotal metalsTotal, bool isSell);
        Task SetRequestForOrderItems(RequestForOrderModel model, IList<ShoppingCartItem> cart, ProductPricingItem[] productPricingItems);
        Task SetEditRequestForOrderItems(RequestForOrderModel model, IList<ShoppingCartItem> cart, ProductPricingItem[] productPricingItems);
        //Task<ProductsQuoteInfo> RequestAmarkOnlineQuote(string sOrderType, List<ProductQuoteItem> lstProductQuoteItems, bool bHFIFlag, string sShippingType, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber, string sTP_Confirm_No, string sSpecialInstructions);
        Task<ProductsQuoteInfo> RequestAmarkOnlineQuote(RequestForOrderModel requestForOrderModel, List<ProductQuoteItem> productQuoteItems);
        Task<bool> AddressValid(string sCountry, string sCity, string sState, string sZip);
        IList<ShoppingCartItem> GetUserShoppingCartItems();
        Task<OpenPositionInfo> GetOpenPositions();
        Task<TradingHistoryInfo> GetTradingHistory(DateTime dtBegin, DateTime dtEnd, int iPageNumber, int iPageSize);
        Task<bool> UpdateOrderShippingInfo(int iRequestID, int iOrderHdrID, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber);
        Task<ShipToAddressItem> GetOrderShippingItem(int iRequestID, int iOrderHdrID);
        IList<string> UpdateShoppingCartItem(Customer customer, int shoppingCartItemId,
            int newQuantity, bool resetCheckoutData, decimal decimalQuantity);
        IList<string> AddToCart(Customer customer, Product product,
            ShoppingCartType shoppingCartType, int storeId, string selectedAttributes,
            decimal customerEnteredPrice, int quantity, bool automaticallyAddRequiredProductsIfEnabled, decimal decimalQuantity);
        Task<CustomerCommodityLimitsInfo> GetAMarkWebTradingLimits(string sOrderType);
        Task<RequestOnlineTradeInfo> RequestAmarkOnlineTrade(string sOnline_Request_Type, string sQuoteKey, bool bHFIFlag, string sShippingType, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber, string sTP_Confirm_No);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> AddRemoveFavoriteProduct(CustomerFavoriteProduct product);
        string CreateTradeConfirmationEmailData(RequestForOrderModel model);


    }
}
