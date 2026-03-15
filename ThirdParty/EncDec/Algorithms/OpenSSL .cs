using IndeedClone.ThirdParty.EncDec.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace IndeedClone.ThirdParty.EncDec.Algorithms
{
    public class OpenSSL : IDecryptable
    {
        private string _secretKey;
        private string _iv;


        public OpenSSL(string secretKey = null, string iv = null)
        {
            _secretKey = secretKey ?? "kGJeGF2hEQ";
            _iv = iv ?? "7236489512357891";
        }

        public string Encrypt(string input, Dictionary<string, object> param = null)
        {
            using (Aes aesAlg = Aes.Create())
            {
             // # Using SHA256.Create() instead of HashData()
                aesAlg.Key = GetSHA256Hash(_secretKey);
                aesAlg.IV = Encoding.UTF8.GetBytes(_iv).Take(16).ToArray();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                    }
                    string encrypted = Convert.ToBase64String(msEncrypt.ToArray());
                    return MakeUrlSafe(encrypted);
                }
            }
        }

        public string Decrypt(string input, Dictionary<string, object> param = null)
        {
            string restored = RestoreFromUrlSafe(input);
            byte[] cipherText = Convert.FromBase64String(restored);

            using (Aes aesAlg = Aes.Create())
            {
             // # Using SHA256.Create() instead of HashData()
                aesAlg.Key = GetSHA256Hash(_secretKey);
                aesAlg.IV = Encoding.UTF8.GetBytes(_iv).Take(16).ToArray();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }



        /******************************** Private SRP Methods **********************************/

        private byte[] GetSHA256Hash(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        private string MakeUrlSafe(string input)
        {
            return input.Replace('+', '-').Replace('/', '_').Replace('=', '~');
        }

        private string RestoreFromUrlSafe(string input)
        {
            return input.Replace('-', '+').Replace('_', '/').Replace('~', '=');
        }

    }
}
