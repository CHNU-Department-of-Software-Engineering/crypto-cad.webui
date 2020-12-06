using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoCAD.Core.Utilities;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.Domain.Repositories;
using CryptoCAD.API.Models.Ciphers;
using CryptoCAD.API.Models.Methods;
using CryptoCAD.Domain.Entities.Methods;
using CryptoCAD.Domain.Entities.Methods.Ciphers;

namespace CryptoCAD.API.Controllers
{
    [ApiController]
    [Route("api/methods")]
    public class MethodsController : ControllerBase
    {
        private readonly ICipherService CipherService;
        private readonly IMethodsRepository MethodsRepository;
        private readonly ILogger<MethodsController> Logger;

        public MethodsController(ICipherService cipherService, IMethodsRepository methodsRepository, ILogger<MethodsController> logger)
        {
            CipherService = cipherService;
            MethodsRepository = methodsRepository;
            Logger = logger;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Method>> GetMethods()
        {
            try
            {
                var methods = MethodsRepository.GetAll();
                Logger.LogInformation("GetMethods");
                return Ok(methods);
            }
            catch (Exception exception)
            {
                Logger.LogError("GetMethods", exception);
                return BadRequest($"Exception occured: {exception}");
            }
            
        }

        [HttpPost]
        [Route("process")]
        public ActionResult<CipherProcessResponse> Process(ProcessRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                {
                    throw new ArgumentNullException("Method Id is required!");
                }

                var type = request.Type.ToMethodType();

                if (type == MethodTypes.Cipher)
                {
                    var mode = request.Cipher.Mode.ToCipherMode();

                    if (mode == CipherModes.None)
                    {
                        throw new NotSupportedException("Only 'encryption' and 'decryption' modes are supported!");
                    }

                    var key = ConvertUtill.FromString(request.Cipher.Key);
                    var data = ConvertUtill.FromString(request.Data, mode == CipherModes.Decrypt ? ConvertMode.BASE64 : ConvertMode.UTF8);

                    var method = MethodsRepository.Get(request.Id);

                    var result = CipherService.Process(method.Name, mode, key, data, request.Configuration);

                    var dataResult = ConvertUtill.ToString(result, mode == CipherModes.Encrypt ? ConvertMode.BASE64 : ConvertMode.UTF8);

                    return Ok(new CipherProcessResponse()
                    {
                        Id = request.Id,
                        Mode = request.Cipher.Mode,
                        Key = request.Cipher.Key,
                        Data = dataResult,
                        Configurations = request.Configuration
                    });
                }
                else
                {
                    throw new NotImplementedException("Not implemented yet!");
                }
            }
            catch (Exception exception)
            {
                Logger.LogError("Process", exception);
                return BadRequest($"Exception occured: {exception}");
            }
        }

        [HttpPost]
        [Route("savechanges")]
        public ActionResult SaveChanges(SaveChangesRequest request)
        {
            try
            {
                var method = new Method
                {
                    Name = request.Name,
                    Type = request.Type.ToMethodType(),
                    IsModifiable = true,
                    IsEditable = true,
                    SecretLength = request.SecretLength,
                    Configuration = request.Configuration
                };

                if (request.Id == Guid.Empty)
                {
                    method.Id = Guid.NewGuid();
                    MethodsRepository.Add(method);
                }
                else
                {
                    var repoMethod = MethodsRepository.Get(request.Id);
                    if (repoMethod is null)
                    {
                        method.Id = request.Id;
                        MethodsRepository.Add(method);
                    }
                    else
                    {
                        if (repoMethod.IsModifiable)
                        {
                            throw new InvalidOperationException($"Method '{repoMethod.Name}' can't be modified!");
                        }
                        else
                        {
                            method.Id = repoMethod.Id;
                            MethodsRepository.Update(method);
                        }
                    }
                }

                MethodsRepository.SaveChanges();
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