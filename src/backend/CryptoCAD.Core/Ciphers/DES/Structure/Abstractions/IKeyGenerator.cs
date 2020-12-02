using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCAD.Core.Ciphers.DES.Structure.Abstractions
{
    internal interface IKeyGenerator
    {
        byte[][] GenerateSubkeys(byte[] key);
    }
}