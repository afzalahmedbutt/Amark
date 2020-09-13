using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Infrastructure.Helpers
{
    public static class ExceptionExtensions
    {
        public static string GetFullMessageStackTrace(this Exception ex)
        {
            return ex.InnerException == null
                 ? (ex.Message + Environment.NewLine + ex.StackTrace)
                 : (ex.Message + " --> " + ex.InnerException.GetFullMessageStackTrace());
        }
    }
}
