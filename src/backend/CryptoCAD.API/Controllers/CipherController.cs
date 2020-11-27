using CryptoCAD.API.Models;
using CryptoCAD.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace CryptoCAD.API.Controllers
{
    [ApiController]
    [Route("api/cipher")]
    public class CipherController : ControllerBase
    {
        private readonly ILogger<CipherController> Logger;

        public CipherController(ILogger<CipherController> logger)
        {
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
                if (request.Mode == Models.CipherMode.Encypt)
                {
                    var response = DESEncode(request.Key, request.Data);
                    response.CipherId = request.CipherId;
                    response.CipherName = request.CipherName;
                    response.Mode = request.Mode;

                    return Ok(response);
                }
                else if (request.Mode == Models.CipherMode.Decrypt)
                {
                    var response = DESDecode(request.Key, request.Data, request.IV);
                    response.CipherId = request.CipherId;
                    response.CipherName = request.CipherName;
                    response.Mode = request.Mode;
                    response.IV = request.IV;

                    return Ok(response);
                }

                return BadRequest("Cipher mode not specified!");
            }
            catch (Exception exception)
            {
                return BadRequest($"Exception occured: {exception}");
            }
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
                Data = ConvertUtill.ToString(ret, ConvertMode.BASE64),
                IV = BitConverter.ToUInt64(IV, 0),
                DataB = ret
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
