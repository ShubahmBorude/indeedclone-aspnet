using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth
{
    public static class GoogleAuthHelper
    {
        public static async Task SignInUser(HttpContext httpContext, int UserId, string name, string email, bool rememberMe = false, string refNo = "")
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, refNo),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Email, email)
        };

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync("AuthCookie", principal);
        }
    }
}
