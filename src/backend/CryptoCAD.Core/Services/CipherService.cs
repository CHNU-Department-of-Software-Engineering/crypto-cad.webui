using System;
using CryptoCAD.Core.Ciphers.AES;
using CryptoCAD.Core.Ciphers.DES;
using CryptoCAD.Core.Ciphers.GOST;
using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Models.Ciphers;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.Core.Factories.Abstractions;
using CryptoCAD.Domain.Repositories;
using CryptoCAD.Domain.Entities.Methods.Ciphers;

namespace CryptoCAD.Core.Services
{
    public class CipherService : ICipherService
    {
        private readonly ICipherFactory CipherFactory;
        private readonly IMethodsRepository MethodsRepository;

        public CipherService(IMethodsRepository methodsRepository)
        {
            MethodsRepository = methodsRepository;
        }

        public byte[] Process(string name, CipherModes mode, byte[] key, byte[] data, string configuration)
        {
            var cipher = GetCipher(name, configuration);
            switch (mode)
            {
                case CipherModes.Encrypt:
                    return cipher.Encrypt(key, data);
                case CipherModes.Decrypt:
                    return cipher.Decrypt(key, data);
                default:
                    throw new NotImplementedException("Cipher operation is not supported!");
            }
        }

        private ICipher GetCipher(string name, string configuration)
        {
            switch (name)
            {
                case "des_library":
                    return new DESCipher();
                case "aes":
                    return new AESCipher();
                case "gost":
                    return new GOSTCipher();
                default:
                    try
                    {
                        return CipherFactory.CreateCipher(configuration);
                    }
                    catch (Exception exception)
                    {
                        throw new NotImplementedException($"Cipher '{name}' is not supported!", exception);
                    }
            }
        }
    }
}