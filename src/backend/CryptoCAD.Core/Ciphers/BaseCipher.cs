using System.IO;
using System.Security.Cryptography;
using CryptoCAD.Core.Ciphers.Abstractions;

namespace CryptoCAD.Core.Ciphers
{
    public abstract class BaseCipher : ICipher
    {
        public abstract byte[] IV { get; }

        /// <inheritdoc/>
        public byte[] Decrypt(byte[] key, byte[] data, byte[] IV)
        {
            byte[] decryptedData;

            using (var memoryStream = new MemoryStream(data))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, GetDecryptor(key, IV), CryptoStreamMode.Read))
                {
                    decryptedData = new byte[data.Length];

                    cryptoStream.Read(decryptedData, 0, decryptedData.Length);
                }
            }

            return decryptedData;
        }

        /// <inheritdoc/>
        public byte[] Encrypt(byte[] key, byte[] data, byte[] IV)
        {
            byte[] encryptedData;

            using (var memoryStream = new MemoryStream())
            {

                using (var cryptoStream = new CryptoStream(memoryStream, GetEncryptor(key, IV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();

                    encryptedData = memoryStream.ToArray();
                }
            }

            return encryptedData;
        }

        protected abstract ICryptoTransform GetEncryptor(byte[] key, byte[] IV);
        protected abstract ICryptoTransform GetDecryptor(byte[] key, byte[] IV);

        public abstract void Dispose();
    }
}