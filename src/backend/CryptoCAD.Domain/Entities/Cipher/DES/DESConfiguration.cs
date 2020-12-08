namespace CryptoCAD.Domain.Entities.Ciphers.DES
{
    public class DESConfiguration
    {
        public int[] InitialPermutationTable { get; set; }
        public int[] FinalPermutationTable { get; set; }
        public int[] ExpansionPermutationTable { get; set; }
        public int[] PermutationTable { get; set; }
        public int[] Pc1PermutationTable { get; set; }
        public int[] Pc2PermutationTable { get; set; }
        public int[] RotationsTable { get; set; }

        public int[] SBox1 { get; set; }
        public int[] SBox2 { get; set; }
        public int[] SBox3 { get; set; }
        public int[] SBox4 { get; set; }
        public int[] SBox5 { get; set; }
        public int[] SBox6 { get; set; }
        public int[] SBox7 { get; set; }
        public int[] SBox8 { get; set; }
    }
}