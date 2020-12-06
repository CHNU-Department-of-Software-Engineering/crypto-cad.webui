using System;

namespace CryptoCAD.API.Models.Methods
{
    public class SaveChangesRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsModifiable { get; set; }
        public bool IsEditable { get; set; }
        public byte SecretLength { get; set; }
        public string Configuration { get; set; }
    }
}