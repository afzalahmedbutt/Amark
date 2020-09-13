using Microsoft.AspNetCore.Identity;
using System;
using TradingPortal.Infrastructure.Services.Interfaces;


namespace TradingPortal.Infrastructure.IdentityExtensions
{
    public class CustomPasswordHasher<TUser> : PasswordHasher<TUser> where TUser : class
    {
        private readonly IEncryptionService _encryptionService;
        public CustomPasswordHasher(IEncryptionService encryptionService)
        {
            this._encryptionService = encryptionService;

            
        }
        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {

            Type type = user.GetType();
            var prop = type.GetProperty("PasswordSalt");
            var passwordSaltValue = prop.GetValue(user);
            if (passwordSaltValue != null)
            {
                var customeHashedPassword = this._encryptionService.CreatePasswordHash(providedPassword, passwordSaltValue.ToString());
                if (customeHashedPassword == hashedPassword)
                {
                    return PasswordVerificationResult.Success;
                }
                //else
                //{
                //    return PasswordVerificationResult.Failed;
                //}
            }
            return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }

        public override string HashPassword(TUser user, string password)
        {
            Type type = user.GetType();
            var prop = type.GetProperty("PasswordSalt");
            var passwordSaltValue = prop.GetValue(user);
            if(passwordSaltValue != null)
            {
                return _encryptionService.CreatePasswordHash(password, passwordSaltValue.ToString());
            }
            else
            {
                return base.HashPassword(user,password);
            }

        }

     }

    
}
