using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CryptoCAD.Core.Tests")]
namespace CryptoCAD.Core.Ciphers.Abstractions
{
    internal interface ICipher : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="IV"></param>
        /// <returns>Encrypted data as byte array</returns>
        byte[] Encrypt(byte[] key, byte[] data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="IV"></param>
        /// <returns>Decrypted data as byte array</returns>
        byte[] Decrypt(byte[] key, byte[] data);
    }
}