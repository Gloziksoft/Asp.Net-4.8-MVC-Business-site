using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace dufeksoft.lib.Model.Sporopay
{
    public class SporopaySignature
    {
        public string Message { get; private set; }
        public string SecretKey { get; private set; }
        public string Signature { get; private set; }
        public string SignatureUrlEncoded
        {
            get
            {
                return HttpUtility.UrlEncode(this.Signature);
            }
        }

        public string Error { get; private set; }

        /// <summary>
        /// Vytvorenie podpisu zo spravy
        /// </summary>
        /// <param name="message">Podpisovaná správa</param>
        /// <param name="secretKey">Tajný kľúč</param>
        /// <returns>Podpis</returns>
        public static SporopaySignature SignData(string message, string secretKey)
        {
            SporopaySignature ret = new SporopaySignature();
            ret.Message = Utf8ToAscii(message);
            ret.SecretKey = secretKey;

            try
            {
                // Create hash to be signed
                byte[] msgData = Encoding.ASCII.GetBytes(ret.Message);
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] hashResult = sha.ComputeHash(msgData);
                // Hash result should be 20 bytes array
                // Add 4 more bytes to get 24 bytes array
                // Add 4 bytes with #FF value
                byte[] toSignArray = new byte[hashResult.Length + 4];
                for (int i = 0; i < toSignArray.Length; i++)
                {
                    if (i < hashResult.Length)
                    {
                        toSignArray[i] = hashResult[i];
                    }
                    else
                    {
                        toSignArray[i] = 0xff;
                    }
                }

                // Get secret key bytes
                byte[] secretKeyBytes = Convert.FromBase64String(secretKey);
                // Encrypt hash
                byte[] signatureBytes = Encrypt(toSignArray, secretKeyBytes);

                ret.Signature = Convert.ToBase64String(signatureBytes);
            }
            catch (Exception exc)
            {
                ret.Error = exc.ToString();
            }

            return ret;
        }
        public static string Utf8ToAscii(string input)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding ascii = Encoding.ASCII;

            return ascii.GetString(Encoding.Convert(utf8, ascii, utf8.GetBytes(input)));
        }

        static byte[] Encrypt(byte[] dataToEncrypt, byte[] secretKey)
        {
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = secretKey;
                tdes.Mode = CipherMode.CBC;
                tdes.IV = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                // Create a encryptor  
                ICryptoTransform encryptor = tdes.CreateEncryptor();
                // Encrypt data
                byte[] result = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);

                // Get onyl 24 bytes of encoded result
                byte[] result24Bytes = new byte[24];
                for (int i = 0; i < 24; i++)
                {
                    result24Bytes[i] = result[i];
                }

                return result24Bytes;
            }
        }
    }
}
