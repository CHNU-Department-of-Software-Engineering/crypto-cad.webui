namespace CryptoCAD.Core.Ciphers.DES.Structure.Abstractions
{
    internal interface IKeySchedule
    {
        ulong[] GenerateSubkeys(byte[] key);
    }
}