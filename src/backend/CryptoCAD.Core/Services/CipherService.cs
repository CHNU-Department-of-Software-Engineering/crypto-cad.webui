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
        public CipherResult Process(CipherArguments args)
        {
            var cipher = GetCipher(args.Name);
            switch (args.Operation)
            {
                case CipherOperations.Encrypt:
                    var encriprionResult = cipher.Encrypt(args.Key, args.Data);
                    return new CipherResult()
                    {
                        Name = args.Name,
                        Operation = args.Operation,
                        Key = args.Key,
                        Data = encriprionResult
                    };
                case CipherOperations.Decrypt:
                    var decriptionResult = cipher.Decrypt(args.Key, args.Data);
                    return new CipherResult()
                    {
                        Name = args.Name,
                        Operation = args.Operation,
                        Key = args.Key,
                        Data = decriptionResult
                    };
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