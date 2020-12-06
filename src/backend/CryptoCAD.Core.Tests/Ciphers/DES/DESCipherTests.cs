using NUnit.Framework;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Ciphers.DES;
using CryptoCAD.Core.Ciphers.Abstractions;

namespace CryptoCAD.Core.Tests.Ciphers.DES
{
    [TestFixture]
    public class DESCipherTests
    {
        private readonly ICipher Сipher;

        public DESCipherTests()
        {
            Сipher = new DESCipher();
        }

        [TestCase("myperkey", "Some generic text for testing!", true)]
        [TestCase("myperkey", "ASIFUNWQCOIAS CAKSMCpoawmd93j1m2r23f90mACSMCMPQWjr-0123m", true)]
        [TestCase("mypeey", "ASIFUNWQCOIAS CAKSMCpoawmd93j1m2r23f90mACSMCMPQWjr-0123m", false)]
        public void Encryption(string key, string data, bool isSuccessful)
        {
            var keyBytes = ConvertUtill.FromString(key);
            var dataBytes = ConvertUtill.FromString(data);

            try
            {
                var encypted = Сipher.Encrypt(keyBytes, dataBytes);
                var decrypted = Сipher.Decrypt(keyBytes, encypted);
                var actual = ConvertUtill.ToString(decrypted).Trim('\0');

                Assert.AreEqual(data, actual);
            }
            catch
            {
                Assert.IsFalse(isSuccessful);
            }
        }
    }
}