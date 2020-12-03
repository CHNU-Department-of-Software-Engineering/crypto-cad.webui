namespace CryptoCAD.Core.Ciphers.DES.Structure.Abstractions
{
    internal interface IRound
    {
        (short, short) Process(short leftBlock32b, short rightBlock32b, int key48b);
    }
}