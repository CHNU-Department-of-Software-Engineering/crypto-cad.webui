using CryptoCAD.Core.Models.Ciphers;

namespace CryptoCAD.Core.Services.Abstractions
{
    public interface ICipherService
    {
        byte[] Process(string name, CipherOperations operation, byte[] key, byte[] data);
    }
}