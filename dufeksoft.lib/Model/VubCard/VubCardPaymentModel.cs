using dufeksoft.lib.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Security.Cryptography;
using System.Text;

namespace dufeksoft.lib.Model.VubCard
{
    public class VubCardPaymentModel
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

        public string Rs_Title = VubCardPaymentModel.RM.GetString("VubTitle");
        public string Rs_SubTitle = VubCardPaymentModel.RM.GetString("VubSubTitle");
        public string Rs_FormTitle = VubCardPaymentModel.RM.GetString("VubFormTitle");
        public string Rs_ErrTitle = VubCardPaymentModel.RM.GetString("VubErrTitle");
        public string Rs_SecureInfo = VubCardPaymentModel.RM.GetString("VubSecureInfo");
        public string Rs_BtnSend = VubCardPaymentModel.RM.GetString("VubBtnSend");
        public string Rs_RegularPayment = VubCardPaymentModel.RM.GetString("VubRegularPayment");
        public string Rs_AdvancePayment = VubCardPaymentModel.RM.GetString("VubAdvancePayment");
        public string Rs_Redirecting = VubCardPaymentModel.RM.GetString("VubRedirectingToGateway");
        public string Rs_Lang = VubCardPaymentModel.RM.GetString("VubLang");

        public const string TranType_PreAuth = "PreAuth";
        public const string TranType_Auth = "Auth";

        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "VubRequiredAmount")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "VubLabelAmount")]
        public string Amount { get; set; }
        [Required(ErrorMessageResourceType = typeof(DufeksoftLibResource), ErrorMessageResourceName = "VubRequiredOid")]
        [Display(ResourceType = typeof(DufeksoftLibResource), Name = "VubLabelOid")]
        public string Oid { get; set; }
        public string TranType { get; set; }
        public string OkUrl { get; set; }
        public string FailUrl { get; set; }

        //public string PaymentUrl = "https://testsecurepay.intesasanpaolocard.com/fim/est3dgate"; // testovacia platobna brana
        public string PaymentUrl = "https://vub.eway2pay.com/fim/est3dgate"; // ostra platobna brana

        public string ClientId = "";
        //public string StoreKey = "";
        public string StoreKey = "";
        public string Currency = "TL";
        public string Instalment = "";
        public string MsAuthType = "";
        public string MsKey = "";
        public string Rnd = DateTime.Now.Ticks.ToString();
        public string Lang = "en";

        public string Hash
        {
            get
            {
                return CreateHash();
            }
        }

        private string CreateHash()
        {
            string strToHash = this.ClientId + "|" + this.Oid + "|" + this.Amount + "|" +
                    this.OkUrl + "|" + this.FailUrl + "|" + this.TranType + "|" +
                    this.Instalment + "|" + this.Rnd + "|" +
                    this.MsAuthType + "|" + this.MsKey + "|" + "|" + this.Currency + "|" + this.StoreKey;

            using (SHA512Managed sha512 = new SHA512Managed())
            {
                var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(strToHash));
                return Convert.ToBase64String(hash);
            }
        }

        public VubCardPaymentModel()
        {
            this.TranType = VubCardPaymentModel.TranType_Auth;
            this.Lang = this.Rs_Lang;
        }
    }
}
