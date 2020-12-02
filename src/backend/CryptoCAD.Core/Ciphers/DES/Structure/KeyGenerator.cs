using System;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Ciphers.DES.Structure.Abstractions;

namespace CryptoCAD.Core.Ciphers.DES.Structure
{
    internal class KeyGenerator : IKeyGenerator
    {
        private readonly byte[] PC1_PERMUTATION_TABLE;
        private readonly byte[] PC2_PERMUTATION_TABLE;
        private readonly byte[] ROTATIONS_TABLE;

        public KeyGenerator(byte[] pc1PermutationTable, byte[] pc2PermutationTable, byte[] rotationsTable)
        {
            if (pc1PermutationTable is null)
            {
                throw new ArgumentNullException(nameof(pc1PermutationTable));
            }

            if (pc2PermutationTable is null)
            {
                throw new ArgumentNullException(nameof(pc2PermutationTable));
            }

            if (rotationsTable is null)
            {
                throw new ArgumentNullException(nameof(rotationsTable));
            }

            if (pc1PermutationTable.Length != DESDefaultSetup.PC1_PERMUTATION_TABLE_LENGHT)
            {
                throw new ArgumentException($"PC1 Permutation table should have {DESDefaultSetup.PC1_PERMUTATION_TABLE_LENGHT} lenght, not {pc1PermutationTable.Length}");
            }

            if (pc2PermutationTable.Length != DESDefaultSetup.PC2_PERMUTATION_TABLE_LENGHT)
            {
                throw new ArgumentException($"PC2 Permutation table should have {DESDefaultSetup.PC2_PERMUTATION_TABLE_LENGHT} lenght, not {pc2PermutationTable.Length}");
            }

            PC1_PERMUTATION_TABLE = pc1PermutationTable;
            PC2_PERMUTATION_TABLE = pc2PermutationTable;
            ROTATIONS_TABLE = rotationsTable;
        }

        public byte[][] GenerateSubkeys(byte[] key)
        {
            var activeKeySize = (byte)PC1_PERMUTATION_TABLE.Length;
            var numberOfSubkeys = (byte)ROTATIONS_TABLE.Length;

            var activeKey = ByteUtill.SelectBits(key, PC1_PERMUTATION_TABLE);
            var halfKeySize = activeKeySize / 2;

            var left = ByteUtill.SelectBits(activeKey, 0, halfKeySize);
            var right = ByteUtill.SelectBits(activeKey, halfKeySize, halfKeySize);

            var subkeys = new byte[numberOfSubkeys][];
            for (byte i = 0; i < numberOfSubkeys; i++)
            {
                left = ByteUtill.RotateLeft(left, halfKeySize, ROTATIONS_TABLE[i]);
                right = ByteUtill.RotateLeft(right, halfKeySize, ROTATIONS_TABLE[i]);
                var value = ByteUtill.ConcatenateBits(left, halfKeySize, right, halfKeySize);
                subkeys[i] = ByteUtill.SelectBits(value, PC2_PERMUTATION_TABLE);
            }

            return subkeys;
        }
    }
}