using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Business;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain.Amark;

namespace TradingPortal.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager, IHttpContextAccessor context) : base(context)
        {
            _productManager = productManager;
        }

        [Route("api/Product/GetWholeSalePrices")]
        public async Task<List<WholeSaleResponseViewModel>> GetWholeSalePrices()
        {
            return await _productManager.GetWholeSalePrices();
        }

        [Route("api/Product/GetAllProductDetails/{comCode}")]
        public async Task<List<Brochure_Products>> GetAllProductDetails(string comCode = "G")
        {
            return await _productManager.GetAllProductDetails(comCode);
        }

    }
}