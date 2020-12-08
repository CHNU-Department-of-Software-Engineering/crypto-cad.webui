using System;
using CryptoCAD.Core.Ciphers.AES;
using CryptoCAD.Core.Ciphers.DES;
using CryptoCAD.Core.Ciphers.DES.Factory;
using CryptoCAD.Core.Ciphers.GOST;
using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Models.Services;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.Domain.Entities.Ciphers;
using CryptoCAD.Domain.Entities.Methods.Base;

namespace CryptoCAD.Core.Services
{
    public class CipherService : ICipherService
    {
        public ServiceResponse Process(byte[] key, byte[] data, CipherModes mode, MethodFamilies family, string configuration)
        {
            var cipher = GetCipher(family, configuration);
            byte[] bytes;

            switch (mode)
            {
                case CipherModes.Encrypt:
                    bytes = cipher.Encrypt(key, data);
                    break;
                case CipherModes.Decrypt:
                    bytes = cipher.Decrypt(key, data);
                    break;
                default:
                    throw new NotImplementedException("Cipher operation is not supported!");
            }

            return new ServiceResponse
            {
                Data = bytes,
                IntermediateResults = string.Empty
            };
        }

        private ICipher GetCipher(MethodFamilies family, string configuration = null)
        {
            switch (family)
            {
                case MethodFamilies.DES:
                    return string.IsNullOrEmpty(configuration)
                        ? new DESCipher()
                        : new DESCipherFactory()
                            .CreateCipher(configuration);
                case MethodFamilies.AES:
                    return string.IsNullOrEmpty(configuration)
                        ? new AESCipher()
                        : throw new NotImplementedException($"{MethodFamilies.AES.ToFriendlyString()} cipher family modification not implemented yet!");
                case MethodFamilies.GOST:
                    return string.IsNullOrEmpty(configuration)
                        ? new GOSTCipher()
                        : throw new NotImplementedException($"{MethodFamilies.GOST.ToFriendlyString()} cipher family modification not implemented yet!");
                default:
                    throw new ArgumentException($"{family.ToFriendlyString()} is not cipher family!");
            }
        }
    }
}