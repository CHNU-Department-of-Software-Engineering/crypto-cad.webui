using System;
using Newtonsoft.Json;
using CryptoCAD.Domain.Entities.Methods;

namespace CryptoCAD.Domain.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CipherEntity
    {
        [JsonProperty]
        public Guid Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public MethodTypes Type { get; set; }
        [JsonProperty]
        public bool IsModifiable { get; set; }
        [JsonProperty]
        public string Configuration { get; set; }
        [JsonProperty]
        public CipherKeyEntity Key { get; set; }
    }
}