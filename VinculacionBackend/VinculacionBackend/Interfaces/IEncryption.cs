namespace VinculacionBackend.Interfaces
{
    public interface IEncryption
    {
        string Encrypt(string stringValue);
        string Decrypt(string stringValue);
    }
}
