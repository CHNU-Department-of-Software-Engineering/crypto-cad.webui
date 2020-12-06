using System.Runtime.CompilerServices;
using CryptoCAD.Core.Ciphers.DES.Structure.Abstractions;

[assembly: InternalsVisibleTo("CryptoCAD.Core.Tests")]
namespace CryptoCAD.Core.Ciphers.DES.Structure
{
    internal class Round : IRound
    {
        private readonly IFunction Function;

        public Round(IFunction function)
        {
            Function = function;
        }

        public (uint, uint) Process(uint leftBlock32b, uint rightBlock32b, ulong key48b)
        {
            var newRightBlock32b = Function.Process(rightBlock32b, key48b);
            newRightBlock32b ^= leftBlock32b;
            return (rightBlock32b, newRightBlock32b);
        }
    }
}