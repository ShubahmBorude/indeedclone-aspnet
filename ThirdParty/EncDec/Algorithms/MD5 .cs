using IndeedClone.ThirdParty.EncDec.Contracts;
using System.Text;

namespace IndeedClone.ThirdParty.EncDec.Algorithms
{
    public class MD5 : INonDecryptable
    {
        public string Encrypt(string input, Dictionary<string, object> param = null)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

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
