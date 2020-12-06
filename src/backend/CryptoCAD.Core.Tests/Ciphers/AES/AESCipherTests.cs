using NUnit.Framework;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Ciphers.AES;
using CryptoCAD.Core.Ciphers.Abstractions;

namespace CryptoCAD.Core.Tests.Ciphers.AES
{
    [TestFixture]
    public class AESCipherTests
    {
        private readonly ICipher Сipher;

        public AESCipherTests()
        {
            Сipher = new AESCipher();
        }

        [TestCase("myperkeymyperkey", "Some generic text for testing!", true)]
        [TestCase("myperkeymyperkey", "ASIFUNWQCOIAS CAKSMCpoawmd93j1m2r23f90mACSMCMPQWjr-0123m", true)]
        [TestCase("mypeey", "ASIFUNWQCOIAS CAKSMCpoawmd93j1m2r23f90mACSMCMPQWjr-0123m", false)]
        public void Encryption(string key, string data, bool isSuccessful)
        {
            var keyBytes = ConvertUtill.FromString(key);
            var dataBytes = ConvertUtill.FromString(data);

            try
            {
                var encypted = Сipher.Encrypt(keyBytes, dataBytes);
                var decypted = Сipher.Decrypt(keyBytes, encypted);
                var actual = ConvertUtill.ToString(decypted).Trim('\0');

                Assert.AreEqual(data, actual);
            }
            catch
            {
                Assert.IsFalse(isSuccessful);
            }
        }
    }
}