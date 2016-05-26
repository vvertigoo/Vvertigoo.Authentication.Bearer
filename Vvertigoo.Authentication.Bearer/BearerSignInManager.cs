using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Vvertigoo.Authentication.Bearer
{
    /// <summary>
    /// Basic implementation of sign in manager for BearerAuthentication
    /// </summary>
    public class BearerSignInManager<TUser> where TUser : class
    {
        private readonly IUserClaimsPrincipalFactory<TUser> _claimsFactory;
        private readonly UserManager<TUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public BearerSignInManager(
            UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IHttpContextAccessor contextAccessor)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            if (claimsFactory == null)
            {
                throw new ArgumentNullException(nameof(claimsFactory));
            }
            if (contextAccessor == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Sign in user using his password
        /// </summary>
        public async Task<BearerSignInManagerResult> PasswordSignInAsync(TUser user, string password)
        {
            try
            {
                var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, password);
                if (!passwordIsCorrect) return BearerSignInManagerResult.WrongPassword;

                var principal = await _claimsFactory.CreateAsync(user);
                await _contextAccessor.HttpContext.Authentication.SignInAsync("Bearer", principal);
                return BearerSignInManagerResult.Success;
            }
            catch {
                return BearerSignInManagerResult.InternalError;
            }
        }
    }
}
