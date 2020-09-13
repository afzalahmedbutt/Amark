using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Business
{
    public class CustomUserManager : UserManager<Customer>
    {
        public CustomUserManager(IUserStore<Customer> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<Customer> passwordHasher, IEnumerable<IUserValidator<Customer>> userValidators, IEnumerable<IPasswordValidator<Customer>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<Customer>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }
    }
}
