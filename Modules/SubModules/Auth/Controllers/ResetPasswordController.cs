using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.SubModules.Auth.DTO;
using IndeedClone.Modules.SubModules.Auth.ServiceContracts;
using IndeedClone.ThirdParty.EncDec;
using Microsoft.AspNetCore.Mvc;


namespace IndeedClone.Modules.SubModules.Auth.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IResetPasswordService _resetPasswordService;

        public ResetPasswordController(IResetPasswordService resetPasswordService)
        {
            _resetPasswordService = resetPasswordService;
        }


        /*************************************************** Reset Password : Login Page ********************************************************/


        [HttpGet]
        public IActionResult Retrieve()
        {
            ErrorError.Clear();
            return View("ResetPassword");   // here email page
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            ErrorError.Clear();
            await _resetPasswordService.RequestResetAsync(email);

            if (!ErrorError.CheckError())
                return RedirectToAction(nameof(Retrieve))
                    .WithErrors(this, ErrorError.GetErrors());

            var data = ErrorError.GetReturnData<AuthResponse>();
         // # Encryt refNo credentials 
            var encryptedRefNo = EncDec.OpenSSL().Encrypt(data.RefNo);

            return RedirectToAction(nameof(VerifyResetOtp), new { refNo = encryptedRefNo });
        }

        [HttpGet]
        public IActionResult VerifyResetOtp(string refNo)
        {
            ErrorError.Clear();

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction(nameof(Retrieve));

            return View("VerifyEmailViaOTP", new VerifyEmailViewDTO { EncryptedRefNo = refNo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyResetOTP(string refNo, string otp)
        {
            ErrorError.Clear();

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction(nameof(Retrieve));

         // # Decrypt the credentials
            var decryptedRefNo = EncDec.OpenSSL().Decrypt(refNo);
            if (string.IsNullOrEmpty(decryptedRefNo))
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Unable to identify user credentials."))
                        .WithErrors(this, ErrorError.GetErrors());

            await _resetPasswordService.VerifyOtpAsync(decryptedRefNo, otp);

            if (!ErrorError.CheckError())
                return RedirectToAction(nameof(VerifyResetOtp),new { refNo })
                    .WithErrors(this, ErrorError.GetErrors());

            HttpContext.Session.SetString("ResetAuthorized", decryptedRefNo);

            return RedirectToAction(nameof(SetNewPassword), new { refNo });
        }

        [HttpGet]
        public async Task<IActionResult> SetNewPassword(string refNo)
        {
            ErrorError.Clear();

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction(nameof(Retrieve));

         // # Decrypt the credentials
            var decryptedRefNo = EncDec.OpenSSL().Decrypt(refNo);
            if (string.IsNullOrEmpty(decryptedRefNo))
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Unable to identify user credentials."))
                        .WithErrors(this, ErrorError.GetErrors());

            var sessionRef = HttpContext.Session.GetString("ResetAuthorized");

            if (sessionRef != decryptedRefNo)
                return RedirectToAction(nameof(Retrieve)).With(() => ErrorError.SetError("Unauthorized access."));

            return View("ResetPasswordConfirm", new VerifyEmailViewDTO { EncryptedRefNo = refNo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNewPassword(string refNo, string password, string confirmPassword)
        {
            ErrorError.Clear();

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction(nameof(Retrieve));

         // # Decrypt the credentials
            var decryptedRefNo = EncDec.OpenSSL().Decrypt(refNo);
            if (string.IsNullOrEmpty(decryptedRefNo))
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Unable to identify user credentials."))
                        .WithErrors(this, ErrorError.GetErrors());

            var sessionRef = HttpContext.Session.GetString("ResetAuthorized");
            if (sessionRef != decryptedRefNo)
                return RedirectToAction(nameof(Retrieve)).With(()=> ErrorError.SetError("Unauthorized access."));

            await _resetPasswordService.ResetPasswordAsync(decryptedRefNo, password, confirmPassword);

            if (!ErrorError.CheckError())
                return RedirectToAction(nameof(SetNewPassword), new { refNo }).WithErrors(this, ErrorError.GetErrors());

            HttpContext.Session.Remove("ResetAuthorized");

            return RedirectToAction("Login", "Auth").With(()=> ErrorError.GetSuccess());
        }


    }
}
