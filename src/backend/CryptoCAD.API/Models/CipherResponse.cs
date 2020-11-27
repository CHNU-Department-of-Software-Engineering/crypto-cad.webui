using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCAD.API.Models
{
    public class CipherResponse
    {
        public Guid CipherId { get; set; }
        public string CipherName { get; set; }
        public CipherMode Mode { get; set; }
        public string Key { get; set; }
        public string Data { get; set; }
        public byte[] DataB { get; set; }
        public ulong IV { get; set; }
    }
}
