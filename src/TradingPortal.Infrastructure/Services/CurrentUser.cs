using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Infrastructure.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationDbContext _dbContext;

        public CurrentUser(IHttpContextAccessor context, ApplicationDbContext dbContext)
        {
            _context = context;
            _dbContext = dbContext;
            

        }
        //public CurrentUser(IHttpContextAccessor context, UserManager<User> userManager)
        //{
        //    _context = context;
        //    _userManager = userManager;
        //    if (_usr == null)
        //        _usr = _userManager.GetUserAsync(_context.HttpContext.User).Result;
        //}
        
        public Customer User
        {
            get
            {
                return _dbContext.CurrentUser;
            }
        }

        public string GetCurrentIpAddress()
        {
            if(_context != null &&
                _context.HttpContext != null &&
                _context.HttpContext.Connection != null &&
                _context.HttpContext.Connection.RemoteIpAddress != null)
            return _context.HttpContext.Connection.RemoteIpAddress.ToString();

            return string.Empty;
        }

        public string GetAbsoluteUri()
        {
            return _context.HttpContext?.Request?.GetDisplayUrl();
        }


    }
}
