namespace CryptoCAD.API.Models
{
    public class DESEncryptResponse
    {
        public string Key { get; set; }
        public string EncryptedData { get; set; }
        public string InitializationVector { get; set; }
    }
}
