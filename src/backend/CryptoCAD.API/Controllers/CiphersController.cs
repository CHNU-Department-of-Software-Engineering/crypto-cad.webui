using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoCAD.API.Models;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Models.Ciphers;
using CryptoCAD.Core.Services.Abstractions;

namespace CryptoCAD.API.Controllers
{
    [ApiController]
    [Route("api/ciphers")]
    public class CiphersController : ControllerBase
    {
        private readonly ICipherService CipherService;
        private readonly ILogger<CiphersController> Logger;

        public CiphersController(ICipherService cipherService, ILogger<CiphersController> logger)
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

                var result = CipherService.Process(request.Name, operation, key, data);

                var dataResult = ConvertUtill.ToString(result, operation == CipherOperations.Encrypt ? ConvertMode.BASE64 : ConvertMode.UTF8);

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
    }
}