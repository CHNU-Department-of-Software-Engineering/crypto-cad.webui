using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using CryptoCAD.API.Models;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace CryptoCAD.API.Controllers
{
    [ApiController]
    [Route("api/des")]
    public class DESController : ControllerBase
    {
        private readonly ILogger<DESController> _logger;

        private readonly DES DES;

        public DESController(ILogger<DESController> logger)
        {
            _logger = logger;
            DES = DES.Create();

            
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult<string> About()
        {
            return Ok("Стандарт шифрования данных DES (DATA ENCRYPTION STANDARD) – блочный шифр с симметричными ключами, разработан Национальным Институтом Стандартов и Технологии (NIST – National Institute of Standards and Technology).");
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult<string> GenerateKey()
        {
            var key = System.Text.Encoding.Unicode.GetString(DES.Key);
            return Ok(key);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public ActionResult<string> SaveKey(string key)
        {
            byte[] bytes = Encoding.Default.GetBytes(key);
            return Ok(Encoding.Default.GetString(bytes));
        }

        [HttpPost]
        [Route("[controller]/encrypt")]
        public ActionResult<DESEncryptResponse> Encrypt(DESEncryptRequest model)
        {
            var key = Encoding.UTF8.GetBytes(model.Key);

            var resultStream = new MemoryStream();

            var DESalg = DES.Create();
            DESalg.Padding = PaddingMode.Zeros;

            var IV = DESalg.IV;

            var cryptoStream = new CryptoStream(resultStream, DESalg.CreateEncryptor(key, IV), CryptoStreamMode.Write);

            var streamWriter = new StreamWriter(cryptoStream);
            streamWriter.WriteLine(model.Data);

            streamWriter.Close();
            cryptoStream.Close();
            resultStream.Close();

            var encryptedData = resultStream.ToArray();

            return new DESEncryptResponse
            {
                Key = model.Key,
                EncryptedData = Encoding.Default.GetString(encryptedData),
                InitializationVector = Encoding.UTF8.GetString(IV)
            };
        }

        [HttpPost]
        [Route("[controller]/decrypt")]
        public ActionResult<DESDecryptResponse> Decrypt(DESDecryptRequest model)
        {
            var key = Encoding.UTF8.GetBytes(model.Key);
            var IV = Encoding.UTF8.GetBytes(model.InitializationVector);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(model.EncryptedData);
            writer.Flush();
            stream.Position = 0;

            var DESalg = DES.Create();
            DESalg.Padding = PaddingMode.Zeros;

            var cryptoStream = new CryptoStream(stream, DESalg.CreateDecryptor(key, DESalg.IV), CryptoStreamMode.Read);

            var streamReader = new StreamReader(cryptoStream);

            var encryptedData = streamReader.ReadLine();

            streamReader.Close();
            cryptoStream.Close();
            stream.Close();

            return new DESDecryptResponse
            {
                Key = model.Key,
                Data = encryptedData
            };
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("[controller]/upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create)) {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
