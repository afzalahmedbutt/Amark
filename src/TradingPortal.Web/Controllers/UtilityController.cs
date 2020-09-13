using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Constants;
using TradingPortal.Core.Domain.Common;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Web.Controllers
{
    public class UtilityController : Controller
    {
        private readonly ITopicManager _topicManager;
        private readonly ISpotPriceManager _spotPriceManager;
        private readonly IHttpContextAccessor _context;
        private readonly ILoggerManager _loggerManager;
        private readonly CustomSettings _customSettings;
        private readonly IProductManager _productManager;
        private readonly IContactUsManager _contactUsManager;
        public readonly ISettingsService _settingsService;
        public UtilityController(
            ITopicManager topicManager, 
            ISpotPriceManager spotPriceManager, 
            IHttpContextAccessor context,
            ILoggerManager loggerManager,
            CustomSettings customSettings,
            IProductManager productManager,
            IContactUsManager contactUsManager,
            ISettingsService settingsService)
        {
            _topicManager = topicManager;
            _spotPriceManager = spotPriceManager;
            _context = context;
            _loggerManager = loggerManager;
            _customSettings = customSettings;
            _productManager = productManager;
            _contactUsManager = contactUsManager;
            _settingsService = settingsService;
        }

        [HttpGet]
        [Route("api/Utility/GetServerTime")]
        public ActionResult GetServerTime()
        {
            DateTime serverTime = DateTime.Now;
            return Json(
                new
                {
                    serverTime = serverTime
                });
        }

        [HttpGet]
        [Route("api/Utility/GetContractDetails")]
        public async Task<TopicViewModel> GetContractDetails()
        {
           
            return await _topicManager.GetTopicBySystemName(TopicSystemName.CONDITIONS_OF_USE);
        }

        [HttpGet]
        [Route("api/Utility/GetAppInitData")]
        public async Task<AppInitData> GetAppInitData()
        {
            try
            {
                //throw new Exception("Invalid Operation!!");
                //return null;
                AppInitData appInitData = new AppInitData();
                appInitData.ServerTime = DateTime.Now;
                appInitData.UpdateSpotsViewModel = await _spotPriceManager.UpdateSpots(DateTime.Parse("2012-01-01T00:00:00"));
                //appInitData.SpotPricePreviewViewModel = await _spotPriceManager.SpotPricePreview();
                appInitData.ServiceContract = await _topicManager.GetTopicBySystemName(TopicSystemName.CONDITIONS_OF_USE);
                appInitData.MaxProductQuantity = _customSettings.MaxProductQuantity;
                appInitData.ContactFormData = _contactUsManager.GetContactFormData();
                appInitData.IsPortalClosed = _settingsService.GetSettingsByKeyAndStoreId<bool>("storeinformationsettings.storeclosed", storeId: 1);
                return appInitData;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex);
                throw ex;
            }
        }
    }
}