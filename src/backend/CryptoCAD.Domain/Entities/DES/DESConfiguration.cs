using Newtonsoft.Json;

namespace CryptoCAD.Domain.Entities.DES
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DESConfiguration
    {
        [JsonProperty]
        public int[] InitialPermutationTable { get; set; }
        [JsonProperty]
        public int[] FinalPermutationTable { get; set; }
        [JsonProperty]
        public int[] ExpansionPermutationTable { get; set; }
        [JsonProperty]
        public int[][] SubstitutionBoxes { get; set; }
        [JsonProperty]
        public int[] PermutationTable { get; set; }
        [JsonProperty]
        public int[] Pc1PermutationTable { get; set; }
        [JsonProperty]
        public int[] Pc2PermutationTable { get; set; }
        [JsonProperty]
        public int[] RotationsTable { get; set; }
    }
}