using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Models.Ciphers;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.API.Models.Ciphers;
using CryptoCAD.Domain.Repositories;

namespace CryptoCAD.API.Controllers
{
    [ApiController]
    [Route("api/ciphers")]
    public class CiphersController : ControllerBase
    {
        private readonly ICipherService CipherService;
        private readonly ILogger<CiphersController> Logger;
        //private readonly ICipherSetupRepository CipherSetupRepository;

        public CiphersController(ICipherService cipherService, ILogger<CiphersController> logger)//, ICipherSetupRepository cipherSetupRepository)
        {
            CipherService = cipherService;
            Logger = logger;
            //CipherSetupRepository = cipherSetupRepository;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<CipherInfo>> GetCiphers()
        {
            var ciphersInfo = new List<CipherInfo>()
            {
                new CipherInfo
                {
                    Name = "des",
                    Key = new KeyInfo
                    {
                        Type = typeof(string).Name,
                        Lenght = 8
                    }
                },
                new CipherInfo
                {
                    Name = "aes",
                    Key = new KeyInfo
                    {
                        Type = typeof(string).Name,
                        Lenght = 16
                    }
                },
                new CipherInfo
                {
                    Name = "gost",
                    Key = new KeyInfo
                    {
                        Type = typeof(string).Name,
                        Lenght = 32
                    }
                }
            };

            return Ok(ciphersInfo);
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

        [HttpGet]
        [Route("testdb")]
        public ActionResult Go()
        {
            var entity = new Domain.Entities.CipherSetup
            {
                Id = new Guid(),
                Name = "Test test test"
            };

            //CipherSetupRepository.Add(entity);

            return Ok();
        }
    }
}