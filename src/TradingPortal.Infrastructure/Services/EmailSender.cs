using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TradingPortal.Core.Domain;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        
        public EmailSender()
        {
            
        }
        
        public Task SendEmailAsync(MessageTemplate emailTemplate, string subject, EmailAccount emailAccount, string[] emailsTo,string[] emailsCC = null)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailAccount.Email, emailAccount.DisplayName);
            foreach (var email in emailsTo)
            {
                mailMessage.To.Add(email);
            }

            if (emailTemplate.BccEmailAddresses != null)
            {

            }
            if (emailsCC != null && emailsCC.Length > 0)
            {
                foreach (var email in emailsCC)
                {
                    mailMessage.CC.Add(email);
                }

            }


            mailMessage.Subject = emailTemplate.Subject;
            mailMessage.Body = emailTemplate.Body;
            mailMessage.IsBodyHtml = true;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                if (emailAccount.UseDefaultCredentials)
                    smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
                else
                    smtpClient.Credentials = new System.Net.NetworkCredential(emailAccount.Email, emailAccount.Password);

                smtpClient.Send(mailMessage);
            }
            return Task.CompletedTask;
        }

        public void Send(List<string> emailsTo,string emailFrom, string subject, string htmlBody,string smtpServer,string serviceEnvironment, List<string> emailsCc = null, List<string> emailsBcc = null)
        {
            try
            {
                
                //create the mail message        
                MailMessage mail = new MailMessage();
               
                mail.From = new MailAddress(emailFrom);
                //create a loop over of emailsTo list
                foreach (var email in emailsTo)
                {
                    mail.To.Add(email);
                }


                //mail.To.Add("emailInList@email.com");
                //set the content        
                mail.Subject = subject;

                htmlBody = string.Format("{0}{1}{1}{1}<p font-size='9px'>[This is an automated email. Please do not reply.]</p>{1}{1}---------{1}Env: {2}-{3}", htmlBody, "<br/>", serviceEnvironment, Environment.MachineName);

                mail.Body = htmlBody;
                mail.IsBodyHtml = true;
                //send the message         
                SmtpClient smtp = new SmtpClient(smtpServer);
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                var e = ex;
                //logger.Error(ex);  will be good to add NLog or Elmah - save to db              
            }
        }

    }
}
