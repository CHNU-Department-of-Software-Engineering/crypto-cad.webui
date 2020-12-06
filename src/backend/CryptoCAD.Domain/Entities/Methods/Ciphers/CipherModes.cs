namespace CryptoCAD.Domain.Entities.Methods.Ciphers
{
    public enum CipherModes
    {
        None = 0,
        Encrypt = 1,
        Decrypt = 2
    }

    public static class CipherModesExtensions
    {
        public static string ToFriendlyString(this CipherModes mode)
        {
            switch (mode)
            {
                case CipherModes.Encrypt:
                    return "encryption";
                case CipherModes.Decrypt:
                    return "decryption";
                case CipherModes.None:
                default:
                    return "none";
            }
        }

        public static CipherModes ToCipherMode(this string type)
        {
            switch (type)
            {
                case "encryption":
                    return CipherModes.Encrypt;
                case "decryption":
                    return CipherModes.Decrypt;
                case "none":
                default:
                    return CipherModes.None;
            }
        }
    }
}