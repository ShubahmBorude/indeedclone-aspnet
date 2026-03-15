using IndeedClone.Modules.Shared.DateFormat;
using System.Security.Cryptography;
using System.Text;

namespace IndeedClone.Modules.SubModules.JobPost.JobUid
{
    public static class JobUidGenerator
    {
    // # Private class-level constants / variables
        private const string Prefix = "JOB";
        private const string RandomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int RandomLength = 32;
        private static readonly string TimestampFormat = "ddMMyyyy-HHmmss";

     // # Generates a globally unique JobUid
     // # Format: JOB-ddMMyyyy-HHmmss-XXXXXX
     // # Ex -> JOB-28012026-173000-7XQ9MZK4L2ABCD8EFGH1IJRSTUVWXY0
        public static string Generate()
        {
            string timestamp = DateHelper.IST_Date().ToString(TimestampFormat);
            string random = GenerateRandomString(RandomLength);
            return $"{Prefix}-{timestamp}-{random}";
        }

     // # Generates secure random alphanumeric string
        private static string GenerateRandomString(int length)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            var result = new StringBuilder(length);
            foreach (var b in bytes)
                result.Append(RandomChars[b % RandomChars.Length]);

            return result.ToString();
        }
    }
}
