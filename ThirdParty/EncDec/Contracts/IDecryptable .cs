namespace IndeedClone.ThirdParty.EncDec.Contracts
{
    public interface IDecryptable : INonDecryptable
    {
        string Decrypt(string input, Dictionary<string, object> param = null);
    }
}
    