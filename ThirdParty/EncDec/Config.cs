

namespace IndeedClone.ThirdParty.EncDec

{
    public class Config
    {
        private readonly IConfiguration _configuration;
        private Dictionary<string, Dictionary<string, string>> _settings;

        public Config(IConfiguration configuration = null)
        {
            _configuration = configuration;
             LoadSettings();
        }

        private void LoadSettings()
        {
            // Read values from appsettings.json (or use defaults if empty)
            var jsonPrivateKey = _configuration?["Encryption:OpenSSLPublicPrivate:PrivateKey"] ?? "";
            var jsonPublicKey = _configuration?["Encryption:OpenSSLPublicPrivate:PublicKey"] ?? "";
            var jsonSecretKey = _configuration?["Encryption:OpenSSL:SecretKey"] ?? "";
            var jsonIV = _configuration?["Encryption:OpenSSL:IV"] ?? "";

            _settings = new Dictionary<string, Dictionary<string, string>>
            {
                ["OpenSSLPublicPrivate"] = new Dictionary<string, string>
                {
                    // Use JSON value if not empty/whitespace, otherwise use default
                    ["private_key"] = !string.IsNullOrWhiteSpace(jsonPrivateKey)
                        ? jsonPrivateKey
                        : GetDefaultPrivateKey(),

                    ["public_key"] = !string.IsNullOrWhiteSpace(jsonPublicKey)
                        ? jsonPublicKey
                        : GetDefaultPublicKey()
                },
                ["OpenSSL"] = new Dictionary<string, string>
                {
                    ["sec"] = !string.IsNullOrWhiteSpace(jsonSecretKey)
                        ? jsonSecretKey
                        : "kGJeGF2hEQ",

                    ["iv"] = !string.IsNullOrWhiteSpace(jsonIV)
                        ? jsonIV
                        : "7236489512357891"
                },
                ["SHA256"] = new Dictionary<string, string>(),
                ["SHA512"] = new Dictionary<string, string>(),
            };
        }


        public Dictionary<string, string> GetSettings(string algorithm)
        {
            return _settings.ContainsKey(algorithm)
                ? new Dictionary<string, string>(_settings[algorithm]) // Return copy
                : new Dictionary<string, string>();
        }

        public void SetSettings(string name, Dictionary<string, string> configurations)
        {
            if (!string.IsNullOrEmpty(name) && configurations != null && _settings.ContainsKey(name))
                _settings[name] = configurations;
        }

        // Helper method to get specific setting
        public string GetSetting(string algorithm, string key)
        {
            if (_settings.ContainsKey(algorithm) && _settings[algorithm].ContainsKey(key))
                return _settings[algorithm][key];
            return string.Empty;
        }



        /**************************************** Private SRP Methods *********************************************/



        private string GetDefaultPrivateKey() => @"-----BEGIN PRIVATE KEY-----
MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBALdOVllp0sf6KSae
VnAFOkmnHc/mB3AAwXIiq+dSsJg7UYG72LMXE0Jy3lpyTB10zKemjWEQVQpGk4ap
ejh84O/gnrKCtdIFg5X2vlXtUnIrNM8hSCCSPrCLXdP+hfpNmCVafEjWURtodNHG
/kv0jgLVou+EOqaCqFqB2E8/yWhbAgMBAAECgYBGMAK0Pebtl4YGOX6Tag0Fgq3R
mxyh8Egh8dCsxGbGA4CUEl9JQ76mJOGq9DTc2oo6b3WXAu/s/VUjrOUVmAtwaDzp
JxWW4gAMGRoH9j2VIfGvIUUsnd30UVHRfJ/Q6fRakUChI/lCT/s9gcmU3NlNrpj3
JnZWzvzezd1A7/kccQJBAN1ktYNlw8iY7yqpBAa0a9QXYGFhcyiCE47esxMUsTVa
/kIO7ZF+odiP0IIG0SjJpfxF2RT3z+hbkSh21dH6cB0CQQDT9YVksGZ3R+n5Nsp2
CV9yPjWxOl9xA9GdIVjMviEGZLrRt7O7N+WLRH9fIg4AUv8jRzKOYZ0naY6pbBik
xkDXAkEAtkEUyCG7ZeS4ZrcSsG5Qoh3IYwI1KfDDJwcgBiIvq8vHqhvd6LuFguEJ
djEkeF5gPWhGx+MljZPr0JLbfOuc5QJAKMRXRLd87cJCKTG1nSBOYE3Ay/abNsRy
Q5OPXcnP1kf3erCnfAHTP4cMLIMDSGKuOd3Oxn3V2Se/TazBzKdo/wJAAXgfbIGI
dgh5xMvf67tg0fBNRfAQ2OdW2MJCiXUmRDHTxygrnfKNNDimBajTd9Go9zKQ9odH
GHhjW4dACL+U6g==
-----END PRIVATE KEY-----";

        private string GetDefaultPublicKey() => @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC3TlZZadLH+ikmnlZwBTpJpx3P
5gdwAMFyIqvnUrCYO1GBu9izFxNCct5ackwddMynpo1hEFUKRpOGqXo4fODv4J6y
grXSBYOV9r5V7VJyKzTPIUggkj6wi13T/oX6TZglWnxI1lEbaHTRxv5L9I4C1aLv
hDqmgqhagdhPP8loWwIDAQAB
-----END PUBLIC KEY-----";
  
        


    }

}

