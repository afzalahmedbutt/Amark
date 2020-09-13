using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Services.Interfaces;
using TradingPortal.Infrastructure.UnitOfWork;

namespace TradingPortal.Business
{
    public class StoreManager : IStoreManager
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ISettingsService _settingsService;
        private readonly IEmailSender _emailSender;
        private readonly CustomSection _customSection;
        private readonly ICurrentUser _currentUser;
        private readonly CustomerAttributes _customerAttributes;
        public StoreManager(
            ISettingsService settingsservice,
            IOptions<CustomSection> customSection, 
            IEmailSender emailSender, 
            ICurrentUser currentUser,
            CustomerAttributes customerAttributes,
            IUnitOfWork unitOfWork)
        {
            _settingsService = settingsservice;
            _customSection = customSection.Value;
            _emailSender = emailSender;
            _currentUser = currentUser;
            _customerAttributes = customerAttributes;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SetStoreCloseStatus(bool isPortalClosed)
        {
            _settingsService.SetSetting<bool>("storeinformationsettings.storeclosed", isPortalClosed, 1, true);
            await _unitOfWork.SaveChangesAsync();

            var emailsTo = _customSection.PortalClose["EmailTo"].Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var emailsCc = _customSection.PortalClose["EmailCc"].Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            string subject = _customSection.PortalClose["Subject-ChangeStatus"].ToString();
            subject = string.Format("{0} {1}", subject, isPortalClosed ? "OFF" : "ON");
            string emailMsg = string.Format("{0} by {1} <br>Time:{2}", subject, _customerAttributes.FirstName+" "+_customerAttributes.LastName, DateTime.Now);
            MessageTemplate emailTemplate = new MessageTemplate();
            emailTemplate.Subject = subject;
            emailTemplate.Body = emailMsg;
            string emailFrom = _customSection.PortalClose["EmailNotif-From"];
            string smtpServer = _customSection.PortalClose["EmailNotif-Smtp"];
            string environment = _customSection.PortalClose["Environment"];
            //EmailAccount emailAccount = new EmailAccount();
            //emailAccount.Email = _customSection.PortalClose["EmailNotif-From"];
            //emailAccount.Password = "123";
            //emailAccount.DisplayName = "";
            //await _emailSender.SendEmailAsync(emailTemplate, subject, emailAccount, emailsTo, emailsCc);
            _emailSender.Send(emailsTo, emailFrom, subject, emailMsg, smtpServer, environment, emailsCc);
            return true;
        }
    }
}
