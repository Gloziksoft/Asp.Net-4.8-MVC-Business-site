using System.Text;
using System.Web;

namespace dufeksoft.lib.Model.GpWebpay
{
    public class GpwpCardPayResponse
    {
        /// <summary>
        /// Přidělené číslo obchodníka.
        /// </summary>
        public string MERCHANTNUMBER { get; set; }

        /// <summary>
        /// Hodnota CREATE_ORDER
        /// </summary>
        public string OPERATION { get; set; }
        /// <summary>
        /// Obsah pole z požadavku.
        /// </summary>
        public string ORDERNUMBER { get; set; }
        /// <summary>
        /// Obsah pole z požadavku, pokud bylo uvedeno.
        /// </summary>
        public string MERORDERNUM { get; set; }
        /// <summary>
        /// Obsah pole z požadavku, pokud bylo uvedeno a nebylo prázdné.
        /// </summary>
        public string MD { get; set; }
        /// <summary>
        /// Udává primární kód, viz „Seznam návratových kódů“.
        /// </summary>
        public string PRCODE { get; set; }
        /// <summary>
        /// Udává sekundární kód, viz „Seznam návratových kódů“.
        /// </summary>
        public string SRCODE { get; set; }
        /// <summary>
        /// Slovní popis chyby, který je jednoznačně dán kombinací PRCODE a SRCODE. Text je zasílán bez diakritiky.
        /// </summary>
        public string RESULTTEXT { get; set; }
        /// <summary>
        /// Hash čísla platební karty. Hash je unikátní hodnota pro každou kartu a každého obchodníka – tj. pokud je platba provedena stejnou kartou u stejného obchodníka je výsledný hash identický, pokud je tatáž karta použita u jiného obchodníka, tak vznikne hash jiný.
        /// </summary>
        public string USERPARAM1 { get; set; }
        /// <summary>
        /// Pole je plněné v závislosti na nastavení vstupních parametrů pro peněženky (MasterPass) a požadované návratové informace (brand platební karty …).
        /// </summary>
        public string ADDINFO { get; set; }
        /// <summary>
        /// Jednoznačný identifikátor platební karty generovaný systémem GP webpay
        /// </summary>
        public string TOKEN { get; set; }
        /// <summary>
        /// Expirace použité platební karty ve formátu YYMM
        /// </summary>
        public string EXPIRY { get; set; }
        /// <summary>
        /// Výsledek autentikace držitele platební karty v systému 3D
        /// </summary>
        public string ACSRES { get; set; }
        /// <summary>
        /// Autorizační kód platby přidělený autorizačním centrem
        /// </summary>
        public string ACCODE { get; set; }
        /// <summary>
        /// Maskované číslo platební karty použité při platbě ve formátu 6+4 Např. 405607******0016
        /// </summary>
        public string PANPATTERN { get; set; }
        /// <summary>
        /// Datum kdy lze nejpozději provést stržení požadované částky. Formát: DDMMYYYY.
        /// </summary>
        public string DAYTOCAPTURE { get; set; }
        /// <summary>
        /// Kontrolní podpis řetězce, který vznikne zřetězením všech polí v uvedeném pořadí.
        /// </summary>
        public string DIGEST { get; set; }
        /// <summary>
        /// Kontrolní podpis řetězce, který vznikne zřetězením všech zaslaných polí v uvedeném pořadí (bez pole DIGEST) a navíc pole MERCHANTNUMBER (pole není zasíláno, obchodník jej musí znát, pole se přidá na konec řetězce). Tímto způsobem je zvýšena bezpečnost a jednoznačnost odpovědi. Ověření podpisu je identické jako u pole DIGEST.
        /// </summary>
        public string DIGEST1 { get; set; }

        public bool IsDigestValid { get; private set; }
        public bool IsDigest1Valid { get; private set; }
        public bool WasRequestValid { get; private set; }
        public bool WasPaymentValid { get; private set; }

        public GpwpCardPayResponse()
        {
        }

        public GpwpCardPayResponse(HttpRequest httpRequest, string publicKeyFile, string merchantNumber)
        {
            this.MERCHANTNUMBER = merchantNumber;

            this.OPERATION = GetRequestParam(httpRequest, "OPERATION");
            this.ORDERNUMBER = GetRequestParam(httpRequest, "ORDERNUMBER");
            this.MERORDERNUM = GetRequestParam(httpRequest, "MERORDERNUM");
            this.MD = GetRequestParam(httpRequest, "MD");
            this.PRCODE = GetRequestParam(httpRequest, "PRCODE");
            this.SRCODE = GetRequestParam(httpRequest, "SRCODE");
            this.RESULTTEXT = GetRequestParam(httpRequest, "RESULTTEXT");

            this.USERPARAM1 = GetRequestParam(httpRequest, "USERPARAM1");
            this.ADDINFO = GetRequestParam(httpRequest, "ADDINFO");
            this.TOKEN = GetRequestParam(httpRequest, "TOKEN");
            this.EXPIRY = GetRequestParam(httpRequest, "EXPIRY");
            this.ACSRES = GetRequestParam(httpRequest, "ACSRES");
            this.ACCODE = GetRequestParam(httpRequest, "ACCODE");
            this.PANPATTERN = GetRequestParam(httpRequest, "PANPATTERN");
            this.DAYTOCAPTURE = GetRequestParam(httpRequest, "DAYTOCAPTURE");

            this.DIGEST = GetRequestParam(httpRequest, "DIGEST");
            this.DIGEST1 = GetRequestParam(httpRequest, "DIGEST1");

            this.IsDigestValid = GpwpSignatureValidation.ValidateDigest(this.DIGEST, GetMessageForDigest(), publicKeyFile).IsValidSignature;
            this.IsDigest1Valid = GpwpSignatureValidation.ValidateDigest(this.DIGEST1, GetMessageForDigest1(), publicKeyFile).IsValidSignature;
            this.WasRequestValid = this.IsDigestValid && this.IsDigest1Valid;
            this.WasPaymentValid = this.WasRequestValid && this.PRCODE == "0" && this.SRCODE == "0";
        }

        string GetMessageForDigest()
        {
            StringBuilder str = new StringBuilder();
            str.Append(this.OPERATION);
            AddNextMessageField(str, this.ORDERNUMBER);
            AddNextMessageField(str, this.MERORDERNUM);
            AddNextMessageField(str, this.MD);
            AddNextMessageField(str, this.PRCODE);
            AddNextMessageField(str, this.SRCODE);

            AddNextMessageField(str, this.RESULTTEXT);
            AddNextMessageField(str, this.USERPARAM1);
            AddNextMessageField(str, this.ADDINFO);
            AddNextMessageField(str, this.TOKEN);
            AddNextMessageField(str, this.EXPIRY);
            AddNextMessageField(str, this.ACSRES);
            AddNextMessageField(str, this.ACCODE);
            AddNextMessageField(str, this.PANPATTERN);
            AddNextMessageField(str, this.DAYTOCAPTURE);

            return str.ToString();
        }
        string GetMessageForDigest1()
        {
            return string.Format("{0}|{1}", GetMessageForDigest(), this.MERCHANTNUMBER);
        }
        void AddNextMessageField(StringBuilder str, string fieldValue)
        {
            if (fieldValue != null)
            {
                str.Append(string.Format("|{0}", fieldValue));
            }
        }

        private string GetRequestParam(HttpRequest httpRequest, string paramName)
        {
            return httpRequest.Params.Get(paramName);
        }
    }
}
