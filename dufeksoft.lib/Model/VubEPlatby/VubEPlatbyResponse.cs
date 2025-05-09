using System.Web;

namespace dufeksoft.lib.Model.VubEPlatby
{
    /// <summary>
    /// https://nib.vub.sk/epay/merchant
    /// </summary>
    public class VubEPlatbyResponse
    {
        /// <summary>
        /// OK - platba bola zrealizovana
        /// FAIL - platba nebola zrealizovana
        /// </summary>
        public string RES { get; set; }
        /// <summary>
        /// Variabilny symbol
        /// </summary>
        public string VS { get; set; }
        /// <summary>
        /// Specificky symbol
        /// </summary>
        public string SS { get; set; }
        /// <summary>
        /// Bezpecnostny kod
        /// </summary>
        public string SIGN { get; set; }

        public bool IsSignatureValid { get; private set; }
        public bool WasPaymentValid { get; private set; }

        public VubEPlatbyResponse()
        {
        }

        public VubEPlatbyResponse(HttpRequest httpRequest, string securityKey)
        {
            this.RES = GetRequestParam(httpRequest, "RES");
            this.VS = GetRequestParam(httpRequest, "VS");
            this.SS = GetRequestParam(httpRequest, "SS");
            this.SIGN = GetRequestParam(httpRequest, "SIGN");

            this.IsSignatureValid = false;
            this.WasPaymentValid = false;
            if (this.RES == "OK")
            {
                VubEPlatbySignature signature = CreateSignature(securityKey);
                this.IsSignatureValid = this.SIGN == signature.Signature;
                this.WasPaymentValid = this.IsSignatureValid;
            }
        }

        public VubEPlatbySignature CreateSignature(string securityKey)
        {
            string message = string.Format("{0}{1}{2}",
                VS,
                SS,
                RES);

            return VubEPlatbySignature.SignData(message, securityKey);
        }

        private string GetRequestParam(HttpRequest httpRequest, string paramName)
        {
            return httpRequest.QueryString.Get(paramName);
        }
    }
}
