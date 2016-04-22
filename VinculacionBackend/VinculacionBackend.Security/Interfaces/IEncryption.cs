namespace VinculacionBackend.Security.Interfaces
{
    public interface IEncryption
    {
        string Encrypt(string stringValue);
        string Decrypt(string stringValue);
    }
}
