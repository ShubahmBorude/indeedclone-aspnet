using IndeedClone.ThirdParty.EncDec.Algorithms;

namespace IndeedClone.ThirdParty.EncDec
{
    /* ######################################
     *  Encryption and Decryption Library V.1
     * ######################################
     * 
     * ## BASIC USAGE:
     *   1. Encrypt / Deccrypt data for URL parameters.
     *   2. EncDec.Algorithms().Encrypt (ALGORITHM GUIDE given below)
     *   3. Eg. Algorith: OpenSSL ==>   string encrypted = EncDec.OpenSSL().Encrypt("sensitive_data");
     *   4. Eg. Algorith: OpenSSL ==>   string decrypted = EncDec.OpenSSL().Decrypt(encrypted);
     * 
     * ## ALGORITHM GUIDE:
     *   1. OpenSSL() - AES-256-CBC encryption (Two-way, Secure).           {Best for: Sensitive data, URL parameters, secure storage}
     *   2. OpenSSLPublicPrivate() - RSA encryption (Two-way, Asymmetric).  {Best for: Secure communications, key exchange}
     *   3. SHA256() / SHA512() - Cryptographic hashing (One-way)           {Best for: Passwords, data integrity verification}
     *   4. MD5() - Fast hashing (One-way, Less secure)                     {Best for: Checksums, non-sensitive hashing}
     *   5. Base64() - Encoding (Two-way, Not encryption)                   {Best for: Localhost Demo Projects }
     *   
     * # EXAMPLE:  return RedirectToAction("Action", "Controller", new { id = EncDec.OpenSSL().Encrypt("12345")});
     * # NOTE: You can use Filters to Encrypt / Decrypt.
     * 
     * ## TROUBLESHOOTING:
     *   1. Invalid length error  ===>  Ensure IV is exactly 16 characters for OpenSSL
     *   2. URL parameters broken ===>  Library automatically handles URL-safe encoding
     *   3. Decryption fails      ===>  Use same algorithm instance for encrypt/decrypt
     *   
     *   
    */
    public class EncDec
    {
        private static Config _config = new Config();

     // # Set custom configuration / Configuration instance with settings
        public static void SetConfig(Config config)
        {
            _config = config;
        }

     // # Get OpenSSL encryption / decryption instance
     // ## Usage: EncDec.OpenSSL().Encrypt("your data")
        public static OpenSSL OpenSSL()
        {
            var settings = _config.GetSettings("OpenSSL");
            string secretKey = settings.ContainsKey("sec") ? settings["sec"] : "kGJeGF2hEQ";
            string iv = settings.ContainsKey("iv") ? settings["iv"] : "7236489512357891";
            return new OpenSSL(secretKey, iv);
        }

     // # Get OpenSSL Public/Private key encryption / decryption instance
        public static OpenSSLPublicPrivate OpenSSLPublicPrivate()
        {
            var settings = _config.GetSettings("OpenSSLPublicPrivate");
            string publicKey = settings.ContainsKey("public_key") ? settings["public_key"] : "";
            string privateKey = settings.ContainsKey("private_key") ? settings["private_key"] : "";
            return new OpenSSLPublicPrivate(publicKey, privateKey);
        }

     // # Get MD5 hashing instance (one-way only)
     // ## Usage: EncDec.MD5().Encrypt("your data")
        public static MD5 MD5() => new MD5();

     // # Get SHA256 hashing instance (one-way only)
     // ## Usage: EncDec.SHA256().Encrypt("your data")
        public static SHA256 SHA256() => new SHA256();

     // # public static SHA512 SHA512() => new SHA512();
     // ## Usage: EncDec.SHA512().Encrypt("your data")
        public static SHA512 SHA512() => new SHA512();

     // # Get Base64 encoding / decoding instance
     // ## Usage: EncDec.Base64().Encrypt("your data")
        public static base64 Base64() => new base64();
    }

}
