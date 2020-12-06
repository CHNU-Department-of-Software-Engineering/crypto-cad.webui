using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCAD.API.Models.Methods
{
    public class CipherRequest
    {
        public string Mode { get; set; }
        public string Key { get; set; }
    }
}