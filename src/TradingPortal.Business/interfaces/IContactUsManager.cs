using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Business.interfaces
{
    public interface IContactUsManager
    {
        ContactFormData GetContactFormData();
        bool SendCustomerMessage(ContactUsMessage contactUsMessage);
    }
}
