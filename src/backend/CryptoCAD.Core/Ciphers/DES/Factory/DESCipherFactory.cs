using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Ciphers.DES.Structure;
using CryptoCAD.Core.Factories.Abstractions;
using CryptoCAD.Common.Configurations.Ciphers;

namespace CryptoCAD.Core.Ciphers.DES.Factory
{
    internal class DESCipherFactory : ICipherFactory
    {
        public ICipher CreateCipher(string configuration)
        {
            var desConfiguration = configuration.DESConfigurationFromJsonString();

            var results = new IntermediateResults
            {
                Rounds = new RoundResults[desConfiguration.Rotations.Length],
                KeySchedule = new KeyScheduleResults()
            };

            var keySchedule = new KeySchedule(
                desConfiguration.Pc1Permutation,
                desConfiguration.Pc2Permutation,
                desConfiguration.Rotations,
                results.KeySchedule);

            var function = new Function(
                desConfiguration.ExpansionPermutation,
                desConfiguration.Permutation,
                desConfiguration.SBoxes);

            var round = new Round(function);

            var cipher = new Cipher(
                keySchedule,
                round,
                desConfiguration.InitialPermutation,
                desConfiguration.FinalPermutation,
                results);

            return cipher;
        }
    }
}