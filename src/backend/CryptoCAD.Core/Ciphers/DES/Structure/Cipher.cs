using System;
using System.Runtime.CompilerServices;
using CryptoCAD.Common.Helpers;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Ciphers.DES.Structure.Abstractions;
using CryptoCAD.Domain.Entities.Ciphers;

[assembly: InternalsVisibleTo("CryptoCAD.Core.Tests")]
namespace CryptoCAD.Core.Ciphers.DES.Structure
{
    internal class Cipher : ICipher
    {
        private readonly IKeySchedule KeySchedule;
        private readonly IRound Round;

        private readonly byte[] IP;
        private readonly byte[] FP;

        public Cipher(IKeySchedule keySchedule, IRound round, byte[] ip, byte[] fp)
        {
            KeySchedule = keySchedule;
            Round = round;
            IP = ip;
            FP = fp;
        }

        public byte[] Decrypt(byte[] key, byte[] data)
        {
            return Process(key, data, CipherModes.Encrypt);
        }
        public byte[] Encrypt(byte[] key, byte[] data)
        {
            return Process(key, data, CipherModes.Decrypt);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private byte[] Process(byte[] key, byte[] data, CipherModes mode)
        {
            if (key.Length != 8)
            {
                throw new ArgumentException("Invalid DES key lenght!");
            }
            var keys = KeySchedule.GenerateSubkeys(key);
            if (mode == CipherModes.Decrypt)
            {
                Array.Reverse(keys);
            }

            var blocks64b = data.ToUInts64();
            var resultBlocks64b = new ulong[blocks64b.Length];

            for (byte i = 0; i < blocks64b.Length; i++)
            {
                resultBlocks64b[i] = Permutation(blocks64b[i], IP);
                resultBlocks64b[i] = FeistelCipher(resultBlocks64b[i], keys);
                resultBlocks64b[i] = Permutation(resultBlocks64b[i], FP);
            }

            return resultBlocks64b.ToBytes();
        }

        private ulong FeistelCipher(ulong block64b, ulong[] keys)
        {
            uint N1 = (uint)(block64b & 0xFFFFFFFF), N2 = (uint)((block64b >> 32) & 0xFFFFFFFF);
            for (byte i = 0; i < keys.Length; i++)
            {
                var res = Round.Process(N1, N2, keys[i]);
                N1 = res.Item1;
                N2 = res.Item2;
            }

            var output = new byte[8];
            var N1buff = BitConverter.GetBytes(N1);
            var N2buff = BitConverter.GetBytes(N2);

            for (byte i = 0; i < 4; i++)
            {
                output[i] = N1buff[i];
                output[4 + i] = N2buff[i];
            }

            return BitConverter.ToUInt64(output, 0);
        }
 
        private ulong Permutation(ulong block64b, byte[] permutationTable)
        {
            ulong newBlock64b = 0;
            for (byte i = 0; i < 64; ++i)
            {
                newBlock64b |= ((block64b >> (64 - permutationTable[i])) & 0x01) << (63 - i);
            }
            return newBlock64b;
        }
    }
}