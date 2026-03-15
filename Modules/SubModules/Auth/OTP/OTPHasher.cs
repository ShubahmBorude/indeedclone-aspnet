using IndeedClone.ThirdParty.EncDec;
using System.Security.Cryptography;
using System.Text;

namespace IndeedClone.Modules.SubModules.Auth.OTP
{
    public class OTPHasher
    {
        public static string Hash(string otp)
        {
            return EncDec.SHA256().Encrypt(otp);
        }

        public static bool Verify(string providedOtp, string storedOtpHash)
        {
            if (string.IsNullOrEmpty(providedOtp) || string.IsNullOrEmpty(storedOtpHash))
                return false;

            var providedHash = OTPHasher.Hash(providedOtp);
            return SecureCompare(providedHash, storedOtpHash);
        }
        private static bool SecureCompare(string a, string b)
        {
            var bytesA = Encoding.UTF8.GetBytes(a);
            var bytesB = Encoding.UTF8.GetBytes(b);

            if (bytesA.Length != bytesB.Length)
                return false;

            return CryptographicOperations.FixedTimeEquals(bytesA, bytesB);
        }
    }

}
