using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Infrastructure.DatabaseContexts;

namespace TradingPortal.Infrastructure.Repositories
{
    //public class CustomUserStore : UserStore<Customer, Role, ApplicationDbContext, int>
    public class CustomUserStore : UserStore<Customer, Role, ApplicationDbContext, int, IdentityUserClaim<int>, CustomerRole, IdentityUserLogin<int>,
        IdentityUserToken<int>,IdentityRoleClaim<int>>
    {

        private readonly ApplicationDbContext _context;
        public CustomUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
            _context = context;
        }



        public override Task<Customer> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName || (u.Email != null && u.Email.ToUpper() == normalizedUserName), cancellationToken);
        }

        /// <summary>
        /// Gets the user, if any, associated with the specified, normalized email address.
        /// </summary>
        /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
        /// </returns>
        public override Task<Customer> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail || (u.Email != null && u.Email.ToUpper() == normalizedEmail), cancellationToken);
        }

        public override Task<string> GetSecurityStampAsync(Customer user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!String.IsNullOrEmpty(user.PasswordSalt))
            {
                return Task.FromResult("NonIdentity User");
            }
            return base.GetSecurityStampAsync(user, cancellationToken);
        }

        /// <summary>
        /// Return a role with the normalized name if it exists.
        /// </summary>
        /// <param name="normalizedRoleName">The normalized role name.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The role if it exists.</returns>
        protected override Task<Role> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            
            return _context.Roles.SingleOrDefaultAsync(r => (r.NormalizedName == normalizedRoleName || r.Name == normalizedRoleName), cancellationToken);
        }

        public override Task<string> GetUserNameAsync(Customer user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!String.IsNullOrEmpty(user.UserName))
            {
                return Task.FromResult(user.UserName);
            }
            else
            {
                return Task.FromResult(user.Email);
            }

        }
    }
}
