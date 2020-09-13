using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Infrastructure.Repositories.Interfaces;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Core.Domain.Amark;

namespace TradingPortal.Business
{
    public class ProductManager  :IProductManager
    {
        private readonly IAmarkProductRepository _productRepository;
        public ProductManager(IAmarkProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        
        public async Task<List<WholeSaleResponseViewModel>> GetWholeSalePrices()
        {
            var wholeSalePrices = await _productRepository.GetWholesalePrices();
            var wholeSalePricesViewModel = wholeSalePrices.GroupBy(wsp => wsp.ComCode)
                .Select(wspg => new WholeSaleResponseViewModel
                {
                    ComCode = wspg.Key,
                    WholesalePrices = wspg.Select(wsp => new WholeSalePriceViewModel
                    {
                        BrochureProductId = wsp.Products != null ? wsp.Products.BROCHURE_PRODUCT_ID.ToString() : "",
                        ProductDescription = wsp.ProductDescription,
                        WholesaleBidPrice = wsp.WholesaleBidPrice,
                        WholesaleAskPrice = wsp.WholesaleAskPrice,
                        UpdateDate = wsp.Update_Date.ToString("dd-MMM hh:mm tt")
                    }).ToList()
                }).ToList();
            //var t = wholeSalePricesViewModel.SelectMany(wspv => wspv.WholesalePrices).Where(wsp => wsp.BrochureProductId == null).ToList();
            return wholeSalePricesViewModel;
        }

        public async Task<List<Brochure_Products>> GetAllProductDetails(string comCode = "G")
        {
            return await _productRepository.GetBrochureProductsByComCode(comCode);
        }
    }

    public class WholeSaleResponseViewModel
    {
        public string ComCode { get; set; }
        public List<WholeSalePriceViewModel> WholesalePrices { get; set; }
    }
    public class WholeSalePriceViewModel
    {
        public string BrochureProductId { get; set; }
        public string ProductDescription { get; set; }
        public decimal WholesaleBidPrice { get; set; }
        public decimal WholesaleAskPrice { get; set; }
        public string UpdateDate { get; set; }

    }
}
