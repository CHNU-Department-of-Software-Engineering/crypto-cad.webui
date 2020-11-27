using CryptoCAD.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoCAD.Console
{
    public class Cipher
    {

        public void Main()
        {
            System.Console.Write("Key (8 chars): ");
            var key = System.Console.ReadLine();

            System.Console.Write("Text: ");
            var text = System.Console.ReadLine();

            System.Console.WriteLine();

            var response = DESEncode(key, text);
            System.Console.WriteLine($"Encrypted text: {response.Data}\n");

            var responseD = DESDecode(key, text, response.IV, response.DataB);
            System.Console.WriteLine($"Decrypted text: {responseD.Data}\n");

            System.Console.ReadKey();
        }


        private CipherResponse DESEncode(string key, string data)
        {
            var key_b = Encoding.UTF8.GetBytes(key);

            if (key_b.Length != 8)
            {
                throw new ArgumentException($"Key lenght should be 64-bit. Received key lenght: {key_b.Length}");
            }

            var memoryStream = new MemoryStream();

            var DESalg = DES.Create();
            DESalg.Padding = PaddingMode.Zeros;

            var IV = DESalg.IV;

            var cryptoStream = new CryptoStream(memoryStream, DESalg.CreateEncryptor(key_b, IV), CryptoStreamMode.Write);

            byte[] toEncrypt = Encoding.UTF8.GetBytes(data);

            cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
            cryptoStream.FlushFinalBlock();

            byte[] ret = memoryStream.ToArray();

            cryptoStream.Close();
            memoryStream.Close();

            return new CipherResponse
            {
                Key = key,
                Data = Encoding.UTF8.GetString(ret),
                IV = BitConverter.ToUInt64(IV, 0),
                DataB = ret
            };
        }

        private CipherResponse DESDecode(string key, string data, ulong IV, byte[] dataB)
        {
            var key_b = Encoding.UTF8.GetBytes(key);

            if (key_b.Length != 8)
            {
                throw new ArgumentException($"Key lenght should be 64-bit. Received key lenght: {key_b.Length}");
            }

            byte[] toDecrypt = Encoding.UTF8.GetBytes(data);

            var iv = BitConverter.GetBytes(IV);

            var memoryStream = new MemoryStream(dataB);

            var DESalg = DES.Create();
            DESalg.Padding = PaddingMode.Zeros;

            var cryptoStream = new CryptoStream(memoryStream,
                DESalg.CreateDecryptor(key_b, iv),
                CryptoStreamMode.Read);

            byte[] fromEncrypt = new byte[dataB.Length];

            cryptoStream.Read(fromEncrypt, 0, fromEncrypt.Length);

            var result = Encoding.UTF8.GetString(fromEncrypt);

            return new CipherResponse
            {
                Key = key,
                Data = result
            };
        }
    }
}
