using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    public class SpotPriceController : Controller
    {
        ISpotPriceManager _spotPriceManager;

        public SpotPriceController(ISpotPriceManager spotPriceManager)
        {
            _spotPriceManager = spotPriceManager;
        }

        [HttpGet("spotpricepreview")]
        public async Task<SpotPricePreviewViewModel> SpotPricePreview(string VsWhich = "NY")
        {

            var spotPrices = await _spotPriceManager.SpotPricePreview();
            return spotPrices;
        }

        [HttpGet("updatespots")]
        public async Task<UpdateSpotsViewModel> UpdateSpots(DateTime LastDate, string VsWhich = "NY", string IsAfterHours = "NO")
        {
            var updateSpotsViewModel = await _spotPriceManager.UpdateSpots(LastDate,VsWhich,IsAfterHours);
            return updateSpotsViewModel;
        }



        
    }
}