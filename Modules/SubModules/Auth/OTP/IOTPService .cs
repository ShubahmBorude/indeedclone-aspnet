

namespace IndeedClone.Modules.SubModules.Auth.OTP
{
    public interface IOTPService
    {
        string GenerateOTP();

        string HashOTP(string otp);

        bool VerifyOTP(string providedOtp, string storedOtpHash);
    }
}
