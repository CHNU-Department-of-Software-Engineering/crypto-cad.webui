using System.Linq;
using Newtonsoft.Json;
using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Ciphers.DES.Structure;
using CryptoCAD.Common.CiphersConfiguration;
using CryptoCAD.Core.Factories.Abstractions;
using CryptoCAD.Domain.Entities.Ciphers.DES;

namespace CryptoCAD.Core.Ciphers.DES.Factory
{
    internal class DESCipherFactory : ICipherFactory
    {
        public ICipher CreateCipher(string configuration)
        {
            byte[] Pc1PermutationTable;
            byte[] Pc2PermutationTable;
            byte[] RotationsTable;
            byte[] ExpansionPermutationTable;
            byte[] PermutationTable;
            byte[][] SubstitutionBoxes;
            byte[] InitialPermutationTable;
            byte[] FinalPermutationTable;

            if (string.IsNullOrEmpty(configuration))
            {
                Pc1PermutationTable = DESConfigurations.PC1_PERMUTATION_TABLE;
                Pc2PermutationTable = DESConfigurations.PC2_PERMUTATION_TABLE;
                RotationsTable = DESConfigurations.ROTATIONS;
                ExpansionPermutationTable = DESConfigurations.EXPANSION_PERMUTATION_TABLE;
                PermutationTable = DESConfigurations.PERMUTATION_TABLE;
                SubstitutionBoxes = DESConfigurations.SUBSTITUTION_BOXES;
                InitialPermutationTable = DESConfigurations.INITIAL_PERMUTATION_TABLE;
                FinalPermutationTable = DESConfigurations.FINAL_PERMUTATION_TABLE;
            }
            else
            {
                var configurations = JsonConvert.DeserializeObject<DESConfiguration>(configuration);

                if (configurations.Pc1PermutationTable is null)
                {
                    Pc1PermutationTable = DESConfigurations.PC1_PERMUTATION_TABLE;
                }
                else
                {
                    Pc1PermutationTable = configurations.Pc1PermutationTable.Select(x => (byte)x).ToArray();
                }

                if (configurations.Pc2PermutationTable is null)
                {
                    Pc2PermutationTable = DESConfigurations.PC2_PERMUTATION_TABLE;
                }
                else
                {
                    Pc2PermutationTable = configurations.Pc2PermutationTable.Select(x => (byte)x).ToArray();
                }

                if (configurations.RotationsTable is null)
                {
                    RotationsTable = DESConfigurations.ROTATIONS;
                }
                else
                {
                    RotationsTable = configurations.RotationsTable.Select(x => (byte)x).ToArray();
                }

                if (configurations.ExpansionPermutationTable is null)
                {
                    ExpansionPermutationTable = DESConfigurations.EXPANSION_PERMUTATION_TABLE;
                }
                else
                {
                    ExpansionPermutationTable = configurations.ExpansionPermutationTable.Select(x => (byte)x).ToArray();
                }

                if (configurations.PermutationTable is null)
                {
                    PermutationTable = DESConfigurations.PERMUTATION_TABLE;
                }
                else
                {
                    PermutationTable = configurations.PermutationTable.Select(x => (byte)x).ToArray();
                }

                if (configurations.InitialPermutationTable is null)
                {
                    InitialPermutationTable = DESConfigurations.INITIAL_PERMUTATION_TABLE;
                }
                else
                {
                    InitialPermutationTable = configurations.InitialPermutationTable.Select(x => (byte)x).ToArray();
                }

                if (configurations.FinalPermutationTable is null)
                {
                    FinalPermutationTable = DESConfigurations.FINAL_PERMUTATION_TABLE;
                }
                else
                {
                    FinalPermutationTable = configurations.FinalPermutationTable.Select(x => (byte)x).ToArray();
                }

                SubstitutionBoxes = new byte[8][];
                if (configurations.SBox1 is null)
                {
                    SubstitutionBoxes[0] = DESConfigurations.SUBSTITUTION_BOXES[0];
                }
                else
                {
                    SubstitutionBoxes[0] = configurations.SBox1.Select(x => (byte)x).ToArray();
                }
                if (configurations.SBox2 is null)
                {
                    SubstitutionBoxes[1] = DESConfigurations.SUBSTITUTION_BOXES[1];
                }
                else
                {
                    SubstitutionBoxes[1] = configurations.SBox2.Select(x => (byte)x).ToArray();
                }
                if (configurations.SBox3 is null)
                {
                    SubstitutionBoxes[2] = DESConfigurations.SUBSTITUTION_BOXES[2];
                }
                else
                {
                    SubstitutionBoxes[2] = configurations.SBox3.Select(x => (byte)x).ToArray();
                }
                if (configurations.SBox4 is null)
                {
                    SubstitutionBoxes[3] = DESConfigurations.SUBSTITUTION_BOXES[3];
                }
                else
                {
                    SubstitutionBoxes[3] = configurations.SBox4.Select(x => (byte)x).ToArray();
                }
                if (configurations.SBox5 is null)
                {
                    SubstitutionBoxes[4] = DESConfigurations.SUBSTITUTION_BOXES[4];
                }
                else
                {
                    SubstitutionBoxes[4] = configurations.SBox5.Select(x => (byte)x).ToArray();
                }
                if (configurations.SBox6 is null)
                {
                    SubstitutionBoxes[5] = DESConfigurations.SUBSTITUTION_BOXES[5];
                }
                else
                {
                    SubstitutionBoxes[5] = configurations.SBox6.Select(x => (byte)x).ToArray();
                }
                if (configurations.SBox7 is null)
                {
                    SubstitutionBoxes[6] = DESConfigurations.SUBSTITUTION_BOXES[6];
                }
                else
                {
                    SubstitutionBoxes[6] = configurations.SBox7.Select(x => (byte)x).ToArray();
                }
                if (configurations.SBox8 is null)
                {
                    SubstitutionBoxes[7] = DESConfigurations.SUBSTITUTION_BOXES[7];
                }
                else
                {
                    SubstitutionBoxes[7] = configurations.SBox8.Select(x => (byte)x).ToArray();
                }
            }

            var keySchedule = new KeySchedule(
                Pc1PermutationTable,
                Pc2PermutationTable,
                RotationsTable);

            var function = new Function(
                ExpansionPermutationTable,
                PermutationTable,
                SubstitutionBoxes);
            var round = new Round(function);

            var cipher = new Cipher(
                keySchedule,
                round,
                InitialPermutationTable,
                FinalPermutationTable);

            return cipher;
        }
    }
}