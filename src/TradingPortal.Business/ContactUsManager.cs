using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Domain.Amark;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Helpers;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Business
{
    public class ContactUsManager : IContactUsManager
    {
        private readonly IConfiguration _config;
        private readonly CustomSection _customSection;
        private readonly IEmailSender _emailSender;

        public ContactUsManager(IConfiguration config, IOptions<CustomSection> customSection, IEmailSender emailSender)
        {
            _config = config;
            _customSection = customSection.Value;
            _emailSender = emailSender;
        }
        public ContactFormData GetContactFormData()
        {
            ContactFormData contactFormData = new ContactFormData();
            contactFormData.EmailAliases = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Send To*", Value = "", Selected = true},
                    //new SelectListItem {Text = "Additional Information", Value = "additional_info"},
                    new SelectListItem {Text = "A-Mark Mining & Metals", Value = "amark_mining"},
                    //new SelectListItem {Text = "Contact Our President", Value = "contact_our_president"},
                    new SelectListItem {Text = "Human Resources", Value = "human_resources"},
                    new SelectListItem {Text = "Investor Relations", Value = "investor_relation"},
                    new SelectListItem {Text = "IRA Storage", Value = "ira_storage"},
                    new SelectListItem {Text = "Modify Customer Information", Value = "modify_customer_info"},
                    new SelectListItem {Text = "New Accounts", Value = "new_accounts"},
                    new SelectListItem {Text = "Opt out of mailing list", Value = "opt_out_of_mailing_list"},
                    new SelectListItem {Text = "Privacy", Value = "privacy"},
                    new SelectListItem {Text = "TDS service", Value = "tds_service"},
                    new SelectListItem {Text = "Trading Desk", Value = "trading_desk"},
                    new SelectListItem {Text = "Website Support", Value = "website_support"}

                };
            contactFormData.ContactMethods =
                new List<SelectListItem>
                    {
                         new SelectListItem {Text = "Preferred contact method*", Value = "",Selected = true},
                         new SelectListItem {Text = "Email", Value = "Email"},
                         new SelectListItem {Text = "Phone", Value = "Phone"},
                    };
            return contactFormData;
        }

        public bool SendCustomerMessage(ContactUsMessage contactUsMessage)
        {
            try
            {
                //_config.GetSection
                var sendMessage = _customSection.ContactUs["ContactUsMessageTemplate"];
                var subject = _customSection.ContactUs["ContactUsSubject"];
                sendMessage = sendMessage.Replace("{firstname}", contactUsMessage.FirstName.Trim())
                                             .Replace("{lastname}", contactUsMessage.LastName.Trim())
                                             .Replace("{preferredContactMethod}", contactUsMessage.PreferredContactMethod)
                                             .Replace("{emailphone}", contactUsMessage.PreferredContactMethod.ToLower() == "email" ? contactUsMessage.Email.Trim() : contactUsMessage.Phone.Trim())
                                             .Replace("{emailAlias}", contactUsMessage.SendToEmailAliasCode.Trim())
                                             .Replace("{message}", contactUsMessage.Message.Trim());
                SendToEmailAlias(contactUsMessage.SendToEmailAliasCode,subject,sendMessage);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void SendToEmailAlias(string contactMessageFor, string subject, string htmlBody)
        {
            //contactMessageFor will be mapped to app config keys and to email aliases/address. Be sure to add app keys for all send to recepients. ex: <add key="human_resources" value="hr@amark.com"/>
            var serviceEnvironment = _customSection.ContactUs["Environment"].ToString(); //<add key="Environment" value="dev" /> <!--dev, test, staging, prod -->
            var aliasEmail = _customSection.ContactUs[contactMessageFor.Trim()].ToString().Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var emailFrom = _customSection.ContactUs["EmailNotif-From"].ToString();
            subject = string.Format("{0} | [{1}]", subject, serviceEnvironment);
            string smtpServer = _customSection.ContactUs["EmailNotif-Smtp"];
            try
            {
                 _emailSender.Send(aliasEmail, emailFrom, subject, htmlBody, smtpServer, serviceEnvironment);
            }
            catch (Exception ex)
            {
                htmlBody = string.Format("{0}<br><br>Above is message from an Amark.com visitor.<br>Please forward to appropriate department.<br><br>There was an error sending to email alias: {1}", htmlBody, ex.GetFullMessageStackTrace());
                SendToAdmins(subject, emailFrom, htmlBody,smtpServer,serviceEnvironment);
            }

        }

        public void SendToAdmins(string subject,string emailFrom,string htmlBody,string smtpServer,string serviceEnvironment)
        {
            var adminEmails = _customSection.ContactUs["EmailNotif-ToAdmins"].ToString().Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            subject = string.Format("{0} | [{1}]", subject, serviceEnvironment);

            _emailSender.Send(adminEmails,emailFrom, subject, htmlBody,smtpServer,serviceEnvironment);

        }

    }

   

    public class CustomSection
    {
        public Dictionary<string, string> ContactUs { get; set; }
        public Dictionary<string, string> PortalClose { get; set; }
    }
}
