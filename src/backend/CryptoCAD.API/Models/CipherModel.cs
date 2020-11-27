using System;

namespace CryptoCAD.API.Models
{
    public class CipherModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int KeyLenghtBits { get; set; }
    }
}
