using System;

namespace CryptoCAD.API.Models.Methods
{
    public class ProcessRequest
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public CipherRequest Cipher { get; set; }
        public HashRequest Hash { get; set; }
        public string Data { get; set; }
        public string Configuration { get; set; }
    }
}