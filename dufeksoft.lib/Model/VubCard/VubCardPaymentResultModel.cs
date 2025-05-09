using System.Web;

namespace dufeksoft.lib.Model.VubCard
{
    public class VubCardPaymentResultModel
    {
        public string mdStatus { get; set; }
        public string txstatus { get; set; }
        public string eci { get; set; }
        public string cavv { get; set; }
        public string md { get; set; }
        public string mdErrorMsg { get; set; }

        public string Response { get; set; }
        public string AuthCode { get; set; }
        public string HostRefNum { get; set; }
        public string ProcReturnCode { get; set; }
        public string TransId { get; set; }
        public string ErrMsg { get; set; }
        public string ClientIp { get; set; }
        public string ReturnOid { get; set; }
        public string MaskedPan { get; set; }
        public string PaymentMethod { get; set; }

        public VubCardPaymentResultModel()
        {
            this.mdStatus = HttpContext.Current.Request.Params["mdStatus"];
            this.txstatus = HttpContext.Current.Request.Params["txstatus"];
            this.eci = HttpContext.Current.Request.Params["eci"];
            this.cavv = HttpContext.Current.Request.Params["cavv"];
            this.md = HttpContext.Current.Request.Params["md"];
            this.mdErrorMsg = HttpContext.Current.Request.Params["mdErrorMsg"];

            this.Response = HttpContext.Current.Request.Params["Response"];
            this.AuthCode = HttpContext.Current.Request.Params["AuthCode"];
            this.HostRefNum = HttpContext.Current.Request.Params["HostRefNum"];
            this.ProcReturnCode = HttpContext.Current.Request.Params["ProcReturnCode"];
            this.TransId = HttpContext.Current.Request.Params["TransId"];
            this.ErrMsg = HttpContext.Current.Request.Params["ErrMsg"];
            this.ClientIp = HttpContext.Current.Request.Params["ClientIp"];
            this.ReturnOid = HttpContext.Current.Request.Params["ReturnOid"];
            this.MaskedPan = HttpContext.Current.Request.Params["MaskedPan"];
            this.PaymentMethod = HttpContext.Current.Request.Params["PaymentMethod"];
        }
    }
}
