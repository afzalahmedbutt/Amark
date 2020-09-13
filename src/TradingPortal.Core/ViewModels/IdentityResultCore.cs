using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class IdentityResultCore
    {
        public IdentityResultCore()
        {
            Errors = new List<string>();
        }
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
    }
}
