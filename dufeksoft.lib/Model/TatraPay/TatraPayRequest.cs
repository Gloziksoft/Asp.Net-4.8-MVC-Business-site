using System.Security.Cryptography;

namespace dufeksoft.lib.Model.TatraPay
{
    public class TatraPayRequest
    {
        public string Key { get; set; }
        public string PaymentUrl { get; set; }
        public string MID { get; set; }
        public string AMT { get; set; }
        public string CURR { get; set; }
        public string VS { get; set; }
        public string SS { get; set; }
        public string CS { get; set; }
        public string RURL { get; set; }
        public string REM { get; set; }
        public string TIMESTAMP { get; set; }
        public string AREDIR { get; set; }
        public string LANG { get; set; }
        public string HMAC
        {
            get
            {
                return CalculateHMAC();
            }
        }

        public TatraPayRequest(string mid, string key)
        {
            this.MID = mid;
            if (key.Length == 64)
            {
                // Convert 64 string key to its 128 hexa representation 
                this.Key = TatraPayEncoding.Utf8StringToHexString(key);
            }
            else
            {
                // Key should be in hexa with 128 characters
                this.Key = key;
            }
        }

        public string CalculateHMAC()
        {
            string message = MID + AMT + CURR + VS + SS + CS + RURL + REM + TIMESTAMP;
            return HashHMACHex(Key, message);
        }

        private string HashHMACHex(string keyHex, string message)
        {
            byte[] hash = HashHMAC(TatraPayEncoding.HexToBytes(keyHex), TatraPayEncoding.Utf8ToBytes(message));
            return TatraPayEncoding.HashEncode(hash);
        }

        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
    }
}
