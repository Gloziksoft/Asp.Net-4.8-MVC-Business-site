using dufeksoft.lib.Localization;
using dufeksoft.lib.Text;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Security.Cryptography;
using System.Web;

namespace dufeksoft.lib.Model.TatraCardPay
{
    /*  Regular expression explanation

    ^                   # Start of string.
    [0-9]+              # Must have one or more numbers.
    (                   # Begin optional group.
        \.              # The decimal point, . must be escaped, 
                        # or it is treated as "any character".
        [0-9]{1,2}      # One or two numbers.
    )?                  # End group, signify it's optional with ?
    $                   # End of string.

     */

    public class TatraCardPayRequest
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

        public string Rs_Title = TatraCardPayRequest.RM.GetString("TbCardPayTitle");
        public string Rs_SubTitle = TatraCardPayRequest.RM.GetString("TbCardPaySubTitle");
        public string Rs_FormTitle = TatraCardPayRequest.RM.GetString("TbCardPayFormTitle");
        public string Rs_ErrTitle = TatraCardPayRequest.RM.GetString("TbCardPayErrTitle");
        public string Rs_SecureInfo = TatraCardPayRequest.RM.GetString("TbCardPaySecureInfo");
        public string Rs_BtnSend = TatraCardPayRequest.RM.GetString("TbCardPayBtnSend");
        public string Rs_RegularPayment = TatraCardPayRequest.RM.GetString("TbCardPayRegularPayment");
        public string Rs_AdvancePayment = TatraCardPayRequest.RM.GetString("TbCardPayAdvancePayment");
        public string Rs_Redirecting = TatraCardPayRequest.RM.GetString("TbCardPayRedirectingToGateway");
        public string Rs_Lang = TatraCardPayRequest.RM.GetString("TbCardPayLang");

        public string Key { get; set; }
        public string PaymentUrl { get; set; }
        public string MID { get; set; }


        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "TatraCardPayRequiredAmount")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "TatraCardPayLabelAmount")]
        [RegularExpression(@"^[0-9]+([\.,][0-9]?[0-9]?)?$", ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "TatraCardPayWrongAmount")]
        public string AMT { get; set; }
        public string CURR { get; set; }

        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "TatraCardPayRequiredVs")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "TatraCardPayLabelVs")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "TatraCardPayWrongVs")]
        public string VS { get; set; }

        public const string TranType_PreAuth = "TranType_PreAuth";
        public const string TranType_Auth = "TranType_Auth";
        public string TransType { get; set; }
        public string TXN
        {
            get
            {
                return this.TransType == TranType_PreAuth ? "PA" : string.Empty;
            }
        }
        public string RURL { get; set; }
        public string IPC { get; set; }

        public const string RegEx_Name = "^[0-9a-zA-Z @._-]*$";

        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "TatraCardPayRequiredName")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "TatraCardPayLabelName")]
        [RegularExpression(RegEx_Name, ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "TatraCardPayWrongName")]
        public string NAME { get; set; }
        public string REM { get; set; }
        public string TPAY { get; set; }
        public string CID { get; set; }
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

        public TatraCardPayRequest()
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

            this.REM = TatraCardPayCfg.GetEmail();
            this.PaymentUrl = "https://moja.tatrabanka.sk/cgi-bin/e-commerce/start/cardpay";
            this.AREDIR = "0";
            this.LANG = this.Rs_Lang;
            if (HttpContext.Current.Request.Url.Host == "localhost")
            {
                this.IPC = "1.2.3.4";
            }
            else
            {
                this.IPC = HttpContext.Current.Request.UserHostAddress;
            }
            this.TransType = TranType_Auth;
        }

        public string ValidateRequestParam_Name(string par)
        {
            if (string.IsNullOrEmpty(par))
            {
                return par;
            }

            par = StringHelper.RemoveDiacritics(par);
            if (string.IsNullOrEmpty(par))
            {
                return par;
            }

            return StringHelper.KeepOnlyValidCharacters(par, RegEx_Name);
        }

        public string CalculateHMAC()
        {
            string message = MID + AMT + CURR + VS + RURL + IPC + NAME + REM + TIMESTAMP;
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
    }
}
