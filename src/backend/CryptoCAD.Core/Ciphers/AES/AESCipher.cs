using System.Security.Cryptography;

namespace CryptoCAD.Core.Ciphers.AES
{
    public class AESCipher : BaseCipher
    {
        private readonly Aes _aes;

        /// <inheritdoc/>
        public override byte[] IV => _aes.IV;

        public AESCipher()
        {
            _aes = Aes.Create();
        }

        protected override ICryptoTransform GetEncryptor(byte[] key, byte[] IV)
        {
            return _aes.CreateEncryptor(key, IV);
        }

        protected override ICryptoTransform GetDecryptor(byte[] key, byte[] IV)
        {
            return _aes.CreateDecryptor(key, IV);
        }

        public override void Dispose()
        {
            _aes.Dispose();
        }
    }
}