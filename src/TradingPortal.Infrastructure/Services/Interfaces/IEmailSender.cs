using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain;

namespace TradingPortal.Infrastructure.Services.Interfaces
{
    public interface IEmailSender
    {
        //Task SendEmailAsync(string email, string subject, string msg,EmailAccount emailAccount);

        //Task SendEmailAsync(MessageTemplate emailTemplate, string email, string subject, EmailAccount emailAccount);
        //Task  SendEmailAsync(MessageTemplate emailTemplate, List<string> emailsTo, string subject, EmailAccount emailAccount);
        Task SendEmailAsync(MessageTemplate emailTemplate, string subject, EmailAccount emailAccount, string[] emailsTo, string[] emailsCC = null);

        void Send(List<string> emailsTo, string emailFrom, string subject, string htmlBody, string smtpServer, string serviceEnvironment, List<string> emailsCc = null, List<string> emailsBcc = null);
    }
}
