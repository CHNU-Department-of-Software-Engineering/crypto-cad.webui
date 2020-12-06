namespace CryptoCAD.Core.Services.Abstractions
{
    public interface IHashService
    {
        string Hash(string name, string data, string configuration);
    }
}