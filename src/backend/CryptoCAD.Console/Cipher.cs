using CryptoCAD.API.Models;
using CryptoCAD.Core.Utilities;
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

            var responseD = DESDecode(key, response.Data, 123);
            System.Console.WriteLine($"Decrypted text: {responseD.Data}\n");

            System.Console.ReadKey();
        }


        private CipherResponse DESEncode(string key, string data)
        {
            var key_b = ConvertUtill.FromString(key, ConvertMode.UTF8);

            if (key_b.Length != 8)
            {
                throw new ArgumentException($"Key lenght should be 64-bit. Received key lenght: {key_b.Length}");
            }

            var memoryStream = new MemoryStream();

            var DESalg = DES.Create();
            DESalg.Padding = PaddingMode.Zeros;

            var IV = DESalg.IV;

            var cryptoStream = new CryptoStream(memoryStream, DESalg.CreateEncryptor(key_b, IV), CryptoStreamMode.Write);

            byte[] toEncrypt = ConvertUtill.FromString(data, ConvertMode.UTF8);

            cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
            cryptoStream.FlushFinalBlock();

            byte[] ret = memoryStream.ToArray();

            cryptoStream.Close();
            memoryStream.Close();

            return new CipherResponse
            {
                Key = key,
                Data = ConvertUtill.ToString(ret, ConvertMode.BASE64)
                //IV = BitConverter.ToUInt64(IV, 0)
            };
        }

        private CipherResponse DESDecode(string key, string data, ulong IV)
        {
            var key_b = ConvertUtill.FromString(key, ConvertMode.UTF8);

            if (key_b.Length != 8)
            {
                throw new ArgumentException($"Key lenght should be 64-bit. Received key lenght: {key_b.Length}");
            }

            byte[] toDecrypt = ConvertUtill.FromString(data, ConvertMode.BASE64);

            var iv = BitConverter.GetBytes(IV);

            var memoryStream = new MemoryStream(toDecrypt);

            var DESalg = DES.Create();
            DESalg.Padding = PaddingMode.Zeros;

            var cryptoStream = new CryptoStream(memoryStream,
                DESalg.CreateDecryptor(key_b, iv),
                CryptoStreamMode.Read);

            byte[] fromEncrypt = new byte[toDecrypt.Length];

            cryptoStream.Read(fromEncrypt, 0, fromEncrypt.Length);

            var result = ConvertUtill.ToString(fromEncrypt, ConvertMode.UTF8);

            return new CipherResponse
            {
                Key = key,
                Data = result
            };
        }
    }
}
