namespace IndeedClone.ThirdParty.EncDec.Contracts
{
    public interface INonDecryptable
    {
        string Encrypt(string input, Dictionary<string, object> param = null);
    }
}
