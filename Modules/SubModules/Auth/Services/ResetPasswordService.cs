using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.Auth.DTO;
using IndeedClone.Modules.SubModules.Auth.Emails;
using IndeedClone.Modules.SubModules.Auth.Helpers.Validators;
using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.Auth.OTP;
using IndeedClone.Modules.SubModules.Auth.RepoContracts;
using IndeedClone.Modules.SubModules.Auth.ServiceContracts;
using Microsoft.AspNetCore.Identity;


namespace IndeedClone.Modules.SubModules.Auth.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IRegisterRepository _userRepository;
        private readonly IResetPasswordRepository _resetPasswordRepository;
        private readonly IOTPService _otpService;
        private readonly IAuthEmailService _emailService;
        private readonly IPasswordHasher<string> _passwordHasher;


        public ResetPasswordService(IRegisterRepository userRepository, IResetPasswordRepository resetPasswordRepository, IOTPService otpService,
                                    IAuthEmailService emailService, IPasswordHasher<string> passwordHasher)
        {
            _userRepository = userRepository;
            _resetPasswordRepository = resetPasswordRepository;
            _otpService = otpService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task RequestResetAsync(string email)
        {
            ErrorError.Clear();

            AuthValidator.EmailValidator(email);
            if (!ErrorError.CheckError())
                return;

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                ErrorError.SetSuccess("If account exists, OTP sent.");
                return;
            }

         // # Google user check
            if (string.IsNullOrEmpty(user.Password))
            {
                ErrorError.SetError("Account uses Google Sign-In.");
                return;
            }

            await _resetPasswordRepository.InvalidateActiveTokensAsync(user.RefNo);
            var rawOtp = _otpService.GenerateOTP();
            var hashedOtp = _otpService.HashOTP(rawOtp);

            await _resetPasswordRepository.CreateAsync(
                new ResetPasswordModel
                {
                    RefNo = user.RefNo,
                    OTP = hashedOtp,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(15)
                });

            await _emailService.SendVerificationEmailAsync(user.Email, user.Name, rawOtp);

            ErrorError.SetReturnData(new AuthResponse 
            {
                Success = true,
                RefNo = user.RefNo,
            });

            ErrorError.SetSuccess("Password reset OTP sent.");
        }

       

        public async Task VerifyOtpAsync(string refNo, string otp)
        {
            ErrorError.Clear();

            var token = await _resetPasswordRepository.GetActiveOtpAsync(refNo);
            if (token == null)
            {
                ErrorError.SetError("OTP expired.");
                return;
            }

            if (token.BlockedUntil.HasValue && token.BlockedUntil > DateTime.UtcNow)
            {
                ErrorError.SetError("Too many attempts. Try later.");
                return;
            }

            if (!_otpService.VerifyOTP(otp, token.OTP))
            {
                token.AttemptCount++;

                if (token.AttemptCount >= 5)
                    token.BlockedUntil =DateTime.UtcNow.AddMinutes(15);

                await _resetPasswordRepository.UpdateAsync(token);

                ErrorError.SetError("Invalid OTP.");
                return;
            }

            token.IsVerified = true;
            token.AttemptCount = 0;
            token.BlockedUntil = null;
            await _resetPasswordRepository.UpdateAsync(token);

            ErrorError.SetSuccess("OTP verified.");
        }

        public async Task ResetPasswordAsync(string refNo, string newPassword, string confirmPassword)
        {
            ErrorError.Clear();

            AuthValidator.PasswordValidator(newPassword);
            AuthValidator.PasswordValidator(confirmPassword);
            if (!ErrorError.CheckError())
                return;

            if (newPassword != confirmPassword) 
            { 
                ErrorError.SetError("Passwords mismatch."); 
                return; 
            }

            var token = await _resetPasswordRepository.GetActiveOtpAsync(refNo);
            if (token == null || !token.IsVerified || token.ExpiredAt < DateTime.UtcNow)
            {
                ErrorError.SetError("Unauthorized.");
                return;
            }

            var user = await _userRepository.GetUserByRefNO(refNo);
            if(user == null)
            {
                ErrorError.SetError("Unable to identify user.");
                return;
            }

            user.Password = _passwordHasher.HashPassword(null, newPassword);

            token.UsedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);
            await _resetPasswordRepository.UpdateAsync(token);

            ErrorError.SetSuccess("Password reset successful.");
        }

        public async Task ResendResetOtpAsync(string refNo)
        {
            ErrorError.Clear();

            var token = await _resetPasswordRepository.GetActiveOtpAsync(refNo);
            if (token == null)
            {
                ErrorError.SetError("Session expired.");
                return;
            }

            if (token.ResendAllowedAt > DateTime.UtcNow)
            {
                ErrorError.SetError("Please wait before requesting again.");
                return;
            }

            var user =  await _userRepository.GetUserByRefNO(refNo);
            var rawOtp = _otpService.GenerateOTP();
            token.OTP = _otpService.HashOTP(rawOtp);
            token.ExpiredAt = DateTime.UtcNow.AddMinutes(10);
            token.ResendAllowedAt = DateTime.UtcNow.AddSeconds(30);

            await _resetPasswordRepository.UpdateAsync(token);

            await _emailService.SendVerificationEmailAsync(user.Email, user.Name, rawOtp);

            ErrorError.SetSuccess("OTP resent.");
        }


        /***************************************** Private Helpers *********************************************/


      
    }
}
