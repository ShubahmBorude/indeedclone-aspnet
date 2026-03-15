using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.RefNo;
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
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _repository;
        private readonly IOTPService _otpService;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IAuthEmailService _emailService;

        public RegisterService(IRegisterRepository repository, IOTPService otpService, IPasswordHasher<string> passwordHasher, IAuthEmailService emailService)
        {
            _repository = repository;
            _otpService = otpService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        // ---------------- Register User ----------------
        public async Task Register(string name, string email, string password, string confirmPassword, bool status)
        {
            ErrorError.Clear();

         // # Validate input
            if(!ValidateRegistrationInput(name, email, password, confirmPassword, status))
                return;

         // # Check if email exists
            if (await _repository.EmailExist(email))
            {
                ErrorError.SetError("Email already registered.");
                return;
            }

         // # Create user 
            var user = CreateUserEntity(name, email, password, status);

            var rawOTP =  _otpService.GenerateOTP();
            var hashedOTP = _otpService.HashOTP(rawOTP);
            user.OTP = hashedOTP;
            user.VerificationOTPExpiry = DateTime.UtcNow.AddMinutes(15);

         // # Save 
            await _repository.CreateUserAsync(user);
         // # Send OTP via email to user
            await _emailService.SendVerificationEmailAsync(user.Email, user.Name, rawOTP);

         // # Set success response
            ErrorError.SetReturnData(new AuthResponse
            {
                Success = true,
                Email = user.Email,
                Name = user.Name,
                RefNo = user.RefNo
            });

            ErrorError.SetSuccess("Registration successful.");
        }

        public async Task<RegisterModel?> UserExistsAsync(string refNo)
        {
            return await _repository.GetUserByRefNO(refNo);
        }

        public async Task VerifyEmailAsync(string otp, string refNo)
        {
            ErrorError.Clear();

            var user = await _repository.GetUserByRefNO(refNo);

            if (user == null)
            {
                ErrorError.SetError("Invalid verification request.");
                return;
            }

            if (user.IsEmailVerified == true)
            {
                SetAuthResponse(user);
                return;
            }

            if (IsBlocked(user))
                return;

            if (IsExpired(user))
            {
                await IncreaseFailedAttemptAsync(user);
                return;
            }

            if (!_otpService.VerifyOTP(otp, user.OTP!))
            {
                await IncreaseFailedAttemptAsync(user);
                ErrorError.SetError("Invalid OTP.");
                return;
            }

            await ActivateUserAsync(user);

            SetAuthResponse(user);
        }

        public async Task ResendOtpAsync(string refNo)
        {
            ErrorError.Clear();

            var user = await _repository.GetUserByRefNO(refNo);

            if (user == null)
            {
                ErrorError.SetError("User not found.");
                return;
            }

            if (user.IsEmailVerified == true)
            {
                ErrorError.SetError("Email already verified.");
                return;
            }


         // # Generate OTP
            var rawOtp = _otpService.GenerateOTP();
            var hashedOtp = _otpService.HashOTP(rawOtp);

            user.OTP = hashedOtp;
            user.VerificationOTPExpiry = DateTime.UtcNow.AddMinutes(5);
            user.ResendOtpAt = DateTime.UtcNow.AddSeconds(30);
            user.Edited = DateHelper.IST_Date();

            await _repository.UpdateUserAsync(user);

         // # Resend Email
            await _emailService.SendVerificationEmailAsync(user.Email, user.Name, rawOtp);

            ErrorError.SetSuccess("A new verification OTP has been sent.");
        }


        /****************************************** Private Helpers *****************************************************/



        private bool ValidateRegistrationInput(string name, string email, string password, string confirmPassword, bool status)
        {
            AuthValidator.NameValidator(name);
            AuthValidator.EmailValidator(email);
            AuthValidator.PasswordValidator(password);
            AuthValidator.ConfirmPassword(password, confirmPassword);
            AuthValidator.Checkbox(status, "Terms & Conditions");

            return ErrorError.CheckError();
        }

        private RegisterModel CreateUserEntity(string name, string email, string password, bool status)
        {
            return new RegisterModel
            {
                RefNo = ReferenceNumber.GenerateRefNo(),
                Name = name,
                Email = email,
                Password = _passwordHasher.HashPassword(null, password),
                Status = AccountStatus.INACTIVE,
                IsEmailVerified = false,
                VerificationAttemptCount = 0,
                Created = DateHelper.IST_Date(),
                Edited = DateHelper.IST_Date()
            };
        }

        private async Task IncreaseFailedAttemptAsync(RegisterModel user)
        {
            user.VerificationAttemptCount++;

            if (user.VerificationAttemptCount >= 3)
            {
                user.BlockedUntil = DateTime.UtcNow.AddHours(1);
                user.Status = AccountStatus.TEMPORARY_BLOCK;
            }

            user.Edited = DateHelper.IST_Date();

            await _repository.UpdateUserAsync(user);
        }

        private async Task ActivateUserAsync(RegisterModel user)
        {
            user.IsEmailVerified = true;
            user.Status = AccountStatus.ACTIVE;

            //user.OTP = null;
            user.VerificationOTPExpiry = null;
            user.VerificationAttemptCount = 0;
            user.BlockedUntil = null;

            user.Edited = DateHelper.IST_Date();

            await _repository.UpdateUserAsync(user);

            ErrorError.SetSuccess("Email verified successfully.");
        }

        private void SetAuthResponse(RegisterModel user)
        {
            ErrorError.SetReturnData(
                new AuthResponse
                {
                    Success = true,
                    Email = user.Email,
                    Name = user.Name,
                    RefNo = user.RefNo
                });
        }

        private bool IsBlocked(RegisterModel user)
        {
            if (user.BlockedUntil.HasValue && user.BlockedUntil.Value > DateTime.UtcNow)
            {
                ErrorError.SetError($"Too many failed attempts. Try again after {user.BlockedUntil.Value:HH:mm}");
                return true;
            }

            return false;
        }

        private bool IsExpired(RegisterModel user)
        {
            if (!user.VerificationOTPExpiry.HasValue || user.VerificationOTPExpiry.Value < DateTime.UtcNow)
            {
                ErrorError.SetError("OTP expired.");
                return true;
            }

            return false;
        }
    }
}
