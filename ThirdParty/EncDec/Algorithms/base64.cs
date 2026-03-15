using IndeedClone.ThirdParty.EncDec.Contracts;
using System.Text;

namespace IndeedClone.ThirdParty.EncDec.Algorithms
{
    public class base64 : IDecryptable
    {
        public string Encrypt(string input, Dictionary<string, object> param = null)
        {
            string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
            return MakeUrlSafe(base64String);
        }

        public string Decrypt(string input, Dictionary<string, object> param = null)
        {
            string restored = RestoreFromUrlSafe(input);
            return Encoding.UTF8.GetString(Convert.FromBase64String(restored));
        }



        /********************************** Private SRP Methods ********************************/


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
