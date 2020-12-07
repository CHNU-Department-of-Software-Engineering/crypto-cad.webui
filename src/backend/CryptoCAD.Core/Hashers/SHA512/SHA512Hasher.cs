using System.Text;
using System.Runtime.CompilerServices;
using CryptoCAD.Core.Hashers.Abstractions;
using CryptoCAD.Core.Utilities;
using System.Security.Cryptography;

[assembly: InternalsVisibleTo("CryptoCAD.Core.Tests")]
namespace CryptoCAD.Core.Hashers.SHA512
{
    internal class SHA512Hasher : IHasher
    {
        public string Hash(string data)
        {
            var bytes = ConvertUtill.FromString(data);

            var shaM = new SHA512Managed();
            var hash = shaM.ComputeHash(bytes);

            var hashStr = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                hashStr.Append(string.Format("{0:X2}", hash[i]));
            }

            return hashStr.ToString();
        }
    }
}