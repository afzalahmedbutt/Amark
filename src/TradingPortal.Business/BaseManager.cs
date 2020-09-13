using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Services.Interfaces;
using WindowsServiceEndPoint;

namespace TradingPortal.Business
{
    public class BaseManager
    {
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly ICurrentUser _currentUser;
        private readonly IConfiguration _config;
        CustomerAttributes _customerAttributes;
        private MTS_Api _mtsApi = null;

        public BaseManager(
            IGenericAttributeManager genericAttributeManager, 
            ICurrentUser currentUser, 
            IConfiguration config,
            CustomerAttributes customerAttributes)
        {
            _genericAttributeManager = genericAttributeManager;
            _currentUser = currentUser;
            _config = config;
            _customerAttributes = customerAttributes;
        }

        public MTS_Api MtsApi
        {
            get
            {
                if (_mtsApi == null)
                {
                    //CustomerAttributes customerAttributes = _genericAttributeManager.GetCustomerAttributes(_currentUser.User.Id).Result;
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
