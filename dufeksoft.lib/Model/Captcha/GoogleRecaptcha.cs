using Newtonsoft.Json.Linq;
using System.Net;

namespace dufeksoft.lib.Model.Captcha
{
    public class GoogleRecaptcha
    {
        private string SiteKey { get; set; }
        private string SecretKey { get; set; }

        public GoogleRecaptcha() : this("6Lc51VkUAAAAAAVk9NmsC737zXA_RL2AR77m3DiY", "6Lc51VkUAAAAAAVj6QmU2ZXP6YVOfsEQkK9XwNek")
        {

        }

        public GoogleRecaptcha(string siteKey, string secretKey)
        {
            this.SiteKey = siteKey;
            this.SecretKey = secretKey;
        }

        public bool IsValidRecaptcha()
        {
            string recaptchaToken = System.Web.HttpContext.Current.Request.Params["g-recaptcha-response"];
            if (string.IsNullOrEmpty(recaptchaToken))
            {
                return false;
            }

            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", this.SecretKey, recaptchaToken));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            return status;
        }

        public string HtmlHead(string lang)
        {
            if (string.IsNullOrEmpty(lang))
            {
                return "<script src='https://www.google.com/recaptcha/api.js'></script>";
            }

            return string.Format("<script src='https://www.google.com/recaptcha/api.js?hl={0}'></script>", lang);
        }

        public string HtmlBody()
        {
            return string.Format("<div class='g-recaptcha' data-sitekey='{0}'></div>", this.SiteKey);
        }
    }
}
