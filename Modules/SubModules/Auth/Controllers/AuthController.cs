using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.Shared.Helper;
using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.Auth.ServiceContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;



namespace IndeedClone.Modules.SubModules.Auth.Controllers
{
    public class AuthController : Controller
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly ILoginService _loginService;
       
        public AuthController(IGoogleAuthService googleAuthService, ILoginService loginService)
        {
            _googleAuthService = googleAuthService;
            _loginService = loginService;
        }


    /*************************************************** Login Page ********************************************************/

        [HttpGet] 
        public async Task<IActionResult> Login()
        {
         // # Clear Previous errors
            ErrorError.Clear();
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Update(LoginModel login)
        {
         // # Clear Previous errors
            ErrorError.Clear();

         // # Login field credentials
            var result = await _loginService.Login(login.Email, login.Password);
            if (!result.Success)
                return Redirect(nameof(Login)) .WithErrors(this, ErrorError.GetErrors());

            await AuthSignInHelper.SignInAsync(HttpContext, result.RefNo, result.Email, result.Name);

            // # This is Success
            return RedirectToAction("Retrieve", "IndeedClone").With(() => TempData["Success"] = $" login successful. Welcome back, {result.Name}!");
        }


        /*************************************************** Sign in with Google : Login Page ********************************************************/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoogleSignIn([FromForm] string token)
        {
         // # Clear Previous errors 
            ErrorError.Clear();
         // # Determine state (Login or Register page)
            var referer = this.GetSafeReferer();

         // # Check token
            if (string.IsNullOrWhiteSpace(token))
                return Redirect(referer).With(() => ErrorError.SetError("Google token is missing"))
                    .WithErrors(this, ErrorError.GetErrors());

         // # Initiate Google Payload
            var result = await _googleAuthService.HandleSignIn(token);

         // # Check Errors
            if (!result.Success || !ErrorError.CheckError())
                return Redirect(referer).With(() => ErrorError.SetError(result.FailureReason ?? "Google sign-in failed"))
                            .WithErrors(this, ErrorError.GetErrors());

            await AuthSignInHelper.SignInAsync(HttpContext, result.RefNo, result.Email, result.Name, result.PictureUrl);

            var msg = !(result.IsNewUser) ?
                          $"Google login successful. Welcome back, {result.Name}!"
                              : $"Google login successful. New account has been created, {result.Name}!";

         // # This is Success
            return RedirectToAction("Retrieve", "IndeedClone").With(() => TempData["Success"] = msg );
        }


        /*************************************************** Logout : Login Page ********************************************************/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");

            ErrorError.Clear();

            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Retrieve", "IndeedClone");
        }

    }
}
