using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MTSWebApi;
using System.Linq;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Services.Interfaces;
using TradingPortal.Business.Extensions;
using TradingPortal.Core.Enums;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using System;
using System.Collections.Generic;
using WindowsServiceEndPoint;
using TradingPortal.Core.Constants;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Core.Domain.Orders;
using TradingPortal.Core.Domain.Common;
using static TradingPortal.Core.ViewModels.RequestForOrderModel;

namespace TradingPortal.Business
{
    public class ShoppingCartManager : IShoppingCartManager
    {
        private readonly IConfiguration _config;
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerManager _customerManager;
        private readonly ICurrentUser _currentUser;
        //private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly IHttpContextAccessor _context;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ISettingsService _settingsService;
        private readonly ApplicationDbContext _dbContext;
        private ShoppingCartSettings _shoppingCartSettings;
        private readonly CustomSettings _customSettings;
        private readonly OrderSettings _orderSettings;
        private readonly IRepository<Vendor> _vendorsRepository;

        private MTS_Api _mtsApi = null;


        public ShoppingCartManager(
            IConfiguration config,
            IGenericAttributeManager genericAttributeManager,
            ICurrentUser currentUser,
            IHttpContextAccessor context,
            IRepository<ShoppingCartItem> shoppingCartItemRepository,
            ICustomerManager customerManager,
            IProductRepository productRepository,
            //ICheckoutAttributeParser checkoutAttributeParser,
            IProductAttributeService productAttributeService,
            IProductAttributeParser productAttributeParser,
            ApplicationDbContext dbContext,
            ISettingsService settingsService,
            ShoppingCartSettings shoppingCartSettings,
            OrderSettings orderSettings,
            CustomSettings customSettings,
            IRepository<Vendor> vendorsRepository)
        {
            _config = config;
            _genericAttributeManager = genericAttributeManager;
            _currentUser = currentUser;
            _context = context;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _customerManager = customerManager;
            _productRepository = productRepository;
            //_checkoutAttributeParser = checkoutAttributeParser;
            _productAttributeService = productAttributeService;
            _productAttributeParser = productAttributeParser;
            _dbContext = dbContext;
            _settingsService = settingsService;
            _shoppingCartSettings = shoppingCartSettings;
            _customSettings = customSettings;
            _orderSettings = orderSettings;
            _vendorsRepository = vendorsRepository;
            //_shoppingCartSettings = _settingsManager.LoadSetting<ShoppingCartSettings>(1);
            //_customSettings = _settingsManager.LoadSetting<CustomSettings>(1);
            //_orderSettings = _settingsManager.LoadSetting<OrderSettings>(1);
        }

      
        public async Task<RequestForOrderModel> RequestForOrder(bool isSell)
        {
            //bool isSell = isSell;
            bool isEditable = true;
            bool isHfi = false;
            bool isDropShip = false;
            string spInstructions = "";
            string tpConfirmation = "";

            //save seleted order mode for now
            Session.SetObject<bool>("IsSellMode", isSell);
            //if we got here - termshave been accepted, save this so we do not ask user again
            Session.SetObject<bool>("TermsAccepted", true);

            RequestForOrderModel model = new RequestForOrderModel();
            try
            {
                await DeleteShoppingCartItems();
               
                var cart = _currentUser.User.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest)
                    .Where(sci => sci.StoreId == 1)
                    .ToList();

                return await PrepareRequestForOrderModel(model, cart, isSell, isEditable, isHfi, isDropShip, spInstructions, tpConfirmation);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

       

        protected async Task<RequestForOrderModel> PrepareRequestForOrderModel(RequestForOrderModel model,
            IList<ShoppingCartItem> cart, bool isSellMode = false, bool isEditable = false, bool IsHfi = false, bool IsDropShip = false, string spInstructions = "", string tpConfirmation = "",
            string name1 = "", string name2 = "", string address1 = "", string address2 = "", string city = "", string state = "", string zip = "", string country = "",
            Dictionary<int, IList<string>> warnings = null)
        {
            model.IsSellMode = isSellMode;
            model.IsEditable = isEditable;
            //model.PriceExpireInterval = Convert.ToInt32(_customSettings.PriceExpiredSeconds);

            decimal decGoldTotal = 0;
            decimal decSilverTotal = 0;
            decimal decPlatinumTotal = 0;
            decimal decPalladiumTotal = 0;
            //Go through each cart items and find out total ounces for Gold, Silver, Platinum and Palladium
            foreach (ShoppingCartItem sci in cart)
            {
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Gold")
                {
                    decGoldTotal = decGoldTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Silver")
                {
                    decSilverTotal = decSilverTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Platinum")
                {
                    decPlatinumTotal = decPlatinumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Palladium")
                {
                    decPalladiumTotal = decPalladiumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
            }

            #region Discounts Info
            ProductsDiscountsInfo prodDiscounts;
            if (_context.HttpContext.Session.GetObject<decimal>("decGoldDiscount") != 0m)
            {
                model.IsDiscountPricing = false;
                if (Session.GetObject<decimal>("decGoldDiscount") > 0m)
                {
                    if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decGoldDiscount")))
                    {
                        model.IsDiscountPricing = true;
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decSilverDiscount") > 0m)
                    {
                        if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decSilverDiscount")))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPlatinumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decPlatinumDiscount")))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPalladiumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decPalladiumDiscount")))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }

            }
            else
            {
                //var mtsApi = GetMtsApiInstance().Result;
                prodDiscounts = await MtsApi.GetAMarkWebProductsDiscounts();
                Session.SetObject<decimal>("decGoldDiscount", prodDiscounts.decGoldDiscount);
                Session.SetObject<decimal>("decSilverDiscount", prodDiscounts.decSilverDiscount);
                Session.SetObject<decimal>("decPlatinumDiscount", prodDiscounts.decPlatinumDiscount);
                Session.SetObject<decimal>("decPalladiumDiscount", prodDiscounts.decPalladiumDiscount);
                model.IsDiscountPricing = false;

                if (Session.GetObject<decimal>("decGoldDiscount") > 0m)
                {
                    if (decGoldTotal >= Session.GetObject<decimal>("decGoldDiscount"))
                    {
                        model.IsDiscountPricing = true;
                    }
                }
                if(model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decSilverDiscount") > 0m)
                    {
                        if (decGoldTotal >= Session.GetObject<decimal>("decSilverDiscount"))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPlatinumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Session.GetObject<decimal>("decPlatinumDiscount"))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPalladiumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Session.GetObject<decimal>("decPalladiumDiscount"))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }

            }
            await IsCommodityLimitViolated(model, cart, isSellMode, true);

            //get Amark web products pricing info
            ProductsPricingInfo prodPricing;
            if (model.IsSellMode)
                //model.IsHFI = false;
                prodPricing = await MtsApi.GetAMarkWebProductsPricingToSell(decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);
            else
                //model.IsHFI = true;
                prodPricing = await MtsApi.GetAMarkWebProductsPricingToBuy(decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);
            if (prodPricing != null)
            {
                if (model.IsEditable)
                {
                    //IY - In V1 fetched from database
                    //ProductInfo products = await MtsApi.GetPortalProducts();
                    //IList<Product> products = _productRepository.GetAll().OrderBy(p => new { p.CommodityID, p.DisplayOrder, p.ShortDescription })
                    //                                           .Where(p => p.Published && !p.Deleted)
                    //                                           .Select(p => p).ToList();
                    IList<ProductViewModel> products = await _productRepository.GetAllActiveProducts(_currentUser.User.Id);
                                                               
                    //var productsList = products.objProductList.OrderBy(p => p.iCommodityCode).ThenBy(p => p.sProductDesc);
                    //prodPricing = await MtsApi.GetAMarkWebProductsPricingToBuy(decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);
                    if (prodPricing != null)
                    {
                        var query = (from product in products
                                     join pp in prodPricing.objWebProductsPricingList on product.Name equals pp.sProductCode
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
                                IsFavorite = item.IsFavorite
                            };
                            model.Items.Add(reqForOrderItemModel);

                        }
                        model.IsHFI = false;
                        model.IsDropShip = false;
                        model.SpecialInstructions = "";
                        model.TPConfirmation = "";
                        model.Name1 = "";
                        model.Name2 = "";
                        model.Address1 = "";
                        model.Address2 = "";
                        model.City = "";
                        model.State = "";
                        model.Zip = "";
                        model.Country = "";
                    }
                }
                else
                {
                    //IList<Product> products = _productRepository.GetAll().OrderBy(p => new { p.CommodityID, p.DisplayOrder, p.ShortDescription })
                    //                                            .Where(p => p.Published && !p.Deleted)
                    //                                            .Select(p => p).ToList();
                    IList<ProductViewModel> products = await _productRepository.GetAllActiveProducts(_currentUser.User.Id);

                    var query = (from product in products
                                 join pp in prodPricing.objWebProductsPricingList on product.Name equals pp.sProductCode
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
                            IsFavorite = item.IsFavorite
                        };
                        model.Items.Add(reqForOrderItemModel);
                    }

                    model.IsHFI = IsHfi;
                    model.IsDropShip = IsDropShip;
                    model.SpecialInstructions = spInstructions;
                    model.TPConfirmation = tpConfirmation;
                    model.Name1 = name1;
                    model.Name2 = name2;
                    model.Address1 = address1;
                    model.Address2 = address2;
                    model.City = city;
                    model.State = state;
                    model.Zip = zip;
                    model.Country = country;
                }
                model.MultipleTransactionGroupsPresent = (prodPricing.transactionGroupCodesCount > 1) ? true : false;
                Session.SetObject<bool>("MultipleTransactionGroupsPresent", model.MultipleTransactionGroupsPresent);
            }

            if (warnings != null)
            {
                //update current warnings
                foreach (var kvp in warnings)
                {
                    //kvp = <cart item identifier, warnings>
                    var sciId = kvp.Key;
                    var warns = kvp.Value;
                    //find model
                    var sciModel = model.Items.FirstOrDefault(x => x.ProductId == sciId);
                    if (sciModel != null)
                        foreach (var w in warns)
                            if (!sciModel.Warnings.Contains(w))
                                sciModel.Warnings.Add(w);
                }
            }
            return model;
        }


        public async Task<RequestForOrderModel> ReviewOrderRequest(RequestForOrderModel requestForOrderModel)
        {
            //bool isSell = false;
            bool isEditable = false;
            var storeId = 1;
            var model = new RequestForOrderModel();
            try
            {
                var cart = _currentUser.User.ShoppingCartItems
                        .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest
                                  && sci.StoreId == storeId)
                                  .ToList();

                //Create only cart items for products that have qty
                //current warnings <cart item identifier, warnings>
                var innerWarnings = new Dictionary<int, IList<string>>();
                var cartType = ShoppingCartType.AMarkOrderRequest;

                //Add /Remove Fav/UnFav Products
                if (!requestForOrderModel.IsRequote)
                {
                    if (requestForOrderModel.ModifiedItems != null && requestForOrderModel.ModifiedItems.Count > 0)
                    {
                        var unfavoriteItems = requestForOrderModel.ModifiedItems.Where(mi => mi.IsFavorite != true)
                            .Select(mi => new CustomerFavoriteProduct { CustomerId = _currentUser.User.Id, ProductName = mi.ProductName }).ToList();
                        if (unfavoriteItems.Count > 0)
                        {
                            await _productRepository.DeleteCustomerFavoriteProducts(unfavoriteItems);
                        }
                        var favoriteItems = requestForOrderModel.ModifiedItems.Where(mi => mi.IsFavorite == true)
                            .Select(mi => new CustomerFavoriteProduct { CustomerId = _currentUser.User.Id, ProductName = mi.ProductName }).ToList();
                        if (favoriteItems.Count > 0)
                        {
                            await _productRepository.AddCustomerFavoriteProducts(favoriteItems);
                        }
                    }
                }

                
                if (!requestForOrderModel.IsRequote) //If requote then dont add products to shopping cart sa they have already been added
                {
                    foreach (var item in requestForOrderModel.Items)
                    {
                        if (item.Quantity > 0)
                        {
                            var product = _productRepository.GetById(item.ProductId);
                            if (product != null)
                            {
                                var currSciWarnings = await AddToCart(_currentUser.User, product, cartType, storeId, string.Empty,
                                    decimal.Zero, Convert.ToInt32(item.Quantity), false, item.Quantity);
                                if (currSciWarnings.Count > 0)
                                {
                                    innerWarnings.Add(item.ProductId, currSciWarnings);
                                }
                            }
                        }
                    }
                }

                //in case there are any products in the Cart that we changed qty back to 0 - update Cart (remove 0 items)
                foreach (var sci in cart)
                {
                    var product = requestForOrderModel.Items.FirstOrDefault(i => i.ProductId == sci.ProductId);
                    if (product != null)
                    {
                        var currSciWarnings = UpdateShoppingCartItem(_currentUser.User,
                                        sci.Id, Convert.ToInt32(product.Quantity), true, product.Quantity);
                        if (currSciWarnings.Count > 0)
                        {
                            innerWarnings.Add(sci.ProductId, currSciWarnings);
                        }
                    }
                }

                //updated cart
                cart = _currentUser.User.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest)
                    .Where(sci => sci.StoreId == storeId)
                    .ToList();

                

                //prevent 2 orders being placed within an X seconds time frame
                if (!IsMinimumOrderPlacementIntervalValid())
                    throw new Exception("Voilation of Min Order Placement Interval!");

                Session.SetObject<DateTime>("LastOrderPlacedDate", DateTime.UtcNow);
                //Check the Commodity Limits
                if (await IsCommodityLimitViolated(model, cart, requestForOrderModel.IsSell))
                {
                    //    throw new Exception("Commodity limits violated");
                    var warList = new List<string>();
                    warList.Add("Commodity limits violated");
                    innerWarnings.Add(0, warList);
                }

                List<ProductQuoteItem> productQuoteItems = new List<ProductQuoteItem>();

                foreach (ShoppingCartItem sci in cart)
                {
                    ProductQuoteItem li = new ProductQuoteItem();
                    li.sProductCode = sci.Product.Name;
                    li.decProductQuantity = sci.DecimalQuantity;
                    productQuoteItems.Add(li);
                }

                if (!requestForOrderModel.IsSell && !requestForOrderModel.IsRequote && requestForOrderModel.Country == "US")
                {
                    requestForOrderModel.State = requestForOrderModel.State.Substring(0, 2);
                }

                if (requestForOrderModel.IsSell || requestForOrderModel.IsRequote  || await MtsApi.AddressValid(requestForOrderModel.Country, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip))
                {
                    var orderType = requestForOrderModel.IsSell ? "B" : "S";
                    ProductsQuoteInfo pinfo = await MtsApi.RequestAmarkOnlineQuote(orderType, productQuoteItems, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip == true ? "Drop Ship" : "Default",
                                                                        requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, "", requestForOrderModel.TPConfirmation, requestForOrderModel.SpecialInstructions);
                    if (pinfo.sRequestStatus == "Success")
                    {
                        model.QuoteKey = pinfo.sQuoteKey;
                        model.SmallOrderCharge = pinfo.decSmallOrderCharge.ToString();
                    }
                    else
                    {
                        //throw new Exception(pinfo.sStatusMessage + ". " + pinfo.sErrorDescription);
                        throw new Exception(pinfo.sErrorDescription);
                    }
                    await PrepareReviewForOrderModel(model, cart, requestForOrderModel.IsSell, isEditable, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip, requestForOrderModel.SpecialInstructions, requestForOrderModel.TPConfirmation, requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, innerWarnings, pinfo);
                }
                else
                {
                    isEditable = true;
                    //ViewData["Message"] = "1";
                    await PrepareEditRequestForOrderModel(model, cart, requestForOrderModel.IsSell, isEditable, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip, requestForOrderModel.SpecialInstructions, requestForOrderModel.TPConfirmation, requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, true);
                }
            }
            catch(Exception ex)
            {
                model.Warnings.Add(ex.Message);
                throw ex;
            }
            var keys = Session.Keys;
            Session.SetObject<RequestForOrderModel>("OrderRequestInfo",model);
            var request = _context.HttpContext.Session.GetString("OrderRequestInfo");
            var info = Session.GetObject<RequestForOrderModel>("OrderRequestInfo");
            keys = Session.Keys;
            var test = _context.HttpContext.Session.GetObject<RequestForOrderModel>("OrderRequestInfo");
            return model;
        }

        public async Task<RequestForOrderModel> MarkUnMarkProductsAsFavorite(RequestForOrderModel requestForOrderModel)
        {
            if (requestForOrderModel.ModifiedItems != null && requestForOrderModel.ModifiedItems.Count > 0)
            {
                var unfavoriteItems = requestForOrderModel.ModifiedItems.Where(mi => mi.IsFavorite != true)
                    .Select(mi => new CustomerFavoriteProduct { CustomerId = _currentUser.User.Id, ProductName = mi.ProductName }).ToList();
                if (unfavoriteItems.Count > 0)
                {
                    await _productRepository.DeleteCustomerFavoriteProducts(unfavoriteItems);
                }
                var favoriteItems = requestForOrderModel.ModifiedItems.Where(mi => mi.IsFavorite == true)
                    .Select(mi => new CustomerFavoriteProduct { CustomerId = _currentUser.User.Id, ProductName = mi.ProductName }).ToList();
                if (favoriteItems.Count > 0)
                {
                    await _productRepository.AddCustomerFavoriteProducts(favoriteItems);
                }

                //requestForOrderModel.Items = requestForOrderModel.Items.OrderBy(p => p.ProductDesc).ToList();
                requestForOrderModel.Items = requestForOrderModel.Items.OrderBy(p => p.CommodityID)
                                                    .ThenByDescending(p => p.IsFavorite)
                                                    .ThenBy(p => p.ProductDesc).ToList();
                                                            
                                                            
                //return requestForOrderModel;
            }
            return requestForOrderModel;
        }

        public async Task<bool> ConfirmOrderRequest(bool isSell)
        {
            var model = new RequestForOrderModel();
            try
            {
                var keys = Session.Keys;
                model = Session.GetObject<RequestForOrderModel>("OrderRequestInfo");// as RequestForOrderModel;
                
                if (model == null)
                {
                    throw new Exception("Order details data is not available.");
                }
                else
                {
                    if (model.Items.Count == 0)
                        throw new Exception("Order details data is empty.");
                }

                Session.SetObject<DateTime>("LastOrderPlacedDate",DateTime.UtcNow);
                var requestType = isSell ? "B" : "S";
                RequestOnlineTradeInfo rinfo = await MtsApi.RequestAmarkOnlineTrade(requestType, model.QuoteKey, model.IsHFI, model.IsDropShip == true ? "Drop Ship" : "Default",
                                                                    model.Name1, model.Name2, model.Address1, model.Address2, model.City, model.State, model.Zip, model.Country, "", model.TPConfirmation);

                if (rinfo.sStatusMessage == "Trade Confirmed")
                {
                    model.QuoteKey = rinfo.sQuoteKey;
                    model.sTicketNumber = rinfo.sTicketNumber;
                    model.IsHedgedOrder = rinfo.IsHedgedOrder;

                    //_workflowMessageService.SendOrderPlacedAmarkNotification();

                    //return RedirectToRoute("RequestForOrderBuyConfirmation");
                    //RequestForOrderConfirmation();
                    
                }
                else //if(rinfo.sStatusMessage == "Trade Already Confirmed" || rinfo.sStatusMessage == "Time Expired")
                {
                    throw new Exception(rinfo.sStatusMessage); //(_localizationService.GetResource("Checkout.MinOrderPlacementInterval"));
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            Session.SetObject<RequestForOrderModel>("OrderRequestInfo", model);
            return true;
        }

        RequestForOrderModel RequestForOrderConfirmation()
        {
            var model = new RequestForOrderModel();
            //model.IsTradeConfirmed = true;
            try
            {
                model = Session.GetObject<RequestForOrderModel>("OrderRequestInfo");
                if (model == null)
                {
                    throw new Exception("Order details data is not available.");
                }
                else
                {
                    if (model.Items.Count == 0)
                        throw new Exception("Order details data is empty.");
                }

                //var cart = _currentUser.User.ShoppingCartItems
                //    .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest)
                //    .Where(sci => sci.StoreId == 1)
                //    .ToList();
                //// empty the cart
                //foreach (var sci in cart)
                //{
                //    DeleteShoppingCartItem(sci);
                //}

            }
            catch (Exception exc)
            {
                //_logger.Warning(exc.Message, exc);
                model.Warnings.Add(exc.Message);
            }

            return model;
        }

              

        protected async Task<RequestForOrderModel> PrepareEditRequestForOrderModel(RequestForOrderModel model,
            IList<ShoppingCartItem> cart, bool isSellMode = false, bool isEditable = true, bool IsHfi = false, bool IsDropShip = false, string spInstructions = "", string tpConfirmation = "",
            string name1 = "", string name2 = "", string address1 = "", string address2 = "", string city = "", string state = "", string zip = "", string country = "", bool isZero = false)
        {
            model.IsSellMode = isSellMode;
            model.IsEditable = isEditable;
            model.PriceExpireInterval = Convert.ToInt32(_customSettings.PriceExpiredSeconds);

            //get AMark trading partner number
            //var customer = _customerManager.UpdateCustomer//.GetCustomerById(_workContext.CurrentCustomer.Id);
            var customer = _currentUser.User;
            string sTradingPartner = "";
            //sTradingPartner = customer.GetAttribute<string>(SystemCustomerAttributeNames.AmarkTradingPartnerNumber);
            sTradingPartner = _genericAttributeManager.GetAttribute<string>(customer, SystemCustomerAttributeNames.AmarkTradingPartnerNumber, 1);
            decimal decGoldTotal = 0;
            decimal decSilverTotal = 0;
            decimal decPlatinumTotal = 0;
            decimal decPalladiumTotal = 0;

            if (isZero)
            {
            }
            else
            {
                //Go through each cart items and find out total ounces for Gold, Silver, Platinum and Palladium
                foreach (ShoppingCartItem sci in cart)
                {
                    if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Gold")
                    {
                        decGoldTotal = decGoldTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                    }
                    if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Silver")
                    {
                        decSilverTotal = decSilverTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                    }
                    if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Platinum")
                    {
                        decPlatinumTotal = decPlatinumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                    }
                    if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Palladium")
                    {
                        decPalladiumTotal = decPalladiumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                    }
                }

            }
            //get Amark web products pricing info
            ProductsPricingInfo prodPricing;
            if (model.IsSellMode)
                //model.IsHFI = false;
                prodPricing = await MtsApi.GetAMarkWebProductsPricingToSell(decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);
            else
                //model.IsHFI = true;
                prodPricing = await MtsApi.GetAMarkWebProductsPricingToBuy(decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);

            if (prodPricing != null)
            {

                IList<ProductViewModel> products = await _productRepository.GetAllActiveProducts(_currentUser.User.Id);
                var query = (from product in products
                             join pp in prodPricing.objWebProductsPricingList on product.Name equals pp.sProductCode
                             join sc in cart on product.Id equals sc.ProductId into scp
                             from s in scp.DefaultIfEmpty()
                             select new
                             {
                                 pp.sCommodityDesc,
                                 product.Sku,
                                 product.Id,
                                 product.Name,
                                 product.ShortDescription,
                                 product.Weight,
                                 product.CommodityID,
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
                        CommodityID = item.CommodityID,
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
                    };
                    model.Items.Add(reqForOrderItemModel);
                    // y.chan - we do not want to add a product to a shopping cart unless there is a quantity entered for it because this is very expansive operation
                    ////also add it to the shopping cart
                    //var cartType = ShoppingCartType.AMarkOrderRequest;
                    //var product = _productService.GetProductById(item.Id);
                    //if (product != null)
                    //{
                    //    IList<string> addToCartWarnings = _shoppingCartService.AddToCart(_workContext.CurrentCustomer,
                    //           product, cartType, _storeContext.CurrentStore.Id,
                    //           string.Empty, decimal.Zero, 0, false, decimal.Zero);

                    //    //if (addToCartWarnings.Count > 0)
                    //    //{
                    //    //    foreach (string warn in addToCartWarnings)
                    //    //    {
                    //    //        model.Warnings.Add(warn);
                    //    //    }
                    //    //}
                    //}
                }
            }
            model.IsHFI = IsHfi;
            model.IsDropShip = IsDropShip;
            model.SpecialInstructions = spInstructions;
            model.TPConfirmation = tpConfirmation;
            model.Name1 = name1;
            model.Name2 = name2;
            model.Address1 = address1;
            model.Address2 = address2;
            model.City = city;
            model.State = state;
            model.Zip = zip;
            model.Country = country;

            model.MultipleTransactionGroupsPresent = (prodPricing.transactionGroupCodesCount > 1) ? true : false;
            //model.DecGoldTotal = model.Items.Where(item => item.Commodity.ToUpper() == "GOLD").Select(item => item.SubTotal).DefaultIfEmpty(0).Sum();//.ToString("#0.00000")
            //model.DecGoldTotalQuantity = model.Items.Where(item => item.Commodity.ToUpper() == "GOLD").Sum(item => item.WeightSubTotal);
            //Code from Razor View
            foreach (var item in model.Items)
            {
                if(item.Commodity.ToUpper() == "GOLD")
                {
                    model.DecGoldTotal = model.DecGoldTotal + item.SubTotal;
                    model.DecGoldTotalQuantity = model.DecGoldTotalQuantity + item.WeightSubTotal;
                }
                if (item.Commodity.ToUpper() == "SILVER")
                {
                    model.DecSilverTotal = model.DecSilverTotal + item.SubTotal;
                    model.DecSilverTotalQuantity = model.DecSilverTotalQuantity + item.WeightSubTotal;
                }
                if (item.Commodity.ToUpper() == "PLATINUM")
                {
                    model.DecPlatinumTotal = model.DecPlatinumTotal + item.SubTotal;
                    model.DecPlatinumTotalQuantity = model.DecPlatinumTotalQuantity + item.WeightSubTotal;
                }
                if (item.Commodity.ToUpper() == "PALLADIUM")
                {
                    model.DecPalladiumTotal = model.DecPalladiumTotal + item.SubTotal;
                    model.DecPalladiumTotalQuantity = model.DecPalladiumTotalQuantity + item.WeightSubTotal;
                }
            }


            //Session["MultipleTransactionGroupsPresent"] = model.MultipleTransactionGroupsPresent;
            Session.SetObject<bool>("MultipleTransactionGroupsPresent",model.MultipleTransactionGroupsPresent);
            return model;
        }

        protected async Task<RequestForOrderModel> PrepareReviewForOrderModel(RequestForOrderModel model,
            IList<ShoppingCartItem> cart, bool isSellMode = false, bool isEditable = false, bool IsHfi = false, bool IsDropShip = false, string spInstructions = "", string tpConfirmation = "",
            string name1 = "", string name2 = "", string address1 = "", string address2 = "", string city = "", string state = "", string zip = "", string country = "",
            Dictionary<int, IList<string>> warnings = null, ProductsQuoteInfo pinfo = null)
        {
            model.IsSellMode = isSellMode;
            model.IsEditable = isEditable;
            model.PriceExpireInterval = Convert.ToInt32(_customSettings.PriceExpiredSeconds);

            // see if there are any warnings, and if there are, turn on edit mode so we do not go to Review stage
            if (warnings != null)
            {
                if (warnings.Count > 0)
                    model.IsEditable = true;
            }

            decimal decGoldTotal = 0;
            decimal decSilverTotal = 0;
            decimal decPlatinumTotal = 0;
            decimal decPalladiumTotal = 0;
            //Go through each cart items and find out total ounces for Gold, Silver, Platinum and Palladium
            foreach (ShoppingCartItem sci in cart)
            {
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Gold")
                {
                    decGoldTotal = decGoldTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Silver")
                {
                    decSilverTotal = decSilverTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Platinum")
                {
                    decPlatinumTotal = decPlatinumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Palladium")
                {
                    decPalladiumTotal = decPalladiumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
            }

            #region Discounts Info
            ProductsDiscountsInfo prodDiscounts;
            if (Session.GetObject<decimal>("decGoldDiscount") != 0m)
            {
                model.IsDiscountPricing = false;
                if (Session.GetObject<decimal>("decGoldDiscount") > 0m)
                {
                    if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decGoldDiscount")))
                    {
                        model.IsDiscountPricing = true;
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decSilverDiscount") > 0m)
                    {
                        if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decSilverDiscount")))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPlatinumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decPlatinumDiscount")))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPalladiumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decPalladiumDiscount")))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }

            }
            else
            {
                prodDiscounts = await MtsApi.GetAMarkWebProductsDiscounts();
                Session.SetObject<decimal>("decGoldDiscount", prodDiscounts.decGoldDiscount);
                Session.SetObject<decimal>("decSilverDiscount", prodDiscounts.decSilverDiscount);
                Session.SetObject<decimal>("decPlatinumDiscount", prodDiscounts.decPlatinumDiscount);
                Session.SetObject<decimal>("decPalladiumDiscount", prodDiscounts.decPalladiumDiscount);
                model.IsDiscountPricing = false;

                if (Session.GetObject<decimal>("decGoldDiscount") > 0m)
                {
                    if (decGoldTotal >= Session.GetObject<decimal>("decGoldDiscount"))
                    {
                        model.IsDiscountPricing = true;
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decSilverDiscount") > 0m)
                    {
                        if (decGoldTotal >= Session.GetObject<decimal>("decSilverDiscount"))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPlatinumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Session.GetObject<decimal>("decPlatinumDiscount"))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
                if (model.IsDiscountPricing == false)
                {
                    if (Session.GetObject<decimal>("decPalladiumDiscount") > 0m)
                    {
                        if (decGoldTotal >= Session.GetObject<decimal>("decPalladiumDiscount"))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                }
            }
            #endregion

            #region Commodity Limits
            await IsCommodityLimitViolated(model, cart, isSellMode, true);
            #endregion

            ////get Amark web products pricing info
            //ProductsPricingInfo prodPricing;
            //if (model.IsSellMode)
            //    //model.IsHFI = false;
            //    prodPricing = _requestForOrderService.GetAMarkWebProductsPricingToSell(decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);
            //else
            //    //model.IsHFI = true;
            //    prodPricing = _requestForOrderService.GetAMarkWebProductsPricingToBuy(decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);

            if (pinfo != null)
            //if (prodPricing != null)
            {
                if (model.IsEditable)
                {
                    // let get all active products from NOP to merge with AMark pricing data
                    //IList<Product> products = _productRepository.GetAll().OrderBy(p => new { p.CommodityID, p.DisplayOrder, p.ShortDescription })
                    //                                           .Where(p => p.Published && !p.Deleted)
                    //                                           .Select(p => p).ToList();
                    IList<ProductViewModel> products = await _productRepository.GetAllActiveProducts(_currentUser.User.Id);
                    var query = (from nopProduct in products
                                 join pp in pinfo.objWebProductsPricingList on nopProduct.Name equals pp.sProductCode
                                 select new
                                 {
                                     pp.sCommodityDesc,
                                     nopProduct.Sku,
                                     nopProduct.Id,
                                     nopProduct.Name,
                                     nopProduct.ShortDescription,
                                     nopProduct.Weight,
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
                            UnitPrice = item.decUnitPrice,
                            Quantity = 0,
                            SubTotal = 0,
                            TierPrices = item.sTierPrices,
                        };
                        model.Items.Add(reqForOrderItemModel);

                        // y.chan - we do not want to add a product to a shopping cart unless there is a quantity entered for it because this is very expansive operation
                        ////also add it to the shopping cart
                        //var cartType = ShoppingCartType.AMarkOrderRequest;

                        //var product = _productService.GetProductById(item.Id);
                        //if (product != null)
                        //{
                        //    IList<string> addToCartWarnings = _shoppingCartService.AddToCart(_workContext.CurrentCustomer,
                        //           product, cartType, _storeContext.CurrentStore.Id,
                        //           string.Empty, decimal.Zero, 0, false, decimal.Zero);

                        //    if (addToCartWarnings.Count > 0)
                        //    {
                        //        foreach (string warn in addToCartWarnings)
                        //        {
                        //            model.Warnings.Add(warn);
                        //        }
                        //    }
                        //}
                    }
                    model.IsHFI = false;
                    model.IsDropShip = false;
                    model.SpecialInstructions = "";
                    model.TPConfirmation = "";
                    model.Name1 = "";
                    model.Name2 = "";
                    model.Address1 = "";
                    model.Address2 = "";
                    model.City = "";
                    model.State = "";
                    model.Zip = "";
                    model.Country = "";
                }
                else
                {
                    // let get all active products from NOP to merge with AMark pricing data and also merge it with 
                    // shopping cart (quantities were entered for those products)
                    IList<ProductViewModel> products = await _productRepository.GetAllActiveProducts(_currentUser.User.Id);
                    var query = (from nopProduct in products
                                 join pp in pinfo.objWebProductsPricingList on nopProduct.Name equals pp.sProductCode
                                 //join pp in prodPricing.objWebProductsPricingList on nopProduct.Name equals pp.sProductCode
                                 join sc in cart on nopProduct.Id equals sc.ProductId
                                 where sc.DecimalQuantity != 0
                                 select new
                                 {
                                     pp.sCommodityDesc,
                                     nopProduct.Sku,
                                     nopProduct.Id,
                                     nopProduct.Name,
                                     nopProduct.ShortDescription,
                                     nopProduct.Weight,
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
                        };
                        model.Items.Add(reqForOrderItemModel);
                    }

                    model.IsHFI = IsHfi;
                    model.IsDropShip = IsDropShip;
                    model.SpecialInstructions = spInstructions;
                    model.TPConfirmation = tpConfirmation;
                    model.Name1 = name1;
                    model.Name2 = name2;
                    model.Address1 = address1;
                    model.Address2 = address2;
                    model.City = city;
                    model.State = state;
                    model.Zip = zip;
                    model.Country = country;
                }

            }
            foreach (var item in model.Items)
            {
                if (item.Commodity.ToUpper() == "GOLD")
                {
                    model.DecGoldTotal = model.DecGoldTotal + item.SubTotal;
                    model.DecGoldTotalQuantity = model.DecGoldTotalQuantity + item.WeightSubTotal;
                }
                else if (item.Commodity.ToUpper() == "SILVER")
                {
                    model.DecSilverTotal = model.DecSilverTotal + item.SubTotal;
                    model.DecSilverTotalQuantity = model.DecSilverTotalQuantity + item.WeightSubTotal;
                }
                else if (item.Commodity.ToUpper() == "PLATINUM")
                {
                    model.DecPlatinumTotal = model.DecPlatinumTotal + item.SubTotal;
                    model.DecPlatinumTotalQuantity = model.DecPlatinumTotalQuantity + item.WeightSubTotal;
                }
                else if (item.Commodity.ToUpper() == "PALLADIUM")
                {
                    model.DecPalladiumTotal = model.DecPalladiumTotal + item.SubTotal;
                    model.DecPalladiumTotalQuantity = model.DecPalladiumTotalQuantity + item.WeightSubTotal;
                }

                model.DecTotal = model.DecTotal + item.SubTotal;
                model.DecTotalQuantity = model.DecTotalQuantity + item.WeightSubTotal;

            }

            if (warnings != null)
            {
                //update current warnings
                foreach (var kvp in warnings)
                {
                    //kvp = <cart item identifier, warnings>
                    var sciId = kvp.Key;
                    var warns = kvp.Value;
                    //find model
                    var sciModel = model.Items.FirstOrDefault(x => x.ProductId == sciId);
                    if (sciModel != null)
                        foreach (var w in warns)
                            if (!sciModel.Warnings.Contains(w))
                                sciModel.Warnings.Add(w);
                }
            }
            return model;
        }


        public virtual async Task<IList<string>> AddToCart(Customer customer, Product product,
            ShoppingCartType shoppingCartType, int storeId, string selectedAttributes,
            decimal customerEnteredPrice, int quantity, bool automaticallyAddRequiredProductsIfEnabled, decimal decimalQuantity)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();
            //if (shoppingCartType == ShoppingCartType.ShoppingCart && !_permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart, customer))
            //{
            //    warnings.Add("Shopping cart is disabled");
            //    return warnings;
            //}
            //if (shoppingCartType == ShoppingCartType.Wishlist && !_permissionService.Authorize(StandardPermissionProvider.EnableWishlist, customer))
            //{
            //    warnings.Add("Wishlist is disabled");
            //    return warnings;
            //}

            //if (quantity <= 0)
            //{
            //    allow zero qty items to be added to shopping cart for Amark Oreder request
            //    if (shoppingCartType != ShoppingCartType.AMarkOrderRequest)
            //        {
            //            warnings.Add(_localizationService.GetResource("ShoppingCart.QuantityShouldPositive"));
            //            return warnings;
            //        }
            //}

            await _customerManager.ResetCheckoutData(customer,1);
            
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
                    //var r = _dbContext.IsDisposed();
                    _customerManager.UpdateCustomer(customer);
                    //_dbContext.IsDisposed();
                    //event notification
                    //_eventPublisher.EntityUpdated(shoppingCartItem);
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
                    //var t = _dbContext.IsDisposed();
                    customer.ShoppingCartItems.Add(shoppingCartItem);
                    _customerManager.UpdateCustomer(customer);
                    //var s = _dbContext.IsDisposed();
                    //event notification
                    //_eventPublisher.EntityInserted(shoppingCartItem);
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
                //if (resetCheckoutData)
                //{
                //    //reset checkout data
                //    _customerManager.ResetCheckoutData(customer, shoppingCartItem.StoreId);
                //}
                //if (newQuantity > 0)
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
                    //DeleteShoppingCartItem(shoppingCartItem, resetCheckoutData, true);
                    DeleteShoppingCartItem(shoppingCartItem);
                }
            }

            return warnings;
        }


        public async Task<(decimal decGoldTotal, decimal decSilverTotal,decimal decPlatinumTotal,decimal decPalladiumTotal)> GetCartItemTotals(List<ShoppingCartItem> cart)
        {
            decimal decGoldTotal = 0;
            decimal decSilverTotal = 0;
            decimal decPlatinumTotal = 0;
            decimal decPalladiumTotal = 0;
            

            //Go through each cart items and find out total ounces for Gold, Silver, Platinum and Palladium
            foreach (ShoppingCartItem sci in cart)
            {
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Gold")
                {
                    decGoldTotal = decGoldTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);

                }
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Silver")
                {
                    decSilverTotal = decSilverTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Platinum")
                {
                    decPlatinumTotal = decPlatinumTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)(await _productRepository.GetAsync(sci.ProductId)).CommodityID).ToString() == "Palladium")
                {
                    decPalladiumTotal = decPalladiumTotal + Math.Round((await _productRepository.GetAsync(sci.ProductId)).Weight * sci.DecimalQuantity, 5);
                }
                
            }

            return (decGoldTotal, decSilverTotal, decPalladiumTotal, decPalladiumTotal);
        }

        public void UpdateShoppingCart()
        {

        }

        public void AddItemsToShoppingCart(IList<RequestForOrderItemModel> items, Dictionary<int, IList<string>> innerWarnings)
        {
            //foreach (var item in items)
            //{
            //    if (item.Quantity > 0)
            //    {
            //        var product = await _productRepository.GetAsync(item.ProductId);
            //        if (product != null)
            //        {
            //            var currSciWarnings = await AddToCart(_currentUser.User, product, cartType, storeId, string.Empty,
            //                decimal.Zero, Convert.ToInt32(item.Quantity), false, item.Quantity);
            //            if (currSciWarnings.Count > 0)
            //            {
            //                innerWarnings.Add(item.ProductId, currSciWarnings);
            //            }
            //        }
            //    }
            //}
        }

        public virtual void DeleteShoppingCartItem(ShoppingCartItem item)
        {
            _shoppingCartItemRepository.Delete(item);
        }

        public virtual async Task DeleteShoppingCartItems()
        {
            var cart = _currentUser.User.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest
                              && sci.StoreId == 1)
                              .ToList();
            await _shoppingCartItemRepository.DeleteRangeAsync(cart);
        }


        /// <summary>
        /// Finds a shopping cart item in the cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="product">Product</param>
        /// <param name="selectedAttributes">Selected attributes</param>
        /// <param name="customerEnteredPrice">Price entered by a customer</param>
        /// <returns>Found shopping cart item</returns>
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

            foreach (var sci in shoppingCart.Where(a => a.ShoppingCartType == shoppingCartType))
            {
                if (sci.ProductId == product.Id)
                {
                    //attributes
                    bool attributesEqual = _productAttributeParser.AreProductAttributesEqual(sci.AttributesXml, selectedAttributes);

                    //gift cards
                    bool giftCardInfoSame = true;
                    if (sci.Product.IsGiftCard)
                    {
                        string giftCardRecipientName1 = string.Empty;
                        string giftCardRecipientEmail1 = string.Empty;
                        string giftCardSenderName1 = string.Empty;
                        string giftCardSenderEmail1 = string.Empty;
                        string giftCardMessage1 = string.Empty;
                        _productAttributeParser.GetGiftCardAttribute(selectedAttributes,
                            out giftCardRecipientName1, out giftCardRecipientEmail1,
                            out giftCardSenderName1, out giftCardSenderEmail1, out giftCardMessage1);

                        string giftCardRecipientName2 = string.Empty;
                        string giftCardRecipientEmail2 = string.Empty;
                        string giftCardSenderName2 = string.Empty;
                        string giftCardSenderEmail2 = string.Empty;
                        string giftCardMessage2 = string.Empty;
                        _productAttributeParser.GetGiftCardAttribute(sci.AttributesXml,
                            out giftCardRecipientName2, out giftCardRecipientEmail2,
                            out giftCardSenderName2, out giftCardSenderEmail2, out giftCardMessage2);


                        if (giftCardRecipientName1.ToLowerInvariant() != giftCardRecipientName2.ToLowerInvariant() ||
                            giftCardSenderName1.ToLowerInvariant() != giftCardSenderName2.ToLowerInvariant())
                            giftCardInfoSame = false;
                    }

                    //price is the same (for products which require customers to enter a price)
                    bool customerEnteredPricesEqual = true;
                    if (sci.Product.CustomerEntersPrice)
                        customerEnteredPricesEqual = Math.Round(sci.CustomerEnteredPrice, 2) == Math.Round(customerEnteredPrice, 2);

                    //found?
                    if (attributesEqual && giftCardInfoSame && customerEnteredPricesEqual)
                        return sci;
                }
            }

            return null;
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

        protected async Task<bool> IsCommodityLimitViolated(RequestForOrderModel model, IList<ShoppingCartItem> cart, bool isSellMode = false, bool getWarnings = false)
        {
            bool boolReturn = false;
            decimal decGoldTotal = 0;
            decimal decSilverTotal = 0;
            decimal decPlatinumTotal = 0;
            decimal decPalladiumTotal = 0;


            //Go through each cart items and find out total ounces for Gold, Silver, Platinum and Palladium
            foreach (ShoppingCartItem sci in cart)
            {
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Gold")
                {
                    decGoldTotal = decGoldTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Silver")
                {
                    decSilverTotal = decSilverTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Platinum")
                {
                    decPlatinumTotal = decPlatinumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
                if (((ProductCommodityType)_productRepository.GetById(sci.ProductId).CommodityID).ToString() == "Palladium")
                {
                    decPalladiumTotal = decPalladiumTotal + Math.Round(_productRepository.GetById(sci.ProductId).Weight * sci.DecimalQuantity, 5);
                }
            }
            if (Session.GetObject<decimal>("decGoldBuyLimit") == 0m)
            {
                CustomerCommodityLimitsInfo commodityBuyLimits;
                commodityBuyLimits = await MtsApi.GetAMarkWebTradingLimits("B");
                if (commodityBuyLimits != null)
                {
                    Session.SetObject<decimal>("decGoldBuyLimit", commodityBuyLimits.decGoldLimit);
                    Session.SetObject<decimal>("decSilverBuyLimit", commodityBuyLimits.decSilverLimit);
                    Session.SetObject<decimal>("decPlatinumBuyLimit", commodityBuyLimits.decPlatinumLimit);
                    Session.SetObject<decimal>("decPalladiumBuyLimit", commodityBuyLimits.decPalladiumLimit);
                    
                }
                else
                {
                    Session.SetObject<decimal>("decGoldBuyLimit", 0m);
                    Session.SetObject<decimal>("decSilverBuyLimit", 0m);
                    Session.SetObject<decimal>("decPlatinumBuyLimit", 0m);
                    Session.SetObject<decimal>("decPalladiumBuyLimit", 0m);
                }
            }
            if (Session.GetObject<decimal>("") == 0m)
            {
                CustomerCommodityLimitsInfo commoditySellLimits;
                commoditySellLimits = await MtsApi.GetAMarkWebTradingLimits("S");
                if (commoditySellLimits != null)
                {
                    Session.SetObject<decimal>("decGoldSellLimit", commoditySellLimits.decGoldLimit);
                    Session.SetObject<decimal>("decSilverSellLimit", commoditySellLimits.decSilverLimit);
                    Session.SetObject<decimal>("decPlatinumSellLimit", commoditySellLimits.decPlatinumLimit);
                    Session.SetObject<decimal>("decPalladiumSellLimit", commoditySellLimits.decPalladiumLimit);
                  
                }
                else
                {
                    Session.SetObject<decimal>("decGoldSellLimit", 0m);
                    Session.SetObject<decimal>("decSilverSellLimit", 0m);
                    Session.SetObject<decimal>("decPlatinumSellLimit", 0m);
                    Session.SetObject<decimal>("decPalladiumSellLimit", 0m);
                  
                }
            }
            if (isSellMode)
            {
                if (Session.GetObject<decimal>("decGoldSellLimit") > 0m)
                {
                    if (decGoldTotal > Session.GetObject<decimal>("decGoldSellLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Gold Sell Limit of " + Session.GetString("decGoldSellLimit")  + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decSilverSellLimit") > 0m )
                {
                    if (decSilverTotal > Session.GetObject<decimal>("decSilverSellLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Silver Sell Limit of " + Session.GetString("decSilverSellLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPlatinumSellLimit") > 0m)
                {
                    if (decPlatinumTotal > Session.GetObject<decimal>("decPlatinumSellLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Platinum Sell Limit of " + Session.GetString("decPlatinumSellLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPalladiumSellLimit") > 0m)
                {
                    if (decPalladiumTotal > Session.GetObject<decimal>("decPalladiumSellLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Palladium Sell Limit of " + Session.GetString("decPalladiumSellLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
            }
            else
            {
                if (Session.GetObject<decimal>("decGoldBuyLimit") > 0m)
                {
                    if (decGoldTotal > Session.GetObject<decimal>("decGoldBuyLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Gold Buy Limit of " + Session.GetString("decGoldBuyLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decSilverBuyLimit") > 0m)
                {
                    if (decSilverTotal > Session.GetObject<decimal>("decSilverBuyLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Silver Buy Limit of " + Session.GetString("decSilverBuyLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPlatinumBuyLimit") > 0m)
                {
                    if (decPlatinumTotal > Session.GetObject<decimal>("decPlatinumBuyLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Platinum Buy Limit of " + Session.GetString("decPlatinumBuyLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPalladiumBuyLimit") > 0m)
                {
                    if (decPalladiumTotal > Session.GetObject<decimal>("decPalladiumBuyLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Palladium Buy Limit of " + Session.GetString("decPalladiumBuyLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
            }
            return boolReturn;
        }

        protected bool IsMinimumOrderPlacementIntervalValid()
        {
            //prevent 2 orders being placed within an X seconds time frame
            if (_orderSettings.MinimumOrderPlacementInterval == 0)
                return true;

            if (Session.GetObject<DateTime>("LastOrderPlacedDate") == null)
                return true;

            DateTime lastOrderDT = Session.GetObject<DateTime>("LastOrderPlacedDate");

            var interval = DateTime.UtcNow - lastOrderDT;
            return interval.TotalSeconds > _orderSettings.MinimumOrderPlacementInterval;
        }

        public async Task<List<SelectListItem>> GetVendors()
        {
            var vendors = await _vendorsRepository.GetAll()
                          .Where(v => v.Active && !v.Deleted)
                          .OrderBy(v => v.Name)
                          .Select(v => new SelectListItem
                          {
                              Text = v.Name,
                              Value = v.Id.ToString()
                          }).ToListAsync();
            return vendors;
        }



        private async Task<WindowsServiceEndPoint.MTS_Api> GetMtsApiInstance()
        {
            CustomerAttributes customerAttributes = await _genericAttributeManager.GetCustomerAttributes(_currentUser.User.Id);
            var serviceEndPoint = _config.GetSection("ConnectionStrings:WebAPI_URL").Value;
            WindowsServiceEndPoint.MTS_Api mtsApi = new WindowsServiceEndPoint.MTS_Api(serviceEndPoint, customerAttributes.AmarkTPAPIKey, customerAttributes.AmarkTradingPartnerNumber, _currentUser.User.Email);
            return mtsApi;
        }

        private ISession Session
        {
            get
            {
                return _context.HttpContext.Session;
            }
        }

        public MTS_Api MtsApi {
            get
            {
                if(_mtsApi == null)
                {
                    CustomerAttributes customerAttributes = _genericAttributeManager.GetCustomerAttributes(_currentUser.User.Id).Result;
                    var serviceEndPoint = _config.GetSection("ConnectionStrings:WebAPI_URL").Value;
                    _mtsApi = new WindowsServiceEndPoint.MTS_Api(serviceEndPoint, customerAttributes.AmarkTPAPIKey, customerAttributes.AmarkTradingPartnerNumber, _currentUser.User.Email);
                    return _mtsApi;
                }
                else
                {
                    return _mtsApi;
                }
            }
         }

    }

    public static class EfExtension
    {
        public static bool IsDisposed(this DbContext context)
        {
            var result = true;

            var typeDbContext = typeof(DbContext);
            var typeInternalContext = typeDbContext.Assembly.GetType("System.Data.Entity.Internal.InternalContext");

            var fi_InternalContext = typeDbContext.GetField("_internalContext", BindingFlags.NonPublic | BindingFlags.Instance);
            var pi_IsDisposed = typeInternalContext.GetProperty("IsDisposed");

            var ic = fi_InternalContext.GetValue(context);

            if (ic != null)
            {
                result = (bool)pi_IsDisposed.GetValue(ic);
            }

            return result;
        }
    }
}
#endregion
