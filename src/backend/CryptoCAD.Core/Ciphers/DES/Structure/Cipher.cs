using CryptoCAD.Core.Ciphers.Abstractions;
using CryptoCAD.Core.Ciphers.DES.Structure.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCAD.Core.Ciphers.DES.Structure
{
    internal class Cipher : ICipher
    {
        private readonly IKeySchedule KeySchedule;
        private readonly IRound Round;

        private readonly byte[] IP;
        private readonly byte[] FP;

        public Cipher()
        {

        }

        public byte[] Decrypt(byte[] key, byte[] data)
        {
            throw new NotImplementedException();
        }
        public byte[] Encrypt(byte[] key, byte[] data)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Process(bool encrypt, byte[] key, byte[] data)
        {
            var keys = KeySchedule.GenerateSubkeys(key);

            var block64b = 0;


            block64b = Permutation(block64b, IP);

            block64b = Permutation(block64b, FP);
        }

        private void FeistelCipher(bool encrypt)
        {

        }
 
        private int Permutation(int block64b, byte[] permutationTable)
        {
            var newBlock64b = 0;
            for (byte i = 0; i < 64; ++i)
            {
                newBlock64b |= ((block64b >> (64 - permutationTable[i])) & 0x01) << (63 - i);
            }
            return newBlock64b;
        }
    }
}