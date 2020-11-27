using System;

namespace CryptoCAD.API.Models
{
    public class CipherRequest
    {
        public Guid CipherId { get; set; }
        public string CipherName { get; set; }
        public CipherMode Mode { get; set; }
        public string Key { get; set; }
        public string Data { get; set; }
        public ulong IV { get; set; }
    }
}
