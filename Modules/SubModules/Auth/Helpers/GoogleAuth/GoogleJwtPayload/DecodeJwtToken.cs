using System.Text;
using System.Text.Json;

namespace IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth.GoogleJwtPayload
{
    public static class DecodeJwtToken
    {
        public static async Task<GoogleJwtPayload?> DecodeGoogleToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            // # JWT format: header.payload.signature
            var parts = token.Split('.');
            if (parts.Length != 3)
                return null;

            var payload = parts[1];

            // # Base64 decode (Google uses Base64Url)
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var bytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(bytes);

            // # Deserialize to GoogleJwtPayload
            return JsonSerializer.Deserialize<GoogleJwtPayload>(json);
        }
    }
}
