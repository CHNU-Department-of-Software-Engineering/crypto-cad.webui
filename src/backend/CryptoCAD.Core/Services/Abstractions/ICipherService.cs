using CryptoCAD.Core.Models.Ciphers;

namespace CryptoCAD.Core.Services.Abstractions
{
    public interface ICipherService
    {
        CipherResult Process(CipherArguments args);
    }
}