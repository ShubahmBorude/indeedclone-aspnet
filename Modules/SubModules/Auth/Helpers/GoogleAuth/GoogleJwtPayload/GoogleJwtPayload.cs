using System.Text.Json.Serialization;

namespace IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth.GoogleJwtPayload
{
    public class GoogleJwtPayload
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; } = string.Empty; // Google user_id

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("picture")]
        public string Picture { get; set; } = string.Empty;

        [JsonPropertyName("iss")]
        public string Iss { get; set; } = string.Empty;
    }
}
