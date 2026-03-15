using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.Auth.DTO;
using IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth.GoogleJwtPayload;
using IndeedClone.Modules.SubModules.Auth.RepoContracts;
using IndeedClone.Modules.SubModules.Auth.ServiceContracts;

namespace IndeedClone.Modules.SubModules.Auth.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IGoogleAuthRepository _googleAuthRepository;

        public GoogleAuthService(IRegisterRepository registerRepository, IGoogleAuthRepository googleAuthRepository)
        {
            _registerRepository = registerRepository;
            _googleAuthRepository = googleAuthRepository;
        }

        public async Task<AuthResponse> HandleSignIn(string token)
        {
         // # Decode Google token
            var payload = await DecodeJwtToken.DecodeGoogleToken(token);

            if (payload == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    FailureReason = "Invalid Google token"
                };
            }

         // # email verification
            if (!payload.EmailVerified)
            {
                return new AuthResponse
                {
                    Success = false,
                    FailureReason = "Google email not verified"
                };
            }

         // # users table
            var user = await _registerRepository.GetUserByEmailAsync(payload.Email);

            if (user == null)
            {
                user = await _registerRepository.CreateUserFromGooglePayload(payload);
            }

         // # GOOGLE AUTH (child table)
            var googleAuth =  await _googleAuthRepository.GetByProviderUserIdAsync(payload.Sub);

            if (googleAuth != null)
            {
                if (googleAuth.Status != GoogleAuthStatus.ACTIVE)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        FailureReason = $"Google account is {googleAuth.Status}"
                    };
                }
            }
            else
            {
                googleAuth = await _googleAuthRepository.CreateFromPayloadAsync(payload, user.RefNo, GoogleAuthStatus.ACTIVE);
            }

        // # SUCCESS RESPONSE (FROM USERS)
            return new AuthResponse
            {
                Success = true,
                IsNewUser = false,
                UserId = googleAuth.Id,
                RefNo = user.RefNo,
                Email = user.Email,
                Name = user.Name,
                PictureUrl = payload.Picture
            };
        }
    }
}
