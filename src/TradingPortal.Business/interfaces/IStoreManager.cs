using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingPortal.Business.interfaces
{
    public interface IStoreManager
    {
        Task<bool> SetStoreCloseStatus(bool isPortalClosed);
    }
}
