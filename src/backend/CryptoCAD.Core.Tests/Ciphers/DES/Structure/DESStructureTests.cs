using NUnit.Framework;
using CryptoCAD.Common.Helpers;
using CryptoCAD.Common.CiphersConfiguration;
using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Ciphers.DES.Structure;

namespace CryptoCAD.Core.Tests.Ciphers.DES.Structure
{
    [TestFixture]
    class DESStructureTests
    {
        [TestCase("myperkey", "Some generic text for testing!", true)]
        [TestCase("myperkey", "ASIFUNWQCOIAS CAKSMCpoawmd93j1m2r23f90mACSMCMPQWjr-0123m", true)]
        [TestCase("mypeey", "ASIFUNWQCOIAS CAKSMCpoawmd93j1m2r23f90mACSMCMPQWjr-0123m", false)]
        public void StructureBuild(string key, string data, bool isSuccessful)
        {
            var Cipher = CreateCipher();

            var keyBytes = key.ToBytes();
            var dataBytes = data.ToBytes();

            try
            {
                var encypted = Cipher.Encrypt(keyBytes, dataBytes);
                var decrypted = Cipher.Decrypt(keyBytes, encypted);
                var actual = decrypted.ToString(ConvertMode.UTF8).Trim('\0');

                Assert.AreEqual(data, actual);
            }
            catch
            {
                Assert.IsFalse(isSuccessful);
            }
        }

        private ICipher CreateCipher()
        {
            var keySchedule = new KeySchedule(
                DESConfigurations.PC1_PERMUTATION_TABLE,
                DESConfigurations.PC2_PERMUTATION_TABLE,
                DESConfigurations.ROTATIONS);

            var function = new Function(
                DESConfigurations.EXPANSION_PERMUTATION_TABLE,
                DESConfigurations.PERMUTATION_TABLE,
                DESConfigurations.SUBSTITUTION_BOXES);
            var round = new Round(function);

            var cipher = new Cipher(
                keySchedule,
                round,
                DESConfigurations.INITIAL_PERMUTATION_TABLE,
                DESConfigurations.FINAL_PERMUTATION_TABLE);

            return cipher;
        }
    }
}