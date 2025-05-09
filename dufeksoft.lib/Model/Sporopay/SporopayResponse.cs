using System.Web;

namespace dufeksoft.lib.Model.Sporopay
{
    /// <summary>
    /// https://www.slsp.sk/sk/kalkulacky/sporopay
    /// </summary>
    public class SporopayResponse
    {
        /// <summary>
        /// OK - klient potvrdil platbu
        /// NOK - klient odmietol platbu
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// OK - banka transakciu spracovala - ak to prebehlo v bankovom systeme do 3 sekund
        /// NOK - transkacia nie je zrealizovana v case odoslania odpoverde e-shopu
        ///       mozno ju klient zamietol vid. parameter result
        ///       alebo nebola spracovana v priebehu 3 sekund, t.j. v case odoslania odpovede e-shopu
        /// </summary>
        public string real { get; set; }


        /// <summary>
        /// Predčíslo účtu.
        /// </summary>
        public string u_predcislo { get; set; }
        /// <summary>
        /// Číslo účtu
        /// </summary>
        public string u_cislo { get; set; }
        /// <summary>
        /// Kód banky
        /// </summary>
        public string u_kbanky { get; set; }
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
        /// Hodnota interneho parametra e-shopu predavaneho cez atribut param, napr. 20190003
        /// </summary>
        public string internalParam { get; set; }

        /// <summary>
        /// Podpis spravy
        /// </summary>
        public string sign2 { get; set; }

        public bool IsSignatureValid { get; private set; }
        public bool WasRequestValid { get; private set; }
        public bool WasPaymentValid { get; private set; }

        public SporopayResponse()
        {
        }

        public SporopayResponse(HttpRequest httpRequest, string secretKey, string internalParamName)
        {
            this.result = GetRequestParam(httpRequest, "result");
            this.real = GetRequestParam(httpRequest, "real");

            this.u_predcislo = GetRequestParam(httpRequest, "u_predcislo");
            this.u_cislo = GetRequestParam(httpRequest, "u_cislo");
            this.u_kbanky = GetRequestParam(httpRequest, "u_kbanky");
            this.pu_predcislo = GetRequestParam(httpRequest, "pu_predcislo");
            this.pu_cislo = GetRequestParam(httpRequest, "pu_cislo");
            this.pu_kbanky = GetRequestParam(httpRequest, "pu_kbanky");

            this.suma = GetRequestParam(httpRequest, "suma");
            this.mena = GetRequestParam(httpRequest, "mena");
            this.vs = GetRequestParam(httpRequest, "vs");
            this.ss = GetRequestParam(httpRequest, "ss");
            this.url = GetRequestParam(httpRequest, "url");
            this.param = GetRequestParam(httpRequest, "param");
            this.internalParam = GetRequestParam(httpRequest, internalParamName);

            this.sign2 = GetRequestParam(httpRequest, "sign2");

            if (this.result == "OK")
            {
                SporopaySignature signature = CreateSignature(secretKey);
                this.IsSignatureValid = this.sign2 == signature.Signature;
                this.WasPaymentValid = this.IsSignatureValid;
            }

            this.WasRequestValid = (this.result == "OK" || this.result == "NOK") && (this.real == "OK" || this.real == "NOK");
        }

        public SporopaySignature CreateSignature(string secretKey)
        {
            string message = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}",
                u_predcislo,
                u_cislo,
                u_kbanky,
                pu_predcislo,
                pu_cislo,
                pu_kbanky,
                suma,
                mena,
                vs,
                ss,
                url,
                param,
                result,
                real);

            return SporopaySignature.SignData(message, secretKey);
        }

        private string GetRequestParam(HttpRequest httpRequest, string paramName)
        {
            return httpRequest.QueryString.Get(paramName);
        }
    }
}
