using System.Security.Cryptography;

namespace IndeedClone.Modules.SubModules.Auth.OTP
{
    public static class OTP
    {
        private const string Allowed ="ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static string Generate()
        {
            var bytes = new byte[OTPPolicy.Length];
            RandomNumberGenerator.Fill(bytes);

            var chars = new char[OTPPolicy.Length];

            for (int i = 0; i < OTPPolicy.Length; i++)
                chars[i] = Allowed[bytes[i] % Allowed.Length];

            return new string(chars);
        }
    }
}
