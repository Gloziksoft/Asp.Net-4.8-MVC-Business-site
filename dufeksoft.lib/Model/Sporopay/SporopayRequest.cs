namespace dufeksoft.lib.Model.Sporopay
{
    /// <summary>
    /// https://www.slsp.sk/sk/kalkulacky/sporopay
    /// </summary>
    public class SporopayRequest
    {
        /// <summary>
        /// Predčíslo účtu.
        /// </summary>
        public string pu_predcislo { get; set; }
        /// <summary>
        /// Číslo účtu
        /// </summary>
        public string pu_cislo { get; set; }
        /// <summary>
        /// Kód banky
        /// </summary>
        public string pu_kbanky { get; set; }
        /// <summary>
        /// Suma
        /// </summary>
        public string suma { get; set; }
        /// <summary>
        /// Mena
        /// </summary>
        public string mena { get; set; }
        /// <summary>
        /// Variabilný symbol
        /// </summary>
        public string vs { get; set; }
        /// <summary>
        /// Špecifický symbol
        /// </summary>
        public string ss { get; set; }
        /// <summary>
        /// Parameter e-shopu, napr. cislo objednavky objid=20190003
        /// </summary>
        public string param { get; set; }
        /// <summary>
        /// URL e-shopu pre návrat po vykonaní alebo zrušení platby
        /// </summary>
        public string url { get; set; }


        /// <summary>
        /// Podpis spravy
        /// </summary>
        public string sign1 { get; set; }


        public string SporopayUrl { get; private set; }

        public SporopayRequest()
        {
            this.SporopayUrl = "https://ib.slsp.sk/epayment/epayment/epayment.xml";

            this.pu_kbanky = "0900";
            this.mena = "EUR";
        }

        public SporopaySignature CreateSignature(string secretKey)
        {
            string message = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                pu_predcislo,
                pu_cislo,
                pu_kbanky,
                suma,
                mena,
                vs,
                ss,
                url,
                param);

            return SporopaySignature.SignData(message, secretKey);
        }
    }
}
