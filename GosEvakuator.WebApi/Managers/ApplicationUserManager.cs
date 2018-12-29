using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using GosEvakuator.WebApi.Options;
using GosEvakuator.WebApi.Models;

namespace GosEvakuator.WebApi.Managers
{
    public class ApplicationUserManager<T> : UserManager<ApplicationUser> where T: ApplicationUser
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async virtual Task<ApplicationUser> FindByNameOrCreateAsync(string name, string phoneNumber)
        {
            var user = await FindByNameAsync(name);
            if (user != null)
            {
                return user;
            }

            user = new ApplicationUser { UserName = name, PhoneNumber = phoneNumber, VerifiedCode = false };
            var identityResult = await CreateAsync(user);
            if (identityResult.Succeeded)
            {
                return user;
            }

            return null;
        }

        public virtual JwtSecurityToken GenerateToken(ApplicationUser user, ClaimsIdentity identity, string code)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return jwt;
        }

        public async virtual Task<string> GenerateCodeAsync(ApplicationUser user)
        {
            var r = new Random();
            var code = r.Next(1000, 9999);

            user.VerifyCode = code.ToString();

            user.VerifyCodeTime = DateTime.Now;
            user.VerifiedCode = false;

            var identityResult = await UpdateAsync(user);

            return user.VerifyCode;
        }

        public async virtual Task<bool> VerifyCodeAsync(ApplicationUser user, string code)
        {
            var result = false;
            try
            {
                if (user.VerifiedCode)
                {
                    return false;
                }

                if (user == null)
                {
                    return false;
                }

                if (!user.VerifyCode.Equals(code))
                {
                    return false;
                }

                user.VerifiedCode = true;
                await UpdateAsync(user);
                var now = DateTime.Now;
                var verifyTime = user.VerifyCodeTime.AddMinutes(10);

                result = DateTime.Compare(now, verifyTime) == -1;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
