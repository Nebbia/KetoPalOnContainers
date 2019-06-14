using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using KetoPal.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KetoPal.Identity.Services
{
    public class EFLoginService : ILoginService<ApplicationUser>
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public EFLoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindByUsername(string user)
        {
            return await _userManager.FindByNameAsync(user);
        }

        public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool rememberMe = false, bool enableLockout=true)
        {
            return await _signInManager.PasswordSignInAsync(user, password, rememberMe, enableLockout);
        }

        public Task SignIn(ApplicationUser user)
        {
            return _signInManager.SignInAsync(user, true);
        }

        public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod = null)
        {
            return _signInManager.SignInAsync(user, properties, authenticationMethod);
        }

        public Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user)
        {
            return _signInManager.CreateUserPrincipalAsync(user);
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);
        }

        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string userId)
        {
            return _signInManager.GetExternalLoginInfoAsync(userId);
        }

        public Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return _signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public Task<bool> IsTwoFactorClientRememberedAsync(ApplicationUser user)
        {
            return _signInManager.IsTwoFactorClientRememberedAsync(user);
        }

        public Task RefreshSignInAsync(ApplicationUser user)
        {
            return _signInManager.RefreshSignInAsync(user);
        }

        public Task ForgetTwoFactorClientAsync()
        {
            return _signInManager.ForgetTwoFactorClientAsync();
        }

        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }

        public Task<ApplicationUser> GetTwoFactorAuthenticationUserAsync()
        {
            return _signInManager.GetTwoFactorAuthenticationUserAsync();
        }

        public Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool rememberMe, bool rememberBrowser)
        {
            return _signInManager.TwoFactorSignInAsync(provider, code, rememberMe, rememberBrowser);
        }
    }
}
