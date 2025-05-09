namespace dufeksoft.lib.Model.VubEPlatby
{
    /// <summary>
    /// https://nib.vub.sk/nepay/merchant
    /// </summary>
    public class VubEPlatbyRequest
    {
        /// <summary>
        /// ID obchodu
        /// </summary>
        public string MID { get; set; }
        /// <summary>
        /// Suma EUR
        /// </summary>
        public string AMT { get; set; }
        /// <summary>
        /// Variabilny symbol
        /// </summary>
        public string VS { get; set; }
        /// <summary>
        /// Konstantny symbol
        /// </summary>
        public string CS { get; set; }
        /// <summary>
        /// Specificky symbol
        /// </summary>
        public string SS { get; set; }
        /// <summary>
        /// Navratova URL obchodu po uspesnej alebo aj neuspesnej platbe
        /// </summary>
        public string RURL { get; set; }
        /// <summary>
        /// Bezpecnostny kod
        /// </summary>
        public string SIGN { get; set; }


        public string VubeplatbyUrl { get; private set; }

        public VubEPlatbyRequest(bool useProd = true)
        {
            if (useProd)
            {
                this.VubeplatbyUrl = "https://ib.vub.sk/e-platbyeuro.aspx";
            }
            else
            {
                this.VubeplatbyUrl = "https://nib.vub.sk/nepay/merchant";
            }
        }

        public VubEPlatbySignature CreateSignature(string securityKey)
        {
            string message = string.Format("{0}{1}{2}{3}{4}{5}",
                MID,
                AMT,
                VS,
                SS,
                CS,
                RURL);

            return VubEPlatbySignature.SignData(message, securityKey);
        }
    }
}
