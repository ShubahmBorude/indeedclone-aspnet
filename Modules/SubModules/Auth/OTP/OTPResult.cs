namespace IndeedClone.Modules.SubModules.Auth.OTP
{
    public class OTPResult
    {
        public bool Success { get; set; }

        public bool IsExpired { get; set; }

        public bool IsBlocked { get; set; }

        public bool InvalidOtp { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
