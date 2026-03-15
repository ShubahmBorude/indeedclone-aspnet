using IndeedClone.ThirdParty.EncDec.Contracts;
using System.Text;

namespace IndeedClone.ThirdParty.EncDec.Algorithms
{
    public class SHA256 : INonDecryptable
    {
        public string Encrypt(string input, Dictionary<string, object> param = null)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

             // # Convert byte array to hex string
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
