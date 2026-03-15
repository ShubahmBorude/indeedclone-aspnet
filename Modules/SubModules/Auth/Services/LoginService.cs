using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.Auth.DTO;
using IndeedClone.Modules.SubModules.Auth.Helpers.Validators;
using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.Auth.RepoContracts;
using IndeedClone.Modules.SubModules.Auth.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace IndeedClone.Modules.SubModules.Auth.Services
{
    public class LoginService : ILoginService
    {
        private readonly IRegisterRepository _userRepository;
       
        private readonly IPasswordHasher<RegisterModel> _passwordHasher;

        public LoginService(IRegisterRepository userRepository, IPasswordHasher<RegisterModel> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

     // # Email + Password login
        public async Task<AuthResponse> Login(string email, string password)
        {
         // # Always reset previous state
            ErrorError.Clear();

         // # Validation 
            if (!ValidateRegistrationInput(email, password))
                return new AuthResponse { Success = false };

         // # Fetch existing user by email
            var user = await GetUserDataByEmail(email);
            if(user == null)
                return new AuthResponse { Success = false };
         
         // # Verify password
            if (!VerifyPassword(user, password)) 
                return new AuthResponse { Success = false };
         // # Check email verification
            if (!IsEmailVerified(user))
                return new AuthResponse { Success = false, EmailVerified = false };


            return new AuthResponse
            {
                Success = true,
                UserId = user.Id,
                EmailVerified = true,
                Name = user.Name,
                RefNo = user.RefNo,
            };
        }

     // # Get user name 
        public async Task<string> UserName(string email)
        {
            var res = await GetUserDataByEmail(email);
            return res.Name;
        }



        /* ########################   Private SRP Methods  ########################*/


        private bool ValidateRegistrationInput(string email, string password)
        {
            AuthValidator.EmailValidator(email);
            AuthValidator.PasswordValidator(password);

            return  ErrorError.CheckError();
        }

        private async Task<RegisterModel?> GetUserDataByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) 
                ErrorError.SetError("Invalid email or password");
            return user;
        }

        private bool VerifyPassword(RegisterModel user, string password)
        {
            var match = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (match == PasswordVerificationResult.Failed) 
                ErrorError.SetError("Password do not match, Please enter correct password");
            return match != PasswordVerificationResult.Failed;
        }

        private bool IsEmailVerified(RegisterModel user)
        {
            if (!(user.IsEmailVerified)) 
                ErrorError.SetError("Please verify your email before logging in");

            return user.IsEmailVerified;
        }
    }
}
