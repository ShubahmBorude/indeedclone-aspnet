namespace IndeedClone.Modules.SubModules.Auth.DTO
{
    public class AuthResponse
    {
     // # Common
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string FailureReason { get; set; } = string.Empty;
        public string RefNo { get; set; } = string.Empty;

        // # User
        public int UserId { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

     // # Flow specific
        public bool IsNewUser { get; set; } = false;
        public bool EmailVerified { get; set; } = false;
        public string Token { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;

        // # Google
        public string PictureUrl { get; set; } = string.Empty;


    }
}
