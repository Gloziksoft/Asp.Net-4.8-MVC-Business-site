using System;
using System.Security.Cryptography;
using System.Text;

namespace dufeksoft.lib.Model.VubEPlatby
{
    public class VubEPlatbySignature
    {
        public string Message { get; private set; }
        public string SecurityKey { get; private set; }
        public string Signature { get; private set; }

        public string Error { get; private set; }

        /// <summary>
        /// Vytvorenie podpisu zo spravy
        /// </summary>
        /// <param name="message">Podpisovaná správa</param>
        /// <param name="securityKey">Bezpecnostny kľúč</param>
        /// <returns>Podpis</returns>
        public static VubEPlatbySignature SignData(string message, string securityKey)
        {
            VubEPlatbySignature ret = new VubEPlatbySignature();
            ret.Message = message;
            ret.SecurityKey = securityKey;

            try
            {
                ret.Signature = CreateHMAC_SHA_256(ret.SecurityKey, ret.Message);
            }
            catch (Exception exc)
            {
                ret.Error = exc.ToString();
            }

            return ret;
        }

        private static string CreateHMAC_SHA_256(string key, string message)
        {
            HMACSHA256 hcrypt = new HMACSHA256();

            hcrypt.Key = Encoding.ASCII.GetBytes(key);
            byte[] hash = hcrypt.ComputeHash(Encoding.ASCII.GetBytes(message));

            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
