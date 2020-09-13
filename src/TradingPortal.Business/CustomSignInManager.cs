using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Infrastructure.DatabaseContexts;

namespace TradingPortal.Business
{
    public class CustomSignInManager<TUser> : SignInManager<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomSignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, ApplicationDbContext dbContext)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _db = dbContext;

        }

        public override async Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            var appUser = user as IdentityUser;
            return result;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
            return result;
        }

        public override async Task SignOutAsync()
        {
            await base.SignOutAsync();
        }


    }
}
