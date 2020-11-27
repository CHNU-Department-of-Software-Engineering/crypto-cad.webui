using System;
using CryptoCAD.Core.DES.Abstractions;

namespace CryptoCAD.Core.DES
{
    internal class DESFunction : IFunction
    {
        public static readonly byte[] EP_DEFAULT = new byte[48]
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        public static readonly byte[][][] SBOX = new byte[8][][]
        {
            new byte[4][]
            {
                new byte[16] { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                new byte[16] { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                new byte[16] { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                new byte[16] { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
            },
            new byte[4][]
            {
                new byte[16] {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                new byte[16] {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                new byte[16] {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                new byte[16] {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 },
            },
            new byte[4][]
            {
                new byte[16] {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                new byte[16] {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                new byte[16] {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                new byte[16] {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 },
            },
            new byte[4][]
            {
                new byte[16] {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                new byte[16] {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                new byte[16] {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                new byte[16] {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 },
            },
            new byte[4][]
            {
                new byte[16] {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                new byte[16] {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                new byte[16] {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                new byte[16] {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 },
            },
            new byte[4][]
            {
                new byte[16] {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                new byte[16] {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                new byte[16] {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                new byte[16] {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 },
            },
            new byte[4][]
            {
                new byte[16] {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                new byte[16] {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                new byte[16] {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                new byte[16] {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 },
            },
            new byte[4][]
            {
                new byte[16] {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                new byte[16] {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                new byte[16] {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                new byte[16] {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 },
            },
        };

        public static readonly byte[] P_DEFAULT = new byte[32]
        {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };


        private const byte KEY_LENGHT = 48;
        private const byte DATA_LENGHT = 32;


        private readonly byte[] _EP;

        private readonly byte[] Key;
        private readonly byte[] Data;

        public DESFunction(byte[] key, byte[] data)
        {
            if (key.Length != KEY_LENGHT)
            {
                throw new ArgumentException($"Key lenght should be {KEY_LENGHT} bytes");
            }

            if (data.Length != DATA_LENGHT)
            {
                throw new ArgumentException($"Data lenght should be {DATA_LENGHT} bytes");
            }

            Key = key;
            Data = data;
        }

        public uint DoFunction(uint block32b, ulong key48b)
        {
            var block48b = ExpansionPermutation(block32b);
            block48b ^= key48b;
            var blocks6b = SplitBlock48bToBlocks6b(block48b);

            throw new NotImplementedException();
        }

        private ulong ExpansionPermutation(uint block32b)
        {
            ulong block48b = 0;
            for (var i = 0; i < 48; ++i)
            {
                block48b |= (ulong)((block32b >> (32 - _EP[i])) & 0x01) << (63 - i);
            }
            return block48b;
        }
        private byte[] SplitBlock48bToBlocks6b(ulong block48b)
        {
            var blocks6b = new byte[8];
            for (var i = 0; i < 8; ++i)
            {
                blocks6b[i] = (byte)((block48b >> (58 - (i - 6))) << 2);
            }
            return blocks6b;
        }

        private byte[] Substitution(byte[] blocks6b)
        {
            byte block2b, block4b;
            byte[] blocks4b = new byte[8];

            for (byte i = 0, j = 0; i < 8; i += 2, ++j)
            {
                block2b = ExtremeBits(blocks6b[i]);
                block4b = MiddleBits(blocks6b[i]);
                blocks4b[j] = SBOX[i][block2b][block4b];

                block2b = ExtremeBits(blocks6b[i + 1]);
                block4b = MiddleBits(blocks6b[i + 1]);
                blocks4b[j] = (byte)((blocks4b[j] << 4) | SBOX[i + 1][block2b][block4b]);
            }

            return blocks4b;
        }

        private byte ExtremeBits(byte block6b) =>
            (byte)(((block6b >> 6) & 0x2) | ((block6b >> 2) & 0x1));

        private byte MiddleBits(byte block6b) =>
            (byte)((block6b >> 3) & 0xF);
    }
}
