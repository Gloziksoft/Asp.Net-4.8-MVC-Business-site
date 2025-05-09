using System;
using System.Globalization;
using System.Text;

namespace dufeksoft.lib.Model.TatraCardPay
{
    /// <summary>
    /// Tatrabanka Cardpay encoding helpers
    /// </summary>
    public abstract class TatraCardPayEncoding
    {
        /// <summary>
        /// Convert UTF-8 string to bytes array
        /// </summary>
        /// <param name="text">Text to convert</param>
        /// <returns>Returns bytes array</returns>
        public static byte[] Utf8ToBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        /// <summary>
        /// Convert hexadecimal string representation to bytes array
        /// </summary>
        /// <param name="hex">Hexadecimal string to convert</param>
        /// <returns>Returns bytes array</returns>
        public static byte[] HexToBytes(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }

        /// <summary>
        /// Convert UTF-8 string to hexadecimal string
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Hexadecimal string</returns>
        public static string Utf8StringToHexString(string str)
        {
            return BytesToHexaString(Utf8ToBytes(str));
        }

        /// <summary>
        /// Bytes array to hexadecimal string
        /// </summary>
        /// <param name="ba">Vytes array</param>
        /// <returns>Hexadecimal string</returns>
        public static string BytesToHexaString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// Convert HMAC265 byte array to string for Tatrabanka Cardpay specification
        /// </summary>
        /// <param name="hash">HMAC256 to convert</param>
        /// <returns>HMAC256 hexadecimal string</returns>
        public static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
