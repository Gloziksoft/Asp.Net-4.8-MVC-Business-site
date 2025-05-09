using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace dufeksoft.lib.Model.TatraPay
{
    public class TatraPayResponse
    {
        public string ResultInfo { get; set; }
        public string SecurityInfo { get; set; }

        public string Key { get; set; }
        public string MID { get; set; }

        public string AMT { get; set; }
        public string CURR { get; set; }
        public string VS { get; set; }
        public string SS { get; set; }
        public string CS { get; set; }
        public string RES { get; set; }
        public string TID { get; set; }
        public string TIMESTAMP { get; set; }
        public string HMAC_RESPONSE { get; set; }
        public string HMAC
        {
            get
            {
                return CalculateHMAC();
            }
        }
        public string ECDSA_KEY { get; set; }
        public string ECDSA { get; set; }


        public TatraPayResponse(string mid, string key, HttpRequest request)
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
            LoadTatraPayResponseData(request);
        }
        private void LoadTatraPayResponseData(HttpRequest request)
        {
            this.AMT = request.Params["AMT"];
            this.CURR = request.Params["CURR"];
            this.VS = request.Params["VS"];
            this.SS = request.Params["SS"];
            this.CS = request.Params["CS"];
            this.RES = request.Params["RES"]; /* môže byť "OK", "FAIL", "TOUT" */
            this.TID = request.Params["TID"];
            this.TIMESTAMP = request.Params["TIMESTAMP"];
            this.HMAC_RESPONSE = request.Params["HMAC"];
            this.ECDSA = request.Params["ECDSA"];
            this.ECDSA_KEY = request.Params["ECDSA_KEY"];
        }

        public string CalculateHMAC()
        {
            string message = AMT + CURR + VS + SS + CS + RES + TID + TIMESTAMP;
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



        public bool VerifySignature(string publicKeyFile)
        {
            string stringToVerify = AMT + CURR + VS + SS + CS + RES + TID + TIMESTAMP + HMAC;
            string signature = ECDSA;

            // Tatrapay default test
            //stringToVerify = "1234.5097811110308OK101092014125505ff1780ef346419d8460dd7f9dec48506524effdb6d2c9739ac44bab07a28b80f";
            //signature = "304502203e610bc0ce7f391f26aa6bcbbc47c5e5a76627cba635b694db087370a86b061a0221008ca764709d25d904d67290a440eee8250473042af128a18261ffde9e8a1f8a89";
            var fileStream = File.OpenText(publicKeyFile);
            var pemReader = new PemReader(fileStream);
            ECPublicKeyParameters keyParameter = (ECPublicKeyParameters)pemReader.ReadObject();

            //create a signerutility with type SHA-256withECDSA 
            ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
            //initial signer with the public key 
            signer.Init(false, keyParameter);
            //get signature in bytes : digitalsignature parameter contains signature that should be used. 
            byte[] signatureBytes = TatraPayEncoding.HexToBytes(signature);
            //block/finalise update to signer : data : is the actual data. 
            byte[] toVerifyBytes = Encoding.UTF8.GetBytes(stringToVerify);
            signer.BlockUpdate(toVerifyBytes, 0, toVerifyBytes.Length);
            //verify signature 
            bool isValid = signer.VerifySignature(signatureBytes);
            return isValid;
        }
    }
}
