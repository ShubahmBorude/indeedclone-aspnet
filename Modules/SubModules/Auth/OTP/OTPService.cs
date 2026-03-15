

namespace IndeedClone.Modules.SubModules.Auth.OTP
{
    public class OTPService : IOTPService
    {
        public string GenerateOTP()
        {
            return OTP.Generate();
        }

        public string HashOTP(string otp)
        {
            return OTPHasher.Hash(otp);
        }

        public bool VerifyOTP(string providedOtp, string storedOtpHash)
        {
            return OTPHasher.Verify(providedOtp, storedOtpHash);
        }
    }
}
