using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    public class FeedsController : BaseController
    {
        IFeedsManager _feedsManager;

        public FeedsController(IFeedsManager feedsManager, IHttpContextAccessor context) : base(context)
        {
            _feedsManager = feedsManager;
        }
        
        [HttpPost("fixhistory")]
        public async Task<List<EuropeanFixesViewModel>> FixHistory(DateTime startDate, DateTime endDate, string comCode = "G", string groupBy = "day")
        {
            var data = await _feedsManager.FixHistory(startDate, endDate, comCode, groupBy);
            return data;
                
        }

        
        [HttpPost("spothistory")]
        public async Task<List<SpotHistoryViewModel>> SpotHistory([FromForm]FeedsRequest request)
        {
            var data = await _feedsManager.SpotHistory(request.startDate, request.endDate, request.comCode, request.groupBy);
            return data;
        }

        
    }

    public class FeedsRequest
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string comCode { get; set; }
        public string groupBy { get; set; }
    }
}