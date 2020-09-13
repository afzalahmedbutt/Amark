using MTSWebApi;
using System.Collections.Generic;
using System.IO;


namespace TradingPortal.Business.interfaces
{
    public interface IExportManager
    {
        void ExportOpenPositionsToXlsx(Stream stream, List<OpenPositionItem> openpositions);
        void ExportTradingHistoryToXlsx(Stream stream, List<TradingHistoryItem> tradinghistory);
    }
}
