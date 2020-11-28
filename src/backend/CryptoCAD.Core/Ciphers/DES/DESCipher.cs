using System.Security.Cryptography;

namespace CryptoCAD.Core.Ciphers.DES
{
    public class DESCipher : BaseCipher
    {
        private readonly System.Security.Cryptography.DES _des;

        /// <inheritdoc/>
        public override byte[] IV => _des.IV;

        public DESCipher()
        {
            _des = System.Security.Cryptography.DES.Create();

        }

        protected override ICryptoTransform GetEncryptor(byte[] key, byte[] IV)
        {
            return _des.CreateEncryptor(key, IV);
        }

        protected override ICryptoTransform GetDecryptor(byte[] key, byte[] IV)
        {
            return _des.CreateDecryptor(key, IV);
        }

        public override void Dispose()
        {
            _des.Dispose();
        }
    }
}