namespace CryptoCAD.Domain.Entities.Methods
{
    public enum MethodTypes
    {
        None = 0,
        Cipher = 1,
        Hash = 2
    }

    public static class MethodTypesExtensions
    {
        public static string ToFriendlyString(this MethodTypes type)
        {
            switch (type)
            {
                case MethodTypes.Cipher:
                    return "cipher";
                case MethodTypes.Hash:
                    return "hash";
                case MethodTypes.None:
                default:
                    return "none";
            }
        }

        public static MethodTypes ToMethodType(this string type)
        {
            switch (type)
            {
                case "cipher":
                    return MethodTypes.Cipher;
                case "hash":
                    return MethodTypes.Hash;
                case "none":
                default:
                    return MethodTypes.None;
            }
        }
    }
}