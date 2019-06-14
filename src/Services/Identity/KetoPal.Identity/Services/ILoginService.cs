using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using KetoPal.Identity.Models;

namespace KetoPal.Identity.Services
{
    public interface ILoginService<T>
    {
        Task<SignInResult> PasswordSignInAsync(T user, string password, bool rememberMe = false, bool enableLockout = true);
        Task<T> FindByUsername(string user);
        Task SignIn(T user);
        Task SignInAsync(T user, AuthenticationProperties properties, string authenticationMethod = null);

        Task<ClaimsPrincipal> CreateUserPrincipalAsync(T user);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string userId);
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task<bool> IsTwoFactorClientRememberedAsync(T user);
        Task RefreshSignInAsync(ApplicationUser user);
        Task ForgetTwoFactorClientAsync();
        Task SignOutAsync();
        Task<T> GetTwoFactorAuthenticationUserAsync();
        Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool rememberMe, bool rememberBrowser);
    }
}
