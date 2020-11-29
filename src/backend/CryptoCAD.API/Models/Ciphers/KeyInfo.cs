namespace CryptoCAD.API.Models.Ciphers
{
    public class KeyInfo
    {
        private const byte CharEncodingLenght = 8;

        public string Type { get; set; }
        public byte Lenght { get; set; }
        public short BitLenght => (short)(Lenght * CharEncodingLenght);
    }
}