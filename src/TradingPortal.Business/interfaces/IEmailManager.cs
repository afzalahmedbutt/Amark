using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain;

namespace TradingPortal.Business.interfaces
{
    public interface IEmailManager
    {
        EmailAccount GetEmailAccountById(int id);

    }
}
