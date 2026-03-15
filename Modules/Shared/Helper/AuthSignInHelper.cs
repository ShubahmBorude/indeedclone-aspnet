using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace IndeedClone.Modules.Shared.Helper
{
    public static class AuthSignInHelper
    {
        public static async Task SignInAsync(HttpContext httpContext, string refNo, string email, string name, string? pictureUrl = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim("RefNo", refNo) 
            };

            if (!string.IsNullOrWhiteSpace(pictureUrl))
                claims.Add(new Claim("Picture", pictureUrl));

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync("AuthCookie", principal, new AuthenticationProperties { IsPersistent = true });
        }
    }
}
