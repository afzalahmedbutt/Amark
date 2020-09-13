using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class ConfirmOrderResponse
    {
        public ConfirmOrderResponse()
        {
            IsSuccess = true;
            Warnings = new List<String>();
        }
        public bool IsSuccess { get; set; }
        public IList<string> Warnings { get; set; }
    }
}
