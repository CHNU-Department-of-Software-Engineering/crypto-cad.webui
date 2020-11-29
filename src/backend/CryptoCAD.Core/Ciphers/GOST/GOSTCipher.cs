
using System;
using System.Threading.Tasks;
using CryptoCAD.Core.Ciphers.Abstractions;

namespace CryptoCAD.Core.Ciphers.GOST
{
    internal class GOSTCipher : ICipher
    {
        private const byte KEY_LENGHT = 32;
        private byte[][] SubstitutionBox =
        {
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF }
        };

        public byte[] Decrypt(byte[] key, byte[] data)
        {
            var subkeys = GenerateKeys(key);
            var result = new byte[data.Length];
            var block = new byte[8];

            var isParallel = false;

            if (isParallel)
            {
                Parallel.For(0, data.Length / 8, i =>
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(DecryptBlock(block, subkeys), 0, result, 8 * i, 8);
                });
            }
            else
            {
                for (int i = 0; i < data.Length / 8; i++) // N blocks 64bits length.
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(DecryptBlock(block, subkeys), 0, result, 8 * i, 8);
                }
            }
            return result;
        }

        public byte[] Encrypt(byte[] key, byte[] data)
        {
            var subkeys = GenerateKeys(key);
            var result = new byte[data.Length];
            var block = new byte[8];

            var isParallel = false;

            if (isParallel)
            {
                Parallel.For(0, data.Length / 8, i =>
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(EncryptBlock(block, subkeys), 0, result, 8 * i, 8);
                });
            }
            else
            {
                for (int i = 0; i < data.Length / 8; i++) // N blocks 64bits length.
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(EncryptBlock(block, subkeys), 0, result, 8 * i, 8);
                }
            }
            return result;
        }

        public void Dispose()
        {

        }

        private byte[] DecryptBlock(byte[] block, uint[] keys)
        {
            uint N1 = BitConverter.ToUInt32(block, 0);
            uint N2 = BitConverter.ToUInt32(block, 4);

            for (byte i = 0; i < 32; i++)
            {
                int keyIndex = i < 8 ? (i % 8) : (7 - i % 8); // to 24th cycle : 0 to 7; after - 7 to 0;
                var s = (N1 + keys[keyIndex]) % uint.MaxValue; // (N1 + X[i]) mod 2^32
                s = Substitution(s); // substitute from box
                s = (s << 11) | (s >> 21);
                s ^= N2;
                if (i < 31) // last cycle : N1 don't change; N2 = s;
                {
                    N2 = N1;
                    N1 = s;
                }
                else
                {
                    N2 = s;
                }
            }

            var output = new byte[8];
            var N1buff = BitConverter.GetBytes(N1);
            var N2buff = BitConverter.GetBytes(N2);

            for (int i = 0; i < 4; i++)
            {
                output[i] = N1buff[i];
                output[4 + i] = N2buff[i];
            }

            return output;
        }
        private byte[] EncryptBlock(byte[] block, uint[] keys)
        {
            uint N1 = BitConverter.ToUInt32(block, 0);
            uint N2 = BitConverter.ToUInt32(block, 4);

            for (int i = 0; i < 32; i++)
            {
                int keyIndex = i < 24 ? (i % 8) : (7 - i % 8); // to 24th cycle : 0 to 7; after - 7 to 0;
                var s = (N1 + keys[keyIndex]) % uint.MaxValue; // (N1 + X[i]) mod 2^32
                s = Substitution(s); // substitute from box
                s = (s << 11) | (s >> 21);
                s = s ^ N2; // ( s + N2 ) mod 2
                //N2 = N1;
                //N1 = s;
                if (i < 31) // last cycle : N1 don't change; N2 = s;
                {
                    N2 = N1;
                    N1 = s;
                }
                else
                {
                    N2 = s;
                }
            }

            var output = new byte[8];
            var N1buff = BitConverter.GetBytes(N1);
            var N2buff = BitConverter.GetBytes(N2);

            for (int i = 0; i < 4; i++)
            {
                output[i] = N1buff[i];
                output[4 + i] = N2buff[i];
            }

            return output;
        }

        private uint[] GenerateKeys(byte[] key)
        {
            if (key.Length != KEY_LENGHT)
            {
                throw new ArgumentException($"Key lenght should be {KEY_LENGHT}, not {key.Length}");
            }

            var subkeys = new uint[8];

            for (byte i = 0; i < 8; i++)
            {
                subkeys[i] = BitConverter.ToUInt32(key, 4 * i);
            }

            return subkeys;
        }
        private uint Substitution(uint value)
        {
            uint output = 0;

            for (byte i = 0; i < 8; i++)
            {
                var temp = (byte)((value >> (4 * i)) & 0x0f);
                temp = SubstitutionBox[i][temp];
                output |= (uint)temp << (4 * i);
            }

            return output;
        }
    }
}