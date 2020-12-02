using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Models.Ciphers;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.API.Models.Ciphers;
using CryptoCAD.Domain.Repositories;
using CryptoCAD.API.Models.Ciphers.DES;
using System.Linq;

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
                        Length = 8
                    }
                },
                new CipherInfo
                {
                    Name = "aes",
                    Key = new KeyInfo
                    {
                        Type = typeof(string).Name,
                        Length = 16
                    }
                },
                new CipherInfo
                {
                    Name = "gost",
                    Key = new KeyInfo
                    {
                        Type = typeof(string).Name,
                        Length = 32
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
        [Route("des/permutation/tables")]
        public ActionResult<PermutationTablesResponse> GetDesPermutationTables()
        {
            return Ok(new PermutationTablesResponse
            {
                IP = INITIAL_PERMUTATION_TABLE,
                FP = FINAL_PERMUTATION_TABLE
            });
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

        private readonly int[] INITIAL_PERMUTATION_TABLE = {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9,  1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        private readonly int[] FINAL_PERMUTATION_TABLE = {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };
    }
}