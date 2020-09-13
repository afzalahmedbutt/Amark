using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingPortal.Business.Extensions;
using TradingPortal.Business.interfaces;

using TradingPortal.Core.ViewModels;


namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class CustomerController : BaseController
    {
        private readonly IShoppingCartManager _shoppingCartManager;
        private readonly IHttpContextAccessor _context;

        public CustomerController(IShoppingCartManager shoppingCartManager, IHttpContextAccessor context) : base(context)
        {
            _shoppingCartManager = shoppingCartManager;
            _context = context;

        }

        [HttpGet("requestfororder")]
        public async Task<RequestForOrderModel> RequestForOrder(bool isSell)
        {
            return await _shoppingCartManager.RequestForOrder(isSell);
        }

        
        [HttpPost("revieworderrequest")]
        public async Task<RequestForOrderModel> ReviewOrderRequest([FromBody] RequestForOrderModel requestForOrderModel)
        {
            return await _shoppingCartManager.ReviewOrderRequest(requestForOrderModel);
        }

        
        [HttpPost("confirmorderrequest/{isSell}")]
        public async Task<bool> ConfirmOrderRequest(bool isSell)
        {
            return await _shoppingCartManager.ConfirmOrderRequest(isSell);
        }

        
        [HttpPost("markunmarkproductsasfavorite")]
        public async Task<RequestForOrderModel> MarkUnMarkProductsAsFavorite([FromBody]RequestForOrderModel requestForOrderModel)
        {
            return await _shoppingCartManager.MarkUnMarkProductsAsFavorite(requestForOrderModel);
        }

        
        [HttpGet("RequestForConfirmation")]
        public RequestForOrderModel RequestForConfirmation()
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

                orderModel.Date = DateTime.UtcNow.AddHours(-7).ToString("MM/dd/yyyy");
                orderModel.Time = DateTime.UtcNow.AddHours(-7).ToString("hh:mm") + " " + "PST";
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return orderModel;
        }

        
    }
}