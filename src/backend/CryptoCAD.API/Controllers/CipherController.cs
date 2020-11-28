using CryptoCAD.API.Models;
using CryptoCAD.Core.Models.Ciphers;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.Core.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace CryptoCAD.API.Controllers
{
    [ApiController]
    [EnableCors("MyPolicy")]
    [Route("api/cipher")]
    public class CipherController : ControllerBase
    {
        private readonly ICipherService CipherService;
        private readonly ILogger<CipherController> Logger;

        public CipherController(ICipherService cipherService, ILogger<CipherController> logger)
        {
            CipherService = cipherService;
            Logger = logger;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<CipherModel>> GetCiphers()
        {
            var ciphers = new List<CipherModel>()
            {
                new CipherModel
                {
                    Name = "des",
                    KeyLenghtBits = 64
                },
                new CipherModel
                {
                    Name = "aes",
                    KeyLenghtBits = 128
                }
            };

            return Ok(ciphers);
        }

        [HttpPost]
        [Route("process")]
        public ActionResult<CipherResponse> Process(CipherRequest request)
        {
            

            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new ArgumentNullException("Cipher name is required!");
                }
                var operation = request.Mode == "encryption"
                    ? CipherOperations.Encrypt
                    : request.Mode == "decryption"
                    ? CipherOperations.Decrypt
                    : throw new NotSupportedException("Only 'encryption' and 'decryption' modes are supported!");

                var key = ConvertUtill.FromString(request.Key, ConvertMode.UTF8);
                var data = ConvertUtill.FromString(request.Data, operation == CipherOperations.Decrypt ? ConvertMode.BASE64 : ConvertMode.UTF8);

                var cipherArgs = new CipherArguments()
                {
                    Name = request.Name,
                    Operation = operation,
                    Key = key,
                    Data = data
                };

                var result = CipherService.Process(cipherArgs);

                var dataResult = ConvertUtill.ToString(result.Data, operation == CipherOperations.Encrypt ? ConvertMode.BASE64 : ConvertMode.UTF8);

                return Ok(new CipherResponse()
                {
                    Name = request.Name,
                    Mode = request.Mode,
                    Key = request.Key,
                    Data = dataResult
                });
            }
            catch (Exception exception)
            {
                return BadRequest($"Exception occured: {exception}");
            }
        }
           
        //private CipherResponse DESEncode(string key, string data)
        //{
        //    var key_b = ConvertUtill.FromString(key, ConvertMode.UTF8);

        //    if (key_b.Length != 8)
        //    {
        //        throw new ArgumentException($"Key lenght should be 64-bit. Received key lenght: {key_b.Length}");
        //    }

        //    var memoryStream = new MemoryStream();

        //    var DESalg = DES.Create();
        //    DESalg.Padding = PaddingMode.Zeros;

        //    var IV = DESalg.IV;

        //    var cryptoStream = new CryptoStream(memoryStream, DESalg.CreateEncryptor(key_b, IV), CryptoStreamMode.Write);

        //    byte[] toEncrypt = ConvertUtill.FromString(data, ConvertMode.UTF8);

        //    cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
        //    cryptoStream.FlushFinalBlock();

        //    byte[] ret = memoryStream.ToArray();

        //    cryptoStream.Close();
        //    memoryStream.Close();

        //    return new CipherResponse
        //    {
        //        Key = key,
        //        Data = ConvertUtill.ToString(ret, ConvertMode.BASE64),
        //        IV = BitConverter.ToUInt64(IV, 0),
        //        DataB = ret
        //    };
        //}

        //private CipherResponse DESDecode(string key, string data, ulong IV)
        //{
        //    var key_b = ConvertUtill.FromString(key, ConvertMode.UTF8);

        //    if (key_b.Length != 8)
        //    {
        //        throw new ArgumentException($"Key lenght should be 64-bit. Received key lenght: {key_b.Length}");
        //    }

        //    byte[] toDecrypt = ConvertUtill.FromString(data, ConvertMode.BASE64);

        //    var iv = BitConverter.GetBytes(IV);

        //    var memoryStream = new MemoryStream(toDecrypt);

        //    var DESalg = DES.Create();
        //    DESalg.Padding = PaddingMode.Zeros;

        //    var cryptoStream = new CryptoStream(memoryStream,
        //        DESalg.CreateDecryptor(key_b, iv),
        //        CryptoStreamMode.Read);

        //    byte[] fromEncrypt = new byte[toDecrypt.Length];

        //    cryptoStream.Read(fromEncrypt, 0, fromEncrypt.Length);

        //    var result = ConvertUtill.ToString(fromEncrypt, ConvertMode.UTF8);

        //    return new CipherResponse
        //    {
        //        Key = key,
        //        Data = result
        //    };
        //}
    }
}
