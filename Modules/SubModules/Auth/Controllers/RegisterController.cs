using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.Shared.Helper;
using IndeedClone.Modules.SubModules.Auth.DTO;
using IndeedClone.Modules.SubModules.Auth.ServiceContracts;
using IndeedClone.ThirdParty.EncDec;
using Microsoft.AspNetCore.Mvc;



namespace IndeedClone.Modules.SubModules.Auth.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;

     // # Inject RegisterService via constructor (DI)
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

 
        /************************************************** Register Page *******************************************************/

        [HttpGet]
        public async Task<IActionResult> Retrieve() => View("Register");


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string name, string email, string password, string confirmPassword, bool status)
        {
         // # Clear Previous state
            ErrorError.Clear();

         // # Initiate & Validate Fields & Send email
            await _registerService.Register(name, email, password, confirmPassword, status);

         // # Check errors 
            if (!ErrorError.CheckError())
                return Redirect(nameof(Retrieve)).WithErrors(this, ErrorError.GetErrors());

         // # Get Service class response
            var response = ErrorError.GetReturnData<AuthResponse>();
         // # Encryt refNo credentials 
            var encryptedRefNo = EncDec.OpenSSL().Encrypt(response.RefNo);

         // # This is sucess & Redirect with token  
            return RedirectToAction(nameof(VerifyEmail), new { refNo = encryptedRefNo }).With( () => TempData["Success"] = ErrorError.GetSuccess());
        }



        /************************************************** Verify Email Page *******************************************************/


        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string refNo)
        {
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction(nameof(Retrieve));

         // # Decrypt the credentials
            var decryptedRefNo = EncDec.OpenSSL().Decrypt(refNo);
            if(string.IsNullOrEmpty(decryptedRefNo))
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Unable to identify user credentials."))
                        .WithErrors(this, ErrorError.GetErrors());

            var user = await _registerService.UserExistsAsync(decryptedRefNo);
            if(user == null)
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Unable to identify user."))
                        .WithErrors(this, ErrorError.GetErrors());

            return View("VerifyEmailViaOTP", new VerifyEmailViewDTO { EncryptedRefNo = refNo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmailViaOTP(string otp, string refNo)
        {
         // # Clear Previous state
            ErrorError.Clear();

            var decryptedRefNo = EncDec.OpenSSL().Decrypt(refNo);

            if (string.IsNullOrEmpty(decryptedRefNo))
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Unable to identify user credentials."))
                        .WithErrors(this, ErrorError.GetErrors());

            await _registerService.VerifyEmailAsync(otp, decryptedRefNo);
            if (!ErrorError.CheckError())
                return RedirectToAction(nameof(VerifyEmail), new { refNo });

         // # Get Service class response
            var response = ErrorError.GetReturnData<AuthResponse>();
            if(response == null)
                return RedirectToAction(nameof(VerifyEmail), new { refNo }).With(() => ErrorError.SetError("Oops! something went wrong!"));

            await AuthSignInHelper.SignInAsync(HttpContext, response.RefNo, response.Email, response.Name);

            return RedirectToAction("Retrieve", "IndeedClone").With(() => TempData["Success"] = ErrorError.GetSuccess());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendOtp(string refNo)
        {
         // # Clear Previous state
            ErrorError.Clear();

            var decryptedRefNo = EncDec.OpenSSL().Decrypt(refNo);
            if (string.IsNullOrEmpty(decryptedRefNo))
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Unable to identify user credentials."))
                        .WithErrors(this, ErrorError.GetErrors());

            await _registerService.ResendOtpAsync(decryptedRefNo);

            return RedirectToAction(nameof(VerifyEmail), new { refNo });
        }

    }
}







/*
 * 
  // # Handle any errors from service
            if (!ErrorError.CheckError())
            {
             // # Fetch user for cooldown / block handling
                var user = _registerService.GetUserByToken(token);
                if (user != null)
                {
                 // # Temporary block → stay on VerifyEmail page
                    if (user.Status == AccountStatus.TEMPORARY_BLOCK)
                        return View("VerifyEmail")
                                   .With(() => ErrorError.SetError("Too many failed attempts. Verification temporarily blocked for 1 hour."));
                }

                // # Token invalid / expired / already verified → redirect home with error
                return RedirectToAction(nameof(VerifyEmail), new { token })
                    .With(() => ErrorError.SetError(ErrorError.GetErrors().FirstOrDefault()
                             ?? "Email verification failed. Please request a new verification email."))
                                     .WithErrors(this, ErrorError.GetErrors());
            }
 * 
 */