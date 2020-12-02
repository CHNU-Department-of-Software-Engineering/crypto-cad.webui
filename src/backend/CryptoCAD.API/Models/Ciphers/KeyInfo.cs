namespace CryptoCAD.API.Models.Ciphers
{
    public class KeyInfo
    {
        private const byte CharEncodingLenght = 8;

        public string Type { get; set; }
        public byte Length { get; set; }
        public short BitLenght => (short)(Length * CharEncodingLenght);
    }
}