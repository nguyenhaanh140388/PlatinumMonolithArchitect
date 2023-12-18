namespace Anhny010920.Core.Abstractions.Services
{
    public interface IEncryptionService
    {
        string Decrypt(string cipherText);
        string Encrypt(string input);
    }
}
