using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    public class ContactUsController : Controller
    {
        private readonly IContactUsManager _contactUsManager;
        public ContactUsController(IContactUsManager contactUsManager)
        {
            _contactUsManager = contactUsManager;
        }

        [HttpPost("sendcustomermessage")]
        public bool SendCustomerMessage([FromBody]ContactUsMessage contactUsMessage)
        {
            return _contactUsManager.SendCustomerMessage(contactUsMessage);
           
        }

        public ContactFormData GetContactFormData()
        {
            return _contactUsManager.GetContactFormData();
        }
    }
}