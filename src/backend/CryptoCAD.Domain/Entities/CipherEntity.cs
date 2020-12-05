using System;
using Newtonsoft.Json;

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
        public CipherTypes Type { get; set; }
        [JsonProperty]
        public bool IsModifiable { get; set; }
        [JsonProperty]
        public string Configuration { get; set; }
        [JsonProperty]
        public CipherKeyEntity Key { get; set; }
    }
}