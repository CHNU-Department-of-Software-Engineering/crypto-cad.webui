using System;
using System.Text;

namespace CryptoCAD.Core.Utilities
{
    public static class ConvertUtill
    {
        public static string ToString(byte[] data, ConvertMode mode = ConvertMode.UTF8)
        {
            switch (mode)
            {
                case ConvertMode.UTF8:
                    return Encoding.UTF8.GetString(data);
                case ConvertMode.ASCII:
                    return Encoding.ASCII.GetString(data);
                case ConvertMode.BASE64:
                    return Convert.ToBase64String(data);
                default:
                    return Encoding.UTF8.GetString(data); ;
            }
        }

        public static byte[] FromString(string data, ConvertMode mode = ConvertMode.UTF8)
        {
            switch (mode)
            {
                case ConvertMode.UTF8:
                    return Encoding.UTF8.GetBytes(data);
                case ConvertMode.ASCII:
                    return Encoding.ASCII.GetBytes(data);
                case ConvertMode.BASE64:
                    return Convert.FromBase64String(data);
                default:
                    return Encoding.UTF8.GetBytes(data); ;
            }
        }
    }

    public enum ConvertMode
    {
        UTF8 = 1,
        ASCII = 2,
        BASE64 = 3
    }
}
