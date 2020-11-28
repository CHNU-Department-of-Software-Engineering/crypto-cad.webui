namespace CryptoCAD.Core.Models.Ciphers
{
    public class CipherResult
    {
        public string Name { get; set; }
        public CipherOperations Operation { get; set; }
        public byte[] Key { get; set; }
        public byte[] Data { get; set; }
    }
}