namespace IndeedClone.Modules.SubModules.Auth.OTP
{
    public class OTPPolicy
    {
        public const int Length = 4;
        public const int ExpiryMinutes = 15;
        public const int MaxAttempts = 3;
        public const int BlockMinutes = 60;
    }
}
