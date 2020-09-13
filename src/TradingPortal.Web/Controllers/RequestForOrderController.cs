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
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Business.interfaces;
using Microsoft.Extensions.Configuration;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TradingPortal.Core.ViewModels;
using TradingPortal.Business.Extensions;
using System.Linq;
using TradingPortal.Core.Enums;
using MTSWebApi;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation;
using static TradingPortal.Core.ViewModels.RequestForOrderModel;
using TradingPortal.Infrastructure.UnitOfWork;
using System.Text;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class RequestForOrderController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly ICustomerManager _customerManager;
        private readonly ICurrentUser _currentUser;
        private readonly IHttpContextAccessor _context;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ApplicationDbContext _dbContext;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly CustomSettings _customSettings;
        private readonly OrderSettings _orderSettings;
        private readonly IRequestForOrderManager _requestForOrderManager;
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailManager _emailManager;
        private readonly IUnitOfWork _unitOfWork;

        


        public RequestForOrderController(
            IConfiguration config,
            IGenericAttributeManager genericAttributeManager,
            ICurrentUser currentUser,
            IHttpContextAccessor context,
            IRepository<ShoppingCartItem> shoppingCartItemRepository,
            ICustomerManager customerManager,
            IProductRepository productRepository,
            IProductAttributeService productAttributeService,
            IProductAttributeParser productAttributeParser,
            ApplicationDbContext dbContext,
            IRequestForOrderManager requestForOrderManager,
            IRepository<CustomerFavoriteProduct> favoriteProductRepostiry,
            IRepository<MessageTemplate> messageTemplateRepository,
            ShoppingCartSettings shoppingCartSettings,
            OrderSettings orderSettings,
            CustomSettings customSettings,
            IEmailSender emailSender,
            IEmailManager emailManager,
            IUnitOfWork unitOfWork) : base(context)
        {
            _config = config;
            _genericAttributeManager = genericAttributeManager;
            _currentUser = currentUser;
            _context = context;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _customerManager = customerManager;
           
            _productAttributeService = productAttributeService;
            _productAttributeParser = productAttributeParser;
            _dbContext = dbContext;
            //_settingsManager = settingsManager;
            _shoppingCartSettings = shoppingCartSettings;
            _customSettings = customSettings;
            _orderSettings = orderSettings;
            _requestForOrderManager = requestForOrderManager;
            //_favoriteProductRepostiry = favoriteProductRepostiry;
            _messageTemplateRepository = messageTemplateRepository;
            _emailManager = emailManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("requestfororder")]
        public async Task<RequestForOrderModel> RequestForOrder(bool isSell)
        {
            //bool isSell = isSell;
            bool isEditable = true;
            bool isHfi = false;
            bool isDropShip = false;
            string spInstructions = "";
            string tpConfirmation = "";
            //if (!isSell)
            //    throw new Exception("Test Exception");
            //save seleted order mode for now
            Session.SetObject<bool>("IsSellMode", isSell);
            //if we got here - termshave been accepted, save this so we do not ask user again
            Session.SetObject<bool>("TermsAccepted", true);

            RequestForOrderModel model = new RequestForOrderModel();
            try
            {
                await _requestForOrderManager.DeleteShoppingCartItems();

                var cart = _currentUser.User.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.AMarkOrderRequest)
                    .Where(sci => sci.StoreId == 1)
                    .ToList();

                return await PrepareRequestForOrderModel(model, cart, isSell, isEditable, isHfi, isDropShip, spInstructions, tpConfirmation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected async Task<RequestForOrderModel> PrepareRequestForOrderModel(RequestForOrderModel model,
            IList<ShoppingCartItem> cart, bool isSellMode = false, bool isEditable = false, bool IsHfi = false, bool IsDropShip = false, string spInstructions = "", string tpConfirmation = "",
            string name1 = "", string name2 = "", string address1 = "", string address2 = "", string city = "", string state = "", string zip = "", string country = "",
            Dictionary<int, IList<string>> warnings = null, ProductsQuoteInfo pinfo = null, bool isEditOrderRequest = false)
        {
            model.IsSellMode = isSellMode;
            model.IsEditable = isEditable;
            //model.PriceExpireInterval = Convert.ToInt32(_customSettings.PriceExpiredSeconds);



            MetalsTotal metalsTotal = await _requestForOrderManager.GetMetalsTotal(cart);

            //#region Discounts Info
            if (!isEditOrderRequest)  //If request is for edit order then dont set discounts info
            {
                ProductsDiscountsInfo prodDiscounts;
                if (_context.HttpContext.Session.GetObject<decimal>("decGoldDiscount") != 0m)
                {
                    model.IsDiscountPricing = false;
                    if (Session.GetObject<decimal>("decGoldDiscount") > 0m)
                    {
                        if (metalsTotal.DecGoldTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decGoldDiscount")))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                    if (model.IsDiscountPricing == false)
                    {
                        if (Session.GetObject<decimal>("decSilverDiscount") > 0m)
                        {
                            if (metalsTotal.DecSilverTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decSilverDiscount")))
                            {
                                model.IsDiscountPricing = true;
                            }
                        }
                    }
                    if (model.IsDiscountPricing == false)
                    {
                        if (Session.GetObject<decimal>("decPlatinumDiscount") > 0m)
                        {
                            if (metalsTotal.DecPlatinumTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decPlatinumDiscount")))
                            {
                                model.IsDiscountPricing = true;
                            }
                        }
                    }
                    if (model.IsDiscountPricing == false)
                    {
                        if (Session.GetObject<decimal>("decPalladiumDiscount") > 0m)
                        {
                            if (metalsTotal.DecPalladiumTotal >= Convert.ToDecimal(Session.GetObject<decimal>("decPalladiumDiscount")))
                            {
                                model.IsDiscountPricing = true;
                            }
                        }
                    }

                }
                else
                {
                    prodDiscounts = await _requestForOrderManager.GetAMarkWebProductsDiscounts();
                    Session.SetObject<decimal>("decGoldDiscount", prodDiscounts.decGoldDiscount);
                    Session.SetObject<decimal>("decSilverDiscount", prodDiscounts.decSilverDiscount);
                    Session.SetObject<decimal>("decPlatinumDiscount", prodDiscounts.decPlatinumDiscount);
                    Session.SetObject<decimal>("decPalladiumDiscount", prodDiscounts.decPalladiumDiscount);
                    model.IsDiscountPricing = false;

                    if (Session.GetObject<decimal>("decGoldDiscount") > 0m)
                    {
                        if (metalsTotal.DecGoldTotal >= Session.GetObject<decimal>("decGoldDiscount"))
                        {
                            model.IsDiscountPricing = true;
                        }
                    }
                    if (model.IsDiscountPricing == false)
                    {
                        if (Session.GetObject<decimal>("decSilverDiscount") > 0m)
                        {
                            if (metalsTotal.DecSilverTotal >= Session.GetObject<decimal>("decSilverDiscount"))
                            {
                                model.IsDiscountPricing = true;
                            }
                        }
                    }
                    if (model.IsDiscountPricing == false)
                    {
                        if (Session.GetObject<decimal>("decPlatinumDiscount") > 0m)
                        {
                            if (metalsTotal.DecPlatinumTotal >= Session.GetObject<decimal>("decPlatinumDiscount"))
                            {
                                model.IsDiscountPricing = true;
                            }
                        }
                    }
                    if (model.IsDiscountPricing == false)
                    {
                        if (Session.GetObject<decimal>("decPalladiumDiscount") > 0m)
                        {
                            if (metalsTotal.DecPalladiumTotal >= Session.GetObject<decimal>("decPalladiumDiscount"))
                            {
                                model.IsDiscountPricing = true;
                            }
                        }
                    }

                }
                await IsCommodityLimitViolated(model, cart, isSellMode, true);
            }
            //get Amark web products pricing info
            ProductsPricingInfo prodPricing = null;
            ProductPricingItem[] productPricingItems = null;
            if (pinfo == null) // Request for order
            {
                prodPricing = await _requestForOrderManager.GetAMarkWebProductsPricing(metalsTotal, model.IsSellMode);
                productPricingItems = prodPricing.objWebProductsPricingList;
            }
            else
            {
                productPricingItems = pinfo.objWebProductsPricingList;
            }

            //If pinfo is not null then it is review/requote request
            //
            if ((prodPricing != null || pinfo != null))
            {
                if (model.IsEditable && !isEditOrderRequest)
                {
                    if (prodPricing != null)
                    {
                        await _requestForOrderManager.SetRequestForOrderItems(model, cart, productPricingItems);
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
                    if (isEditOrderRequest)
                    {
                        await _requestForOrderManager.SetEditRequestForOrderItems(model, cart, productPricingItems);
                    }
                    else
                    {
                        await _requestForOrderManager.SetRequestForOrderItems(model, cart, productPricingItems);

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
                if (prodPricing != null)
                {
                    model.MultipleTransactionGroupsPresent = (prodPricing.transactionGroupCodesCount > 1) ? true : false;
                }
                Session.SetObject<bool>("MultipleTransactionGroupsPresent", model.MultipleTransactionGroupsPresent);

                if (model.SmallOrderCharge != null && model.SmallOrderCharge != "" && model.SmallOrderCharge != "0.00000")
                {
                    model.DecTotal += Decimal.Parse(model.SmallOrderCharge);
                }

                //Review Order Request
                if (pinfo != null)
                {
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

                }
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



        [HttpPost("revieworderrequest")]
        public async Task<RequestForOrderModel> ReviewOrderRequest([FromBody]RequestForOrderModel requestForOrderModel)
        {
            //bool isSell = false;
            bool isEditable = false;
            var storeId = 1;
            var model = new RequestForOrderModel();

            var cart = _requestForOrderManager.GetUserShoppingCartItems();

            //Create only cart items for products that have qty
            //current warnings <cart item identifier, warnings>
            var innerWarnings = new Dictionary<int, IList<string>>();
            var cartType = ShoppingCartType.AMarkOrderRequest;


            if (!requestForOrderModel.IsRequote) //If requote then dont add products to shopping cart as they have already been added
            {
                foreach (var item in requestForOrderModel.Items.Where(item => item.Quantity > 0))
                {
                    if (item.Quantity > 0)
                    {
                        var product = await _requestForOrderManager.GetProductByIdAsync(item.ProductId);
                        if (product != null)
                        {
                            var currSciWarnings = _requestForOrderManager.AddToCart(_currentUser.User, product, cartType, storeId, string.Empty,
                                decimal.Zero, Convert.ToInt32(item.Quantity), false, item.Quantity);
                            //var currSciWarnings = AddToCart(_currentUser.User, product, cartType, storeId, string.Empty,
                            //    decimal.Zero, Convert.ToInt32(item.Quantity), false, item.Quantity);
                            if (currSciWarnings.Count > 0)
                            {
                                innerWarnings.Add(item.ProductId, currSciWarnings);
                            }
                        }
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }

            //IY - Seems like redundant code
            //in case there are any products in the Cart that we changed qty back to 0 - update Cart (remove 0 items)
            foreach (var sci in cart)
            {
                var product = requestForOrderModel.Items.FirstOrDefault(i => i.ProductId == sci.ProductId);
                if (product != null)
                {
                    var currSciWarnings = _requestForOrderManager.UpdateShoppingCartItem(_currentUser.User,
                                    sci.Id, Convert.ToInt32(product.Quantity), true, product.Quantity);
                    if (currSciWarnings.Count > 0)
                    {
                        innerWarnings.Add(sci.ProductId, currSciWarnings);
                    }
                }
            }
            await _unitOfWork.SaveChangesAsync();

            //updated cart
            cart = _requestForOrderManager.GetUserShoppingCartItems();

            try
            {

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

                if (requestForOrderModel.IsSell || requestForOrderModel.IsRequote
                    || await _requestForOrderManager.AddressValid(requestForOrderModel.Country, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip))
                {
                    //var orderType = requestForOrderModel.IsSell ? "B" : "S";
                    //ProductsQuoteInfo pinfo = await MtsApi.RequestAmarkOnlineQuote(orderType, productQuoteItems, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip == true ? "Drop Ship" : "Default",
                    //                                                    requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, "", requestForOrderModel.TPConfirmation, requestForOrderModel.SpecialInstructions);
                    ProductsQuoteInfo pinfo = await _requestForOrderManager.RequestAmarkOnlineQuote(requestForOrderModel, productQuoteItems);
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
                    await PrepareRequestForOrderModel(model, cart, requestForOrderModel.IsSell, isEditable, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip, requestForOrderModel.SpecialInstructions, requestForOrderModel.TPConfirmation, requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, innerWarnings, pinfo);
                    //await PrepareReviewForOrderModel(model, cart, requestForOrderModel.IsSell, isEditable, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip, requestForOrderModel.SpecialInstructions, requestForOrderModel.TPConfirmation, requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, innerWarnings, pinfo);
                }
                else
                {
                    isEditable = true;
                    //ViewData["Message"] = "1";
                    //await PrepareEditRequestForOrderModel(model, cart, requestForOrderModel.IsSell, isEditable, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip, requestForOrderModel.SpecialInstructions, requestForOrderModel.TPConfirmation, requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, true);
                    await PrepareRequestForOrderModel(model, cart, requestForOrderModel.IsSell, isEditable, requestForOrderModel.IsHFI, requestForOrderModel.IsDropShip, requestForOrderModel.SpecialInstructions, requestForOrderModel.TPConfirmation, requestForOrderModel.Name1, requestForOrderModel.Name2, requestForOrderModel.Address1, requestForOrderModel.Address2, requestForOrderModel.City, requestForOrderModel.State, requestForOrderModel.Zip, requestForOrderModel.Country, isEditOrderRequest: true);
                }
            }
            catch (Exception ex)
            {
                model.Warnings.Add(ex.Message);

            }
            Session.SetObject<RequestForOrderModel>("OrderRequestInfo", model);
            return model;
        }




        [HttpPost("editorderrequest")]
        public async Task<RequestForOrderModel> EditOrderRequest([FromBody] RequestForOrderModel requestForOrderModel)
        {
            //bool isSell = false;
            bool isEditable = true;
            bool isHfi = false;
            bool isDropShip = false;
            string spInstructions = "";
            string tpConfirmation = "";
            requestForOrderModel.Items = new List<RequestForOrderItemModel>();
            var cart = _requestForOrderManager.GetUserShoppingCartItems();

            foreach (var sci in cart)
            {
                var product = requestForOrderModel.Items.FirstOrDefault(i => i.ProductId == sci.ProductId);
                if (product != null)
                {
                    var currSciWarnings = _requestForOrderManager.UpdateShoppingCartItem(_currentUser.User,
                                    sci.Id, Convert.ToInt32(product.Quantity), true, product.Quantity);
                    //if (currSciWarnings.Count > 0)
                    //{
                    //    innerWarnings.Add(sci.ProductId, currSciWarnings);
                    //}
                }
            }

            cart = _requestForOrderManager.GetUserShoppingCartItems();
            return await PrepareRequestForOrderModel(requestForOrderModel, cart, requestForOrderModel.IsSell, isEditable, isHfi, isDropShip, spInstructions, tpConfirmation, isEditOrderRequest: true);
        }


        [HttpPost("addremovefavoriteproduct")]
        public async Task<bool> AddRemoveFavoriteProduct([FromBody]CustomerFavoriteProduct product)
        {
            return await _requestForOrderManager.AddRemoveFavoriteProduct(product);
          
        }

        [HttpPost("confirmorderrequest/{isSell}")]
        public async Task<ConfirmOrderResponse> ConfirmOrderRequest(bool isSell)
        {
            var response = new ConfirmOrderResponse();
            var model = new RequestForOrderModel();
            try
            {
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

                Session.SetObject<DateTime>("LastOrderPlacedDate", DateTime.UtcNow);
                var requestType = isSell ? "B" : "S";
                RequestOnlineTradeInfo rinfo = await _requestForOrderManager.RequestAmarkOnlineTrade(requestType, model.QuoteKey, model.IsHFI, model.IsDropShip == true ? "Drop Ship" : "Default",
                                                                    model.Name1, model.Name2, model.Address1, model.Address2, model.City, model.State, model.Zip, model.Country, "", model.TPConfirmation);

                
                if (rinfo.sStatusMessage == "Trade Confirmed")
                {
                    model.QuoteKey = rinfo.sQuoteKey;
                    model.sTicketNumber = rinfo.sTicketNumber;
                    model.IsHedgedOrder = rinfo.IsHedgedOrder;

                    await SendOrderPlacedAmarkNotification(model, isSell);
                    
                }
                else
                {
                    throw new Exception(rinfo.sStatusMessage); 
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                response.IsSuccess = false;
                response.Warnings.Add(ex.Message);
            }
            Session.SetObject<RequestForOrderModel>("OrderRequestInfo", model);
            return response;
        }


        async Task<bool> SendOrderPlacedAmarkNotification(RequestForOrderModel model, bool isSell)
        {
            var messageTemplate = await _messageTemplateRepository.FindAsync(mt => mt.Name == "OrderPlaced.AmarkTradingNotification");
            var emailAccount = _emailManager.GetEmailAccountById(messageTemplate.EmailAccountId);
            messageTemplate.Body = messageTemplate.Body.Replace("%Trade.Type%", isSell ? "Sell" : "Buy");
            messageTemplate.Body = messageTemplate.Body.Replace("%Trade.Information%",_requestForOrderManager.CreateTradeConfirmationEmailData(model));
            var toEmail = _customSettings.TradeNotificationEmail.Split(',');
            await _emailSender.SendEmailAsync(messageTemplate, messageTemplate.Subject, emailAccount, toEmail);

            return true;
        }


        [HttpGet("requestforconfirmation")]
        public async Task<RequestForOrderModel> RequestForConfirmation()
        {
            var orderModel = new RequestForOrderModel();
            orderModel.IsTradeConfirmed = true;
            try
            {
                orderModel = Session.GetObject<RequestForOrderModel>("OrderRequestInfo");
                if (orderModel == null)
                {
                    throw new Exception("Order details data is not available.");
                }
                else
                {
                    if (orderModel.Items.Count == 0)
                        throw new Exception("Order details data is empty.");
                }

                await _requestForOrderManager.DeleteShoppingCartItems();
                orderModel.Date = DateTime.UtcNow.AddHours(-7).ToString("MM/dd/yyyy");
                orderModel.Time = DateTime.UtcNow.AddHours(-7).ToString("hh:mm") + " " + "PST";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orderModel;
        }


        //string CreateTradeConfirmationEmailData(RequestForOrderModel model)
        //{

        //    decimal decGoldTotal = 0;
        //    decimal decSilverTotal = 0;
        //    decimal decPlatinumTotal = 0;
        //    decimal decPalladiumTotal = 0;
        //    decimal decGoldTotalQty = 0;
        //    decimal decSilverTotalQty = 0;
        //    decimal decPlatinumTotalQty = 0;
        //    decimal decPalladiumTotalQty = 0;
        //    string strLastCommodity = "";
        //    decimal decTotal = 0;
        //    decimal decTotalQty = 0;


        //    StringBuilder table = new StringBuilder();
        //    table.Append("<table style=\"border-collapse:collapse; text-align:center;border: 1px solid #999\">");
        //    table.Append("<thead>");
        //    table.Append("<tr class=\"cart-header-row\">");
        //    table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //    table.Append("Commodity");
        //    table.Append("</th>");

        //    table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //    table.Append("Product Name");
        //    table.Append("</th>");

        //    table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //    table.Append("Quantity");
        //    table.Append("</th>");

        //    table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //    table.Append("Oz.");
        //    table.Append("</th>");

        //    table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //    table.Append("Unit Price");
        //    table.Append("</th>");

        //    table.Append("<th style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //    table.Append("Item Total");
        //    table.Append("</th>");

        //    table.Append("</tr>");
        //    table.Append("</thead>");
        //    table.Append("<tbody>");

        //    foreach (var item in model.Items)
        //    {
        //        if (item.Commodity.ToUpper() == "GOLD")
        //        { decGoldTotal = decGoldTotal + item.SubTotal; decGoldTotalQty = decGoldTotalQty + item.WeightSubTotal; }
        //        if (item.Commodity.ToUpper() == "SILVER")
        //        { decSilverTotal = decSilverTotal + item.SubTotal; decSilverTotalQty = decSilverTotalQty + item.WeightSubTotal; }
        //        if (item.Commodity.ToUpper() == "PLATINUM")
        //        { decPlatinumTotal = decPlatinumTotal + item.SubTotal; decPlatinumTotalQty = decPlatinumTotalQty + item.WeightSubTotal; }
        //        if (item.Commodity.ToUpper() == "PALLADIUM")
        //        { decPalladiumTotal = decPalladiumTotal + item.SubTotal; decPalladiumTotalQty = decPalladiumTotalQty + item.WeightSubTotal; }
        //        decTotal = decTotal + item.SubTotal;
        //        decTotalQty = decTotalQty + item.WeightSubTotal;
        //        table.Append("<tr class=\"cart-item-row\">");
        //        table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //        if (strLastCommodity != item.Commodity)
        //        {
        //            strLastCommodity = item.Commodity;
        //            table.Append(item.Commodity + "</td>");
        //        }
        //        else
        //        {
        //            table.Append("&nbsp;</td>");
        //        }
        //        //Product Description
        //        table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //        table.Append(item.ProductDesc);
        //        table.Append("</td>");

        //        //Product Quantity
        //        table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //        table.Append(item.Quantity);
        //        table.Append("</td>");

        //        //Product Weight
        //        table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //        table.Append(item.WeightSubTotal.ToString("#0.00000"));
        //        table.Append("</td>");

        //        //Product Unit Price
        //        table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //        table.Append(item.UnitPrice.ToString("C"));
        //        table.Append("</td>");

        //        //Product SubTotal
        //        table.Append("<td style=\"border: 1px solid #999;padding: 0.5rem;text-align: left;\">");
        //        table.Append(item.SubTotal.ToString("C"));
        //        table.Append("</td>");

        //        table.Append("</tr>");

        //    }
        //    table.Append("</tbody>");
        //    table.Append("</table>");

        //    return table.ToString();
        //}

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

                string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
                string htmlTableEnd = "</table>";

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

                StringBuilder thead = new StringBuilder();

                StringBuilder rows = new StringBuilder();
                rows.Append("<thead>");
                rows.Append("<tr class=\"cart-header-row\">");
                rows.Append("<th>");
                rows.Append("Commodity");
                rows.Append("</th>");

                rows.Append("<th>");
                rows.Append("Product Name");
                rows.Append("</th>");

                rows.Append("<th>");
                rows.Append("Quantity");
                rows.Append("</th>");

                rows.Append("<th>");
                rows.Append("Oz.");
                rows.Append("</th>");

                rows.Append("<th>");
                rows.Append("Unit Price");
                rows.Append("</th>");

                rows.Append("<th>");
                rows.Append("Item Total");
                rows.Append("</th>");

                rows.Append("</tr>");
                rows.Append("</thead>");
                rows.Append("</tbody>");

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
                    rows.Append("<tr class=\"cart-item-row\">");
                    rows.Append("<td class=\"commodity\">");
                    if (strLastCommodity != item.Commodity)
                    {
                        strLastCommodity = item.Commodity;
                        rows.Append(item.Commodity);
                    }
                    else
                    {
                        rows.Append("&nbsp;</td>");
                    }
                    //Product Description
                    rows.Append("<td class=\"product\">");
                    rows.Append(item.ProductDesc);
                    rows.Append("</td>");

                    //Product Quantity
                    rows.Append("<td class=\"qty nobr\">");
                    rows.Append(item.Quantity);
                    rows.Append("</td>");

                    //Product Weight
                    rows.Append("<td class=\"weight nobr\">");
                    rows.Append(item.WeightSubTotal.ToString("#0.00000"));
                    rows.Append("</td>");

                    //Product Unit Price
                    rows.Append("<td class=\"unit-price nobr\">");
                    rows.Append(item.UnitPrice.ToString("#0.00000"));
                    rows.Append("</td>");

                    //Product SubTotal
                    rows.Append("<td class=\"subtotal nobr end\">");
                    rows.Append(item.SubTotal.ToString("#0.00000"));
                    rows.Append("</td>");

                    rows.Append("</tr>");

                    rows.Append("</tbody>");
                    rows.Append("</table>");


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

            MetalsTotal metalsTotal = await _requestForOrderManager.GetMetalsTotal(cart);
            if (Session.GetObject<decimal>("decGoldBuyLimit") == 0m)
            {
                CustomerCommodityLimitsInfo commodityBuyLimits;
                commodityBuyLimits = await _requestForOrderManager.GetAMarkWebTradingLimits("B");
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
                commoditySellLimits = await _requestForOrderManager.GetAMarkWebTradingLimits("S");
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
                    if (metalsTotal.DecGoldTotal > Session.GetObject<decimal>("decGoldSellLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Gold Sell Limit of " + Session.GetString("decGoldSellLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decSilverSellLimit") > 0m)
                {
                    if (metalsTotal.DecSilverTotal > Session.GetObject<decimal>("decSilverSellLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Silver Sell Limit of " + Session.GetString("decSilverSellLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPlatinumSellLimit") > 0m)
                {
                    if (metalsTotal.DecPlatinumTotal > Session.GetObject<decimal>("decPlatinumSellLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Platinum Sell Limit of " + Session.GetString("decPlatinumSellLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPalladiumSellLimit") > 0m)
                {
                    if (metalsTotal.DecPalladiumTotal > Session.GetObject<decimal>("decPalladiumSellLimit"))
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
                    if (metalsTotal.DecGoldTotal > Session.GetObject<decimal>("decGoldBuyLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Gold Buy Limit of " + Session.GetString("decGoldBuyLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decSilverBuyLimit") > 0m)
                {
                    if (metalsTotal.DecSilverTotal > Session.GetObject<decimal>("decSilverBuyLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Silver Buy Limit of " + Session.GetString("decSilverBuyLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPlatinumBuyLimit") > 0m)
                {
                    if (metalsTotal.DecSilverTotal > Session.GetObject<decimal>("decPlatinumBuyLimit"))
                    {
                        if (getWarnings)
                            model.Warnings.Add("Platinum Buy Limit of " + Session.GetString("decPlatinumBuyLimit") + " has been exeeded.");
                        boolReturn = true;
                    }
                }
                if (Session.GetObject<decimal>("decPalladiumBuyLimit") > 0m)
                {
                    if (metalsTotal.DecPalladiumTotal > Session.GetObject<decimal>("decPalladiumBuyLimit"))
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

    }
}

