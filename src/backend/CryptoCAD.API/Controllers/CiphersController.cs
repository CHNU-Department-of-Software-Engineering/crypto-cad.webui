using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Models.Ciphers;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.Domain.Entities;
using CryptoCAD.Domain.Repositories;
using CryptoCAD.API.Models.Ciphers;

namespace CryptoCAD.API.Controllers
{
    [ApiController]
    [Route("api/ciphers")]
    public class CiphersController : ControllerBase
    {
        private readonly ICipherService CipherService;
        private readonly ICipherEntityRepository CipherRepository;
        private readonly ILogger<CiphersController> Logger;

        public CiphersController(ICipherService cipherService, ICipherEntityRepository cipherRepository, ILogger<CiphersController> logger)
        {
            CipherService = cipherService;
            CipherRepository = cipherRepository;
            Logger = logger;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<CipherEntity>> GetCiphers()
        {
            try
            {
                var ciphers = CipherRepository.GetAll();
                Logger.LogInformation("GetCiphers");
                return Ok(ciphers);
            }
            catch (Exception exception)
            {
                Logger.LogError("GetCiphers", exception);
                return BadRequest($"Exception occured: {exception}");
            }
            
        }

        [HttpPost]
        [Route("process")]
        public ActionResult<CipherProcessResponse> Process(CipherProcessRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                {
                    throw new ArgumentNullException("Cipher Id is required!");
                }
                var operation = request.Mode == "encryption"
                    ? CipherOperations.Encrypt
                    : request.Mode == "decryption"
                    ? CipherOperations.Decrypt
                    : throw new NotSupportedException("Only 'encryption' and 'decryption' modes are supported!");

                var key = ConvertUtill.FromString(request.Key, ConvertMode.UTF8);
                var data = ConvertUtill.FromString(request.Data, operation == CipherOperations.Decrypt ? ConvertMode.BASE64 : ConvertMode.UTF8);

                var cipher = CipherRepository.Get(request.Id);

                var result = CipherService.Process(cipher.Name, operation, key, data, request.Configuration);

                var dataResult = ConvertUtill.ToString(result, operation == CipherOperations.Encrypt ? ConvertMode.BASE64 : ConvertMode.UTF8);

                return Ok(new CipherProcessResponse()
                {
                    Id = request.Id,
                    Mode = request.Mode,
                    Key = request.Key,
                    Data = dataResult,
                    Configurations = request.Configuration
                });
            }
            catch (Exception exception)
            {
                Logger.LogError("Process", exception);
                return BadRequest($"Exception occured: {exception}");
            }
        }

        [HttpPost]
        [Route("savechanges")]
        public ActionResult SaveChanges(CipherSaveChangesRequest request)
        {
            try
            {
                var cipher = new CipherEntity
                {
                    Name = request.Name,
                    Type = request.Type,
                    IsModifiable = true,
                    Configuration = request.Configuration,
                    Key = request.Key
                };

                if (request.Id == Guid.Empty)
                {
                    cipher.Id = Guid.NewGuid();
                    CipherRepository.Add(cipher);
                }
                else
                {
                    var repoCipher = CipherRepository.Get(request.Id);
                    if (repoCipher is null)
                    {
                        cipher.Id = request.Id;
                        CipherRepository.Add(cipher);
                    }
                    else
                    {
                        if (repoCipher.IsModifiable)
                        {
                            throw new InvalidOperationException($"Cipher '{repoCipher.Name}' can't be modified!");
                        }
                        else
                        {
                            cipher.Id = repoCipher.Id;
                            CipherRepository.Update(cipher);
                        }
                    }
                }

                CipherRepository.SaveChanges();
                return Ok("Successfully saved!");
            }
            catch (Exception exception)
            {
                Logger.LogError("SaveChanges", exception);
                return BadRequest($"Exception occured: {exception}");
            }
        }
    }
}