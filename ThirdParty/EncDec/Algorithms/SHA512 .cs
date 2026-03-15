using IndeedClone.ThirdParty.EncDec.Contracts;
using System.Text;

namespace IndeedClone.ThirdParty.EncDec.Algorithms
{
    public class SHA512 : INonDecryptable
    {
        public string Encrypt(string input, Dictionary<string, object> param = null)
        {
            using (var sha512 = System.Security.Cryptography.SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                // Convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
