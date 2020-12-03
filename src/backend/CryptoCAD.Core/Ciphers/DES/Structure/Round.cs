using CryptoCAD.Core.Ciphers.DES.Structure.Abstractions;

namespace CryptoCAD.Core.Ciphers.DES.Structure
{
    internal class Round : IRound
    {
        private readonly IFunction Function;

        public Round(IFunction function)
        {
            Function = function;
        }

        public (short, short) Process(short leftBlock32b, short rightBlock32b, int key48b)
        {
            var newRightBlock32b = Function.Process(rightBlock32b, key48b);
            newRightBlock32b ^= leftBlock32b;
            return (rightBlock32b, newRightBlock32b);
        }
    }
}