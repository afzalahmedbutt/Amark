using System;

namespace TradingPortal.Infrastructure.Services.Interfaces
{
    public interface ILoggerManager
    {
        void LogInfo(Exception ex);
        void LogWarn(Exception ex);
        void LogDebug(Exception ex);
        void LogError(Exception ex);

    }
}
