namespace CryptoCAD.Core.DES.Abstractions
{
    public interface IFunction
    {
        uint DoFunction(uint block32, ulong key48);
    }
}
