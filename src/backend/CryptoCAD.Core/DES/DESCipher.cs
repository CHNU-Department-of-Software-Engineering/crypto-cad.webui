using CryptoCAD.Core.DES.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCAD.Core.DES
{
    public class DESCipher
    {
        private readonly IPermutator Permutator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="fp"></param>
        public DESCipher(IPermutator permutator)
        {
            this.Permutator = permutator;
        }
    }
}
