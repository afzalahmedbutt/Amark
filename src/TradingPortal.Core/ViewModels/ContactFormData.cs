using System;
using System.Collections.Generic;
using System.Text;

namespace TradingPortal.Core.ViewModels
{
    public class ContactFormData
    {
        public List<SelectListItem> EmailAliases { get; set; }
        public List<SelectListItem> ContactMethods { get; set; }
    }
}
