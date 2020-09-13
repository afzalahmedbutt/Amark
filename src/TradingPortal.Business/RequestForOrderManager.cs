using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using MTSWebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Common;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.Enums;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure;
using TradingPortal.Infrastructure.Repositories.Interfaces;
using TradingPortal.Infrastructure.Services.Interfaces;
using WindowsServiceEndPoint;

namespace TradingPortal.Business
{
    public class RequestForOrderManager : IRequestForOrderManager
    {
        private readonly IStoreCommandRepository _storeCommandRepository;
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly IProductRepository _productRepository;
        private MTS_Api _mtsApi;
        private readonly CustomerAttributes _customerAttributes;
        private readonly IConfiguration _config;
        private readonly CustomSettings _customSettings;
        private readonly ICustomerManager _customerManager;
        private readonly IRepository<CustomerFavoriteProduct> _favoriteProductRepostiry;


        public RequestForOrderManager(
            ICurrentUser currentUser,
            IRepository<ShoppingCartItem> shoppingCartItemRepository,
            CustomerAttributes customerAttributes,
            IProductRepository productRepository,
            IConfiguration config,
            IStoreCommandRepository storeCommandRepository,
            IGenericAttributeManager genericAttributeManager,
            CustomSettings customSettings,
            ICustomerManager customerManager,
            IRepository<CustomerFavoriteProduct> favoriteProductRepostiry)
        {
            _currentUser = currentUser;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _customerAttributes = customerAttributes;
            _productRepository = productRepository;
            _config = config;
            _storeCommandRepository = storeCommandRepository;
            _genericAttributeManager = genericAttributeManager;
            _customSettings = customSettings;
            _customerManager = customerManager;
            _favoriteProductRepostiry = favoriteProductRepostiry;
        }

        public async Task DeleteShoppingCartItems()
        {
            var cart = _currentUser.User.ShoppingCartItems
                   .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest)
                   .Where(sci => sci.StoreId == 1)
                   .ToList();
            _shoppingCartItemRepository.DeleteRange(cart);
            await _shoppingCartItemRepository.SaveAsync();
        }

        public async Task<MetalsTotal> GetMetalsTotal(IList<ShoppingCartItem> cart)
        {
            MetalsTotal metalsTotal = new MetalsTotal();
            foreach (ShoppingCartItem sci in cart)
            {
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Gold")
                {
                    metalsTotal.DecGoldTotal = metalsTotal.DecGoldTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Silver")
                {
                    metalsTotal.DecSilverTotal = metalsTotal.DecSilverTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Platinum")
                {
                    metalsTotal.DecPlatinumTotal = metalsTotal.DecPlatinumTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Palladium")
                {
                    metalsTotal.DecPalladiumTotal = metalsTotal.DecPalladiumTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);
                }
            }
            return metalsTotal;
        }

        public async Task<ProductsDiscountsInfo> GetAMarkWebProductsDiscounts()
        {
            return await MtsApi.GetAMarkWebProductsDiscounts();
        }

        public async Task<ProductsPricingInfo> GetAMarkWebProductsPricing(MetalsTotal metalsTotal, bool isSell)
        {
            if (isSell)
            {
                return await MtsApi.GetAMarkWebProductsPricingToSell(metalsTotal.DecGoldTotal, metalsTotal.DecSilverTotal, metalsTotal.DecPlatinumTotal, metalsTotal.DecPalladiumTotal);
            }
            else
            {
                return await MtsApi.GetAMarkWebProductsPricingToBuy(metalsTotal.DecGoldTotal, metalsTotal.DecSilverTotal, metalsTotal.DecPlatinumTotal, metalsTotal.DecPalladiumTotal);
            }
        }

        public async Task<bool> UpdateOrderShippingInfo(int iRequestID, int iOrderHdrID, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber)
        {
            return await MtsApi.UpdateOrderShippingInfoWithOrderHdrID(iRequestID.ToString(), iOrderHdrID.ToString(), "Drop Ship",sShippingName1, sShippingName2, sShippingAddress1, sShippingAddress2, sShippingCity, sShippingState, sShippingZipCode, sShippingCountry, sShippingPhoneNumber, "OnlinePortalLogin");
        }

        public async Task SetRequestForOrderItems(RequestForOrderModel model, IList<ShoppingCartItem> cart, ProductPricingItem[] objWebProductsPricingList)
        {
            IList<ProductViewModel> products = await _productRepository.GetAllActiveProducts(_currentUser.User.Id);
            if (model.IsEditable)
            {

                if (objWebProductsPricingList != null && objWebProductsPricingList.Count() > 0)
                {
                    var query = (from product in products
                                 join pp in objWebProductsPricingList on product.Name equals pp.sProductCode
                                 select new
                                 {
                                     pp.sCommodityDesc,
                                     product.Sku,
                                     product.Id,
                                     product.Name,
                                     product.ShortDescription,
                                     product.Weight,
                                     product.IsFavorite,
                                     product.CommodityID,
                                     product.CreatedOnUtc,
                                     pp.decSpotPrice,
                                     pp.bPremiumIsPercent,
                                     pp.decProductPremium,
                                     pp.iMinPurchase,
                                     pp.decPurchaseIncrement,
                                     pp.decUnitPrice,
                                     pp.sTierPrices
                                 }).ToList();

                    foreach (var item in query)
                    {
                        var reqForOrderItemModel = new RequestForOrderModel.RequestForOrderItemModel()
                        {
                            Commodity = item.sCommodityDesc,
                            CommodityID = item.CommodityID,
                            Sku = item.Sku,
                            ProductId = item.Id,
                            ProductName = item.Name,
                            ProductDesc = item.ShortDescription,
                            ProductWeight = item.Weight,
                            WeightSubTotal = 0,
                            SpotPrice = item.decSpotPrice,
                            PremiumIsPercent = item.bPremiumIsPercent,
                            ProductPremium = item.decProductPremium,
                            MinPurchase = item.iMinPurchase,
                            PurchaseIncrement = item.decPurchaseIncrement,
                            UnitPrice = item.decUnitPrice,//Math.Round((item.bPremiumIsPercent == true ? ((item.Weight * item.decSpotPrice) * (1 + item.decProductPremium / 100)) : ((item.Weight * item.decSpotPrice) + item.decProductPremium)), 2),
                            Quantity = 0,
                            SubTotal = 0,
                            TierPrices = item.sTierPrices,
                            IsFavorite = item.IsFavorite,
                            IsNew = (DateTime.UtcNow - item.CreatedOnUtc).TotalDays <= _customSettings.NewProductNoOfDays
                        };
                        model.Items.Add(reqForOrderItemModel);

                    }
                }
            }
            else
            {
                //IList<ProductViewModel> products = _productRepository.GetAllActiveProducts(_currentUser.User.Id);

                var query = (from product in products
                             join pp in objWebProductsPricingList on product.Name equals pp.sProductCode
                             join sc in cart on product.Id equals sc.ProductId
                             where sc.DecimalQuantity != 0
                             select new
                             {
                                 pp.sCommodityDesc,
                                 product.Sku,
                                 product.Id,
                                 product.Name,
                                 product.ShortDescription,
                                 product.Weight,
                                 product.IsFavorite,
                                 product.CreatedOnUtc,
                                 pp.decSpotPrice,
                                 pp.bPremiumIsPercent,
                                 pp.decProductPremium,
                                 pp.iMinPurchase,
                                 pp.decPurchaseIncrement,
                                 sc.DecimalQuantity,
                                 pp.decUnitPrice,
                                 pp.sTierPrices
                             }

                             ).ToList();

                foreach (var item in query)
                {
                    var reqForOrderItemModel = new RequestForOrderModel.RequestForOrderItemModel()
                    {
                        Commodity = item.sCommodityDesc,
                        Sku = item.Sku,
                        ProductId = item.Id,
                        ProductName = item.Name,
                        ProductDesc = item.ShortDescription,
                        ProductWeight = item.Weight,
                        WeightSubTotal = Math.Round(item.Weight * item.DecimalQuantity, 5),
                        SpotPrice = item.decSpotPrice,
                        PremiumIsPercent = item.bPremiumIsPercent,
                        ProductPremium = item.decProductPremium,
                        MinPurchase = item.iMinPurchase,
                        PurchaseIncrement = item.decPurchaseIncrement,
                        UnitPrice = item.decUnitPrice,//Math.Round((item.bPremiumIsPercent == true ? ((item.Weight * item.decSpotPrice) * (1 + item.decProductPremium / 100)) : ((item.Weight * item.decSpotPrice) + item.decProductPremium)), 2),
                        Quantity = item.DecimalQuantity,
                        //SubTotal = Math.Round((item.bPremiumIsPercent == true ? (((item.Weight * item.decSpotPrice) * (1 + item.decProductPremium / 100)) * item.DecimalQuantity) : (((item.Weight * item.decSpotPrice) + item.decProductPremium) * item.DecimalQuantity)), 2),
                        SubTotal = Math.Round(item.decUnitPrice * item.DecimalQuantity, 2),
                        TierPrices = item.sTierPrices,
                        IsFavorite = item.IsFavorite,
                        IsNew = (DateTime.UtcNow - item.CreatedOnUtc).TotalDays <= _customSettings.NewProductNoOfDays
                    };
                    model.Items.Add(reqForOrderItemModel);
                }
            }
            model.Items = model.Items.OrderBy(i => i.CommodityID)
                               .ThenByDescending(i => i.IsNew)
                               .ThenByDescending(i => i.IsFavorite)
                               .ThenBy(i => i.ProductDesc)
                               .ToList();
        }

        public async Task SetEditRequestForOrderItems(RequestForOrderModel model, IList<ShoppingCartItem> cart, ProductPricingItem[] objWebProductsPricingList)
        {
            IList<ProductViewModel> products = await _productRepository.GetAllActiveProducts(_currentUser.User.Id);
            var query = (from product in products
                         join pp in objWebProductsPricingList on product.Name equals pp.sProductCode
                         join sc in cart on product.Id equals sc.ProductId into scp
                         from s in scp.DefaultIfEmpty()
                         select new
                         {
                             pp.sCommodityDesc,
                             product.Sku,
                             product.Id,
                             product.Name,
                             product.ShortDescription,
                             product.CreatedOnUtc,
                             product.Weight,
                             product.IsFavorite,
                             pp.decSpotPrice,
                             pp.bPremiumIsPercent,
                             pp.decProductPremium,
                             pp.iMinPurchase,
                             pp.decPurchaseIncrement,
                             Quantity = s == null ? 0 : s.DecimalQuantity,
                             pp.decUnitPrice,
                             pp.sTierPrices
                         }

                         ).ToList();
            foreach (var item in query)
            {
                var reqForOrderItemModel = new RequestForOrderModel.RequestForOrderItemModel()
                {
                    Commodity = item.sCommodityDesc,
                    Sku = item.Sku,
                    ProductId = item.Id,
                    ProductName = item.Name,
                    ProductDesc = item.ShortDescription,
                    ProductWeight = item.Weight,
                    WeightSubTotal = Math.Round(item.Weight * item.Quantity, 5),
                    SpotPrice = item.decSpotPrice,
                    PremiumIsPercent = item.bPremiumIsPercent,
                    ProductPremium = item.decProductPremium,
                    MinPurchase = item.iMinPurchase,
                    PurchaseIncrement = item.decPurchaseIncrement,
                    UnitPrice = item.decUnitPrice,//Math.Round((item.bPremiumIsPercent == true ? ((item.Weight * item.decSpotPrice) * (1 + item.decProductPremium / 100)) : ((item.Weight * item.decSpotPrice) + item.decProductPremium)), 2),
                    Quantity = item.Quantity,
                    // SubTotal = Math.Round((item.bPremiumIsPercent == true ? (((item.Weight * item.decSpotPrice) * (1 + item.decProductPremium / 100)) * item.Quantity) : (((item.Weight * item.decSpotPrice) + item.decProductPremium) * item.Quantity)), 2),
                    SubTotal = Math.Round(item.decUnitPrice * item.Quantity, 2),
                    TierPrices = item.sTierPrices,
                    IsFavorite = item.IsFavorite,
                    IsNew = (DateTime.UtcNow - item.CreatedOnUtc).TotalDays <= _customSettings.NewProductNoOfDays
                };
                model.Items.Add(reqForOrderItemModel);
            }
            model.Items = model.Items.OrderBy(i => i.CommodityID)
                                .ThenByDescending(i => i.IsNew)
                                .ThenByDescending(i => i.IsFavorite)
                                .ThenBy(i => i.ProductDesc)
                                .ToList();
        }

        public virtual IList<string> AddToCart(Customer customer, Product product,
            ShoppingCartType shoppingCartType, int storeId, string selectedAttributes,
            decimal customerEnteredPrice, int quantity, bool automaticallyAddRequiredProductsIfEnabled, decimal decimalQuantity)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();
            var cart = _currentUser.User.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest
                              && sci.StoreId == storeId)
                              .ToList();


            var shoppingCartItem = FindShoppingCartItemInTheCart(cart,
                shoppingCartType, product, selectedAttributes, customerEnteredPrice);

            if (shoppingCartItem != null)
            {
                int newQuantity = 0;
                decimal newDecimalQuantity = 0;
                //update existing shopping cart item
                if (shoppingCartType != ShoppingCartType.AMarkOrderRequest)
                {
                    newQuantity = shoppingCartItem.Quantity + quantity;
                    newDecimalQuantity = shoppingCartItem.DecimalQuantity + decimalQuantity;
                }
                else
                {
                    newQuantity = quantity;
                    newDecimalQuantity = decimalQuantity;
                }
                warnings.AddRange(GetShoppingCartItemWarnings(customer, shoppingCartType, product,
                    storeId, selectedAttributes,
                    customerEnteredPrice, newQuantity, automaticallyAddRequiredProductsIfEnabled));

                if (warnings.Count == 0 || shoppingCartType == ShoppingCartType.AMarkOrderRequest)
                {
                    shoppingCartItem.AttributesXml = selectedAttributes;
                    shoppingCartItem.Quantity = newQuantity;
                    shoppingCartItem.DecimalQuantity = newDecimalQuantity;
                    shoppingCartItem.UpdatedOnUtc = DateTime.UtcNow;
                    _customerManager.UpdateCustomer(customer);

                }
            }
            else
            {
                // warnings.AddRange(GetShoppingCartItemWarnings(customer, shoppingCartType, product,
                //storeId, selectedAttributes, customerEnteredPrice, quantity, automaticallyAddRequiredProductsIfEnabled));
                if (warnings.Count == 0)
                {
                    //maximum items validation
                    switch (shoppingCartType)
                    {
                        //case ShoppingCartType.ShoppingCart:
                        //    {
                        //        if (cart.Count >= _shoppingCartSettings.MaximumShoppingCartItems)
                        //        {
                        //            warnings.Add(string.Format(_localizationService.GetResource("ShoppingCart.MaximumShoppingCartItems"), _shoppingCartSettings.MaximumShoppingCartItems));
                        //            return warnings;
                        //        }
                        //    }
                        //    break;
                        //case ShoppingCartType.Wishlist:
                        //    {
                        //        if (cart.Count >= _shoppingCartSettings.MaximumWishlistItems)
                        //        {
                        //            warnings.Add(string.Format(_localizationService.GetResource("ShoppingCart.MaximumWishlistItems"), _shoppingCartSettings.MaximumWishlistItems));
                        //            return warnings;
                        //        }
                        //    }
                        //    break;
                        default:
                            break;
                    }

                    DateTime now = DateTime.UtcNow;
                    shoppingCartItem = new ShoppingCartItem()
                    {
                        ShoppingCartType = shoppingCartType,
                        StoreId = storeId,
                        Product = product,
                        AttributesXml = selectedAttributes,
                        CustomerEnteredPrice = customerEnteredPrice,
                        Quantity = quantity,
                        DecimalQuantity = decimalQuantity,
                        CreatedOnUtc = now,
                        UpdatedOnUtc = now
                    };

                    customer.ShoppingCartItems.Add(shoppingCartItem);
                    _customerManager.UpdateCustomer(customer);

                }
            }


            return warnings;
        }



        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="shoppingCartItemId">Shopping cart item identifier</param>
        /// <param name="newQuantity">New shopping cart item quantity</param>
        /// <param name="resetCheckoutData">A value indicating whether to reset checkout data</param>
        /// <param name="decimalQuantity">Quantity as decimal for Amark REPO products</param>
        /// <returns>Warnings</returns>
        public virtual IList<string> UpdateShoppingCartItem(Customer customer, int shoppingCartItemId,
            int newQuantity, bool resetCheckoutData, decimal decimalQuantity)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var warnings = new List<string>();

            var shoppingCartItem = customer.ShoppingCartItems.FirstOrDefault(sci => sci.Id == shoppingCartItemId);
            if (shoppingCartItem != null)
            {

                if (decimalQuantity > 0)
                {
                    //check warnings
                    warnings.AddRange(GetShoppingCartItemWarnings(customer, shoppingCartItem.ShoppingCartType,
                        shoppingCartItem.Product, shoppingCartItem.StoreId,
                        shoppingCartItem.AttributesXml,
                        shoppingCartItem.CustomerEnteredPrice, newQuantity, false));
                    if (warnings.Count == 0 || shoppingCartItem.ShoppingCartType == ShoppingCartType.AMarkOrderRequest)
                    {
                        //if everything is OK, then update a shopping cart item
                        shoppingCartItem.Quantity = newQuantity;
                        shoppingCartItem.DecimalQuantity = decimalQuantity;
                        shoppingCartItem.UpdatedOnUtc = DateTime.UtcNow;
                        _customerManager.UpdateCustomer(customer);

                        //event notification
                        //_eventPublisher.EntityUpdated(shoppingCartItem);
                    }
                }
                else
                {
                    //delete a shopping cart item
                    DeleteShoppingCartItem(shoppingCartItem);
                    //DeleteShoppingCartItem(shoppingCartItem, resetCheckoutData, true);
                }
            }

            return warnings;
        }

        /// <summary>
        /// Delete shopping cart item
        /// </summary>
        /// <param name="shoppingCartItem">Shopping cart item</param>
        /// <param name="resetCheckoutData">A value indicating whether to reset checkout data</param>
        /// <param name="ensureOnlyActiveCheckoutAttributes">A value indicating whether to ensure that only active checkout attributes are attached to the current customer</param>
        public virtual void DeleteShoppingCartItem(ShoppingCartItem shoppingCartItem, bool resetCheckoutData = true,
            bool ensureOnlyActiveCheckoutAttributes = false)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException("shoppingCartItem");

            var customer = shoppingCartItem.Customer;
            var storeId = shoppingCartItem.StoreId;


            //delete item
            _shoppingCartItemRepository.Delete(shoppingCartItem);

            //validate checkout attributes
            if (ensureOnlyActiveCheckoutAttributes &&
                //only for shopping cart items (ignore wishlist)
                shoppingCartItem.ShoppingCartType == ShoppingCartType.ShoppingCart)
            {
                var cart = customer.ShoppingCartItems
                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .Where(x => x.StoreId == storeId)
                    .ToList();

            }

            //event notification
            //_eventPublisher.EntityDeleted(shoppingCartItem);
        }

        public virtual ShoppingCartItem FindShoppingCartItemInTheCart(IList<ShoppingCartItem> shoppingCart,
            ShoppingCartType shoppingCartType,
            Product product,
            string selectedAttributes = "",
            decimal customerEnteredPrice = decimal.Zero)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException("shoppingCart");

            if (product == null)
                throw new ArgumentNullException("product");

            var shoppingCartItem = shoppingCart.Where(sci => sci.ShoppingCartType == shoppingCartType && sci.ProductId == product.Id)
                .FirstOrDefault();
            return shoppingCartItem;
        }

        
        /// <summary>
        /// Validates shopping cart item
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="product">Product</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="selectedAttributes">Selected attributes</param>
        /// <param name="customerEnteredPrice">Customer entered price</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="automaticallyAddRequiredProductsIfEnabled">Automatically add required products if enabled</param>
        /// <param name="getStandardWarnings">A value indicating whether we should validate a product for standard properties</param>
        /// <param name="getAttributesWarnings">A value indicating whether we should validate product attributes</param>
        /// <param name="getGiftCardWarnings">A value indicating whether we should validate gift card properties</param>
        /// <param name="getRequiredProductVariantWarnings">A value indicating whether we should validate required products (products which require other products to be added to the cart)</param>
        /// <returns>Warnings</returns>
        public virtual IList<string> GetShoppingCartItemWarnings(Customer customer, ShoppingCartType shoppingCartType,
            Product product, int storeId,
            string selectedAttributes, decimal customerEnteredPrice,
            int quantity, bool automaticallyAddRequiredProductsIfEnabled,
            bool getStandardWarnings = true, bool getAttributesWarnings = true,
            bool getGiftCardWarnings = true, bool getRequiredProductVariantWarnings = true)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();

            //standard properties
            if (getStandardWarnings)
                warnings.AddRange(GetStandardWarnings(customer, shoppingCartType, product, selectedAttributes, customerEnteredPrice, quantity));

            //selected attributes
            if (getAttributesWarnings)
                warnings.AddRange(GetShoppingCartItemAttributeWarnings(customer, shoppingCartType, product, selectedAttributes));

            //gift cards
            if (getGiftCardWarnings)
                warnings.AddRange(GetShoppingCartItemGiftCardWarnings(shoppingCartType, product, selectedAttributes));

            //required products
            if (getRequiredProductVariantWarnings)
                warnings.AddRange(GetRequiredProductWarnings(customer, shoppingCartType, product, storeId, automaticallyAddRequiredProductsIfEnabled));

            return warnings;
        }

        public virtual IList<string> GetStandardWarnings(Customer customer, ShoppingCartType shoppingCartType,
            Product product, string selectedAttributes, decimal customerEnteredPrice,
            int quantity)
        {
            return new List<string>();
        }

        public virtual IList<string> GetShoppingCartItemAttributeWarnings(Customer customer,
            ShoppingCartType shoppingCartType,
            Product product,
            string selectedAttributes)
        {
            return new List<string>();
        }

        public virtual IList<string> GetShoppingCartItemGiftCardWarnings(ShoppingCartType shoppingCartType,
            Product product, string selectedAttributes)
        {
            return new List<string>();
        }

        public virtual IList<string> GetRequiredProductWarnings(Customer customer,
            ShoppingCartType shoppingCartType, Product product,
            int storeId, bool automaticallyAddRequiredProductsIfEnabled)
        {
            return new List<string>();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetAsync(id);
        }

        public async Task<bool> AddRemoveFavoriteProduct(CustomerFavoriteProduct product)
        {
            product.CustomerId = _currentUser.User.Id;
            if (product.IsFavorite)
            {
                _favoriteProductRepostiry.Add(product);
            }
            else
            {
                _favoriteProductRepostiry.Delete(product);
            }
            await _favoriteProductRepostiry.SaveAsync();
            return true;
        }

        public string CreateTradeConfirmationEmailData(RequestForOrderModel model)
        {

            decimal decGoldTotal = 0;
            decimal decSilverTotal = 0;
            decimal decPlatinumTotal = 0;
            decimal decPalladiumTotal = 0;
            decimal decGoldTotalQty = 0;
            decimal decSilverTotalQty = 0;
            decimal decPlatinumTotalQty = 0;
            decimal decPalladiumTotalQty = 0;
            string strLastCommodity = "";
            decimal decTotal = 0;
            decimal decTotalQty = 0;


            StringBuilder table = new StringBuilder();
            table.Append("<table style=\"border-collapse:collapse; text-align:center;border: 1px solid #999\">");
            table.Append("<thead>");
            table.Append("<tr class=\"cart-header-row\">");
            table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
            table.Append("Commodity");
            table.Append("</th>");

            table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
            table.Append("Product Name");
            table.Append("</th>");

            table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
            table.Append("Quantity");
            table.Append("</th>");

            table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
            table.Append("Oz.");
            table.Append("</th>");

            table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
            table.Append("Unit Price");
            table.Append("</th>");

            table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
            table.Append("Item Total");
            table.Append("</th>");

            table.Append("</tr>");
            table.Append("</thead>");
            table.Append("<tbody>");

            foreach (var item in model.Items)
            {
                if (item.Commodity.ToUpper() == "GOLD")
                { decGoldTotal = decGoldTotal + item.SubTotal; decGoldTotalQty = decGoldTotalQty + item.WeightSubTotal; }
                if (item.Commodity.ToUpper() == "SILVER")
                { decSilverTotal = decSilverTotal + item.SubTotal; decSilverTotalQty = decSilverTotalQty + item.WeightSubTotal; }
                if (item.Commodity.ToUpper() == "PLATINUM")
                { decPlatinumTotal = decPlatinumTotal + item.SubTotal; decPlatinumTotalQty = decPlatinumTotalQty + item.WeightSubTotal; }
                if (item.Commodity.ToUpper() == "PALLADIUM")
                { decPalladiumTotal = decPalladiumTotal + item.SubTotal; decPalladiumTotalQty = decPalladiumTotalQty + item.WeightSubTotal; }
                decTotal = decTotal + item.SubTotal;
                decTotalQty = decTotalQty + item.WeightSubTotal;
                table.Append("<tr class=\"cart-item-row\">");
                table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
                if (strLastCommodity != item.Commodity)
                {
                    strLastCommodity = item.Commodity;
                    table.Append(item.Commodity + "</td>");
                }
                else
                {
                    table.Append("&nbsp;</td>");
                }
                //Product Description
                table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
                table.Append(item.ProductDesc);
                table.Append("</td>");

                //Product Quantity
                table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
                table.Append(item.Quantity);
                table.Append("</td>");

                //Product Weight
                table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
                table.Append(item.WeightSubTotal.ToString("#0.00000"));
                table.Append("</td>");

                //Product Unit Price
                table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
                table.Append(item.UnitPrice.ToString("C"));
                table.Append("</td>");

                //Product SubTotal
                table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
                table.Append(item.SubTotal.ToString("C"));
                table.Append("</td>");

                table.Append("</tr>");

            }
            table.Append("</tbody>");
            table.Append("</table>");

            return table.ToString();
        }

        public IList<ShoppingCartItem> GetUserShoppingCartItems()
        {
            var cart = _currentUser.User.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest)
                .Where(sci => sci.StoreId == 1)
                .ToList();
            return cart;
        }

        public async Task UpdatePortalProducts(Customer user)
        {
            try
            {
                //CustomerAttributes customerAttributes = _genericAttributeManager.GetCustomerAttributes(user.Id).Result;
                var serviceEndPoint = _config.GetSection("ConnectionStrings:WebAPI_URL").Value;
                //_mtsApi = new WindowsServiceEndPoint.MTS_Api(serviceEndPoint, customerAttributes.AmarkTPAPIKey, customerAttributes.AmarkTradingPartnerNumber, user.Email);
                var productInfo = await MtsApi.GetPortalProducts();
                if (productInfo.objProductList[0].sRequestStatus != "Declined")
                {
                    var productsList = GetProductsList(productInfo.objProductList);
                    await _storeCommandRepository.CreateStoreProcedureCommand("[spUpdateAllPortalProducts]")
                        .AddParameter("@PortalProducts", productsList, SqlDbType.Structured, "UT_PortalProduct")
                        .ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private List<SqlDataRecord> GetProductsList(ProductItem[] productsList)
        {
            List<SqlDataRecord> datatable = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaData = new SqlMetaData[11];
            sqlMetaData[0] = new SqlMetaData("Name", SqlDbType.NVarChar);
            sqlMetaData[0] = new SqlMetaData("ShortDescription", SqlDbType.NVarChar);
            sqlMetaData[0] = new SqlMetaData("Weight", SqlDbType.Decimal);
            sqlMetaData[0] = new SqlMetaData("CommodityID", SqlDbType.Int);
            SqlDataRecord row = new SqlDataRecord(sqlMetaData);

            foreach (var product in productsList)
            {
                row = new SqlDataRecord(sqlMetaData);
                row.SetValues(new object[] {product.sProductCode,product.sProductDesc,product.decProductOzconv,product.iCommodityCode});
                datatable.Add(row);
            }
            return datatable;
        }


        public async Task<ProductsQuoteInfo> RequestAmarkOnlineQuote(RequestForOrderModel requestForOrderModel, List<ProductQuoteItem> productQuoteItems)
        {
            var orderType = requestForOrderModel.IsSell ? "B" : "S";
            return await MtsApi.RequestAmarkOnlineQuote(orderType, productQuoteItems, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip == true ? "Drop Ship" : "Default",
                                                                requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, "", requestForOrderModel.TPConfirmation, requestForOrderModel.SpecialInstructions);
        }

        public async Task<bool> AddressValid(string sCountry, string sCity, string sState, string sZip)
        {
            return await MtsApi.AddressValid(sCountry, sCity, sState, sZip);
        }

        public async Task<OpenPositionInfo> GetOpenPositions()
        {
            return await MtsApi.RetrieveOpenPositions();
        }

        public async Task<TradingHistoryInfo> GetTradingHistory(DateTime dtBegin, DateTime dtEnd, int iPageNumber, int iPageSize)
        {
            return await MtsApi.RetrieveTradingHistory(dtBegin,dtEnd,iPageNumber,iPageSize);
        }

        public async Task<ShipToAddressItem> GetOrderShippingItem(int iRequestID, int iOrderHdrID)
        {
            return await MtsApi.GetOrderShippingItem(iRequestID,iOrderHdrID);
        }

        public async Task<CustomerCommodityLimitsInfo> GetAMarkWebTradingLimits(string sOrderType)
        {
            return await MtsApi.GetAMarkWebTradingLimits("S");
        }

        public async Task<RequestOnlineTradeInfo> RequestAmarkOnlineTrade(string sOnline_Request_Type, string sQuoteKey, bool bHFIFlag, string sShippingType, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber, string sTP_Confirm_No)
        {
            return await MtsApi.RequestAmarkOnlineTrade(sOnline_Request_Type, sQuoteKey, bHFIFlag, sShippingType, sShippingName1, sShippingName2, sShippingAddress1, sShippingAddress2, sShippingCity, sShippingState, sShippingZipCode, sShippingCountry, sShippingPhoneNumber,sTP_Confirm_No);
        }

        public MTS_Api MtsApi
        {
            get
            {
                if (_mtsApi == null)
                {
                    var serviceEndPoint = _config.GetSection("ConnectionStrings:WebAPI_URL").Value;
                    _mtsApi = new WindowsServiceEndPoint.MTS_Api(serviceEndPoint, _customerAttributes.AmarkTPAPIKey, _customerAttributes.AmarkTradingPartnerNumber, _currentUser.User.Email);
                    return _mtsApi;
                }
                else
                {
                    return _mtsApi;
                }
            }
        }
    }
}
