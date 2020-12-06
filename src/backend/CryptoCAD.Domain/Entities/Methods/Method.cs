using System;

namespace CryptoCAD.Domain.Entities.Methods
{
    public class Method
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MethodTypes Type { get; set; }
        public bool IsModifiable { get; set; }
        public bool IsEditable { get; set; }
        public byte SecretLength { get; set; }
        public string Configuration { get; set; }
    }
}