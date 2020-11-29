using System;
using CryptoCAD.Core.Ciphers.AES;
using CryptoCAD.Core.Ciphers.DES;
using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Models.Ciphers;
using CryptoCAD.Core.Services.Abstractions;

namespace CryptoCAD.Core.Services
{
    public class CipherService : ICipherService
    {
        public byte[] Process(string name, CipherOperations operation, byte[] key, byte[] data)
        {
            var cipher = GetCipher(name);
            switch (operation)
            {
                case CipherOperations.Encrypt:
                    return cipher.Encrypt(key, data);
                case CipherOperations.Decrypt:
                    return cipher.Decrypt(key, data);
                default:
                    throw new NotImplementedException("Cipher operation is not supported!");
            }
        }

        private ICipher GetCipher(string name)
        {
            switch (name)
            {
                case "des":
                    return new DESCipher();
                case "aes":
                    return new AESCipher();
                default:
                    throw new NotImplementedException($"Cipher '{name}' is not supported!");
            }
        }
    }
}