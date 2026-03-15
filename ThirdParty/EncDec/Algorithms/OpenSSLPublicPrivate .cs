using IndeedClone.ThirdParty.EncDec.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace IndeedClone.ThirdParty.EncDec.Algorithms
{
    public class OpenSSLPublicPrivate : IDecryptable
    {
        private string _publicKey;
        private string _privateKey;


        public OpenSSLPublicPrivate(string publicKey = null, string privateKey = null)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;
        }

        public string Encrypt(string input, Dictionary<string, object> param = null)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(_publicKey);
                byte[] data = Encoding.UTF8.GetBytes(input);
                byte[] encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
                string encrypted = Convert.ToBase64String(encryptedData);
                return MakeUrlSafe(encrypted);
            }
        }

        public string Decrypt(string input, Dictionary<string, object> param = null)
        {
            string restored = RestoreFromUrlSafe(input);
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(_privateKey);
                byte[] data = Convert.FromBase64String(restored);
                byte[] decryptedData = rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }


       /************************************** Private SRP Methods *************************************/


        public string GetPublicEncryptKey() => _publicKey;
        public string GetPrivateDecryptKey() => _privateKey;

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
