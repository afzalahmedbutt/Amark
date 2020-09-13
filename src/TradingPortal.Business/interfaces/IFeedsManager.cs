using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Business.interfaces
{
    public interface IFeedsManager
    {
        Task<List<EuropeanFixesViewModel>> FixHistory(DateTime startDate, DateTime endDate, string comCode = "G", string groupBy = "day");
        Task<List<SpotHistoryViewModel>> SpotHistory(DateTime startDate, DateTime endDate, string comCode = "G", string groupBy = "day");
    }
}
