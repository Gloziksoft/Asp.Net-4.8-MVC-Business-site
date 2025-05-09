using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace dufeksoft.lib.Model.TatraCardPay
{
    public class TatraCardPayResponse
    {
        static ResourceManager _rm = null;

        public static ResourceManager RM
        {
            get
            {
                if (_rm == null)
                {
                    _rm = new ResourceManager(typeof(dufeksoft.lib.Localization.DufeksoftLibResource));
                }

                return _rm;
            }
        }
        public string Rs_PaymentOk = TatraCardPayRequest.RM.GetString("TbCardPayPaymentOk");
        public string Rs_PaymentFailed = TatraCardPayRequest.RM.GetString("TbCardPayPaymentFailed");
        public string Rs_PaymentTryLater = TatraCardPayRequest.RM.GetString("TbCardPayPaymentTryLater");
        public string Rs_PaymentWrongSecInfo = TatraCardPayRequest.RM.GetString("TbCardPayPaymentWrongSecInfo");
        public string Rs_PaymentNoSecInfo = TatraCardPayRequest.RM.GetString("TbCardPayPaymentNoSecInfo");

        public bool IsPaymentOk { get; set; }

        public string ResultInfo { get; set; }
        public string SecurityInfo { get; set; }

        public string Key { get; set; }
        public string MID { get; set; }

        public string AMT { get; set; }
        public string CURR { get; set; }
        public string VS { get; set; }
        public string TXN { get; set; }
        public string RES { get; set; }
        public string AC { get; set; }
        public string TRES { get; set; }
        public string CID { get; set; }
        public string RC { get; set; }
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


        public TatraCardPayResponse()
        {
            this.MID = TatraCardPayCfg.GetMID();

            string key = TatraCardPayCfg.GetKey();
            if (key.Length == 64)
            {
                // Convert 64 string key to its 128 hexa representation 
                this.Key = TatraCardPayEncoding.Utf8StringToHexString(key);
            }
            else
            {
                // Key should be in hexa with 128 characters
                this.Key = key;
            }
            LoadTatraCardPayResponseData();

            switch (this.RES)
            {
                case "FAIL":
                    this.ResultInfo = string.Format("{0} {1}", Rs_PaymentFailed, Rs_PaymentTryLater);
                    this.SecurityInfo = "";
                    break;
                case "OK":
                    this.ResultInfo = Rs_PaymentOk;
                    this.IsPaymentOk = true;
                    if (this.HMAC_RESPONSE != this.HMAC || !this.VerifySignature(GetTatraPayPublicKeyFileName(this.ECDSA_KEY)))
                    {
                        this.ResultInfo = Rs_PaymentFailed;
                        this.SecurityInfo = Rs_PaymentWrongSecInfo;
                        this.IsPaymentOk = false;
                    }
                    break;
                default:
                    this.ResultInfo = Rs_PaymentFailed;
                    this.SecurityInfo = Rs_PaymentNoSecInfo;
                    break;
            }
        }
        private string GetTatraPayPublicKeyFileName(string fileNumber)
        {
            if (TatraCardPayCfg.UseTestData())
            {
                // Test
                return string.Format("{0}\\App_Data\\TatraCardPay\\keytest_{1}.txt",
                    System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath), fileNumber);
            }
            else
            {
                return string.Format("{0}\\App_Data\\TatraCardPay\\key_{1}.txt",
                    System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath), fileNumber);
            }
        }

        private void LoadTatraCardPayResponseData()
        {
            this.AMT = HttpContext.Current.Request.Params["AMT"];
            this.CURR = HttpContext.Current.Request.Params["CURR"];
            this.VS = HttpContext.Current.Request.Params["VS"];
            this.TXN = HttpContext.Current.Request.Params["TXN"];
            this.RES = HttpContext.Current.Request.Params["RES"]; /* môže byť "OK", "FAIL", "TOUT" */
            this.AC = HttpContext.Current.Request.Params["AC"];
            this.TRES = HttpContext.Current.Request.Params["TRES"];
            this.CID = HttpContext.Current.Request.Params["CID"];
            this.RC = HttpContext.Current.Request.Params["RC"];
            this.TID = HttpContext.Current.Request.Params["TID"];
            this.TIMESTAMP = HttpContext.Current.Request.Params["TIMESTAMP"];
            this.HMAC_RESPONSE = HttpContext.Current.Request.Params["HMAC"];
            this.ECDSA = HttpContext.Current.Request.Params["ECDSA"];
            this.ECDSA_KEY = HttpContext.Current.Request.Params["ECDSA_KEY"];
        }

        public string CalculateHMAC()
        {
            string message = AMT + CURR + VS + TXN + RES + AC + TRES + CID + RC + TID + TIMESTAMP;
            return HashHMACHex(Key, message);
        }

        private string HashHMACHex(string keyHex, string message)
        {
            byte[] hash = HashHMAC(TatraCardPayEncoding.HexToBytes(keyHex), TatraCardPayEncoding.Utf8ToBytes(message));
            return TatraCardPayEncoding.HashEncode(hash);
        }

        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }



        public bool VerifySignature(string publicKeyFile)
        {
            string stringToVerify = AMT + CURR + VS + TXN + RES + AC + TRES + CID + RC + TID + TIMESTAMP + HMAC;
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
            byte[] signatureBytes = TatraCardPayEncoding.HexToBytes(signature);
            //block/finalise update to signer : data : is the actual data. 
            byte[] toVerifyBytes = Encoding.UTF8.GetBytes(stringToVerify);
            signer.BlockUpdate(toVerifyBytes, 0, toVerifyBytes.Length);
            //verify signature 
            bool isValid = signer.VerifySignature(signatureBytes);
            return isValid;
        }
    }
}
