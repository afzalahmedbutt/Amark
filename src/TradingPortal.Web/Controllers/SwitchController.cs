using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using TradingPortal.Business;
using TradingPortal.Business.interfaces;
using TradingPortal.Infrastructure.Services.Interfaces;
using TradingPortal.Infrastructure.UnitOfWork;


namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Administrators")]
    public class SwitchController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ISettingsService _settingsService;
        private readonly IStoreManager _storeManager;
        public SwitchController(ISettingsService settingsservice, IStoreManager storeManager, IUnitOfWork unitOfWork)
        {
            _settingsService = settingsservice;
             _storeManager = storeManager;
            _unitOfWork = unitOfWork;
        }

        
        [HttpGet("getstoreclosestatus")]
        public bool GetStoreCloseStatus()
        {
            try
            {
                var status = _settingsService.GetSettingsByKeyAndStoreId<bool>("storeinformationsettings.storeclosed",storeId:1);
                return status;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpPost("setstoreclosestatus/{isPortalClosed}")]
        public async Task<bool> SetStoreCloseStatus(bool isPortalClosed)
        {
            //_settingsService.SetSetting<bool>("storeinformationsettings.storeclosed", isPortalClosed, 1, true);
            //await _unitOfWork.SaveChangesAsync();
            await _storeManager.SetStoreCloseStatus(isPortalClosed);
            return true;
        }
    }
}