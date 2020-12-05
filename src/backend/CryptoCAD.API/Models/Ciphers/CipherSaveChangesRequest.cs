using System;
using CryptoCAD.Domain.Entities;

namespace CryptoCAD.API.Models.Ciphers
{
    public class CipherSaveChangesRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Configuration { get; set; }
        public CipherTypes Type { get; set; }
        public CipherKeyEntity Key { get; set; }
    }
}