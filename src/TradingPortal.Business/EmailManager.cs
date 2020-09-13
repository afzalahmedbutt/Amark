using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Domain;

namespace TradingPortal.Business
{
    public class EmailManager : IEmailManager
    {
        IRepository<EmailAccount> _emailAccountRepository;
        public EmailManager(IRepository<EmailAccount> emailAccountRepository)
        {
            _emailAccountRepository = emailAccountRepository;
        }
        public EmailAccount GetEmailAccountById(int id)
        {
            return _emailAccountRepository.GetById(id);
        }
    }
}
