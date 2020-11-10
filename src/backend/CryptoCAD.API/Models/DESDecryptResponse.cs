using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCAD.API.Models
{
    public class DESDecryptResponse
    {
        public string Key { get; set; }
        public string Data { get; set; }
    }
}
