namespace CryptoCAD.Core.DES.Abstractions
{
    public interface IPermutator
    {
        /// <summary>
        /// Procees permutation on 64 block. If initial true - use IP table, if false - IP-1 table
        /// </summary>
        /// <param name="block64"></param>
        /// <param name="initial"></param>
        /// <returns>Permutated block64</returns>
        ulong DoPermutation(ulong block64, bool initial = true);
    }
}
