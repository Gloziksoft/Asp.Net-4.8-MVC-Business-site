namespace dufeksoft.lib.Model.GpWebpay
{
    public class GpwpCardPayRequest
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
        /// Číslo platby. Číslo musí být v každém požadavku od obchodníka unikátní.
        /// </summary>
        public string ORDERNUMBER { get; set; }
        /// <summary>
        /// Částka v nejmenších jednotkách dané měny pro Kč = v haléřích, pro EUR = v centech
        /// </summary>
        public string AMOUNT { get; set; }
        /// <summary>
        /// Identifikátor měny dle ISO 4217. Multicurrency (použití různých měn) je závislé na podpoře jednotlivých bank. Je nutné se informovat u své banky.
        /// </summary>
        public string CURRENCY { get; set; }
        /// <summary>
        /// Udává, zda má být platba uhrazena automaticky. Povolené hodnoty: 0 = není požadována okamžitá úhrada 1 = je požadována úhrada
        /// </summary>
        public string DEPOSITFLAG { get; set; }
        /// <summary>
        /// Číslo platby. V případě, že není zadáno, použije se hodnota ORDERNUMBER Zobrazí se na výpisu z banky. Každá banka má své řešení/limit.
        /// </summary>
        public string MERORDERNUM { get; set; }
        /// <summary>
        /// Plná URL adresa obchodníka. Na tuto adresu bude odeslán výsledek požadavku. Výsledek je přeposlán přes prohlížeč zákazníka. Je použit redirect (metoda GET), nebo formulář (metoda POST). (včetně specifikace protokolu – např. https://) Z bezpečnostních důvodů může dojít k zamezení některých tvarů URL adresy – např. použití parametrů v adrese. Tuto kontrolu nelze vypnout a je nutné odzkoušet reálný tvar
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Popis nákupu. Obsah pole se přenáší do 3D Secure systému pro možnost následné kontroly držitelem karty během autentikace Access Control Serveru vydavatelské banky. Pole musí obsahovat pouze ASCII znaky v rozsahu 0x20 – 0x7E.
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// Libovolná data obchodníka, která jsou vrácena obchodníkovi v odpovědi v nezměněné podobě – pouze očištěna
        /// </summary>
        public string MD { get; set; }
        /// <summary>
        /// Hodnota určující preferovanou platební metodu. Podporované hodnoty: CRD – platební karta MCM – MasterCard Mobile MPS – MasterPass GPAY – GooglePay BTNCS – Platba 24 BTN360CS – Platba z účtu BTN360CS-0100 – Platba z účtu – Komerční banka, a.s. BTN360CS-0300 – Platba z účtu – Československá obchodní banka, a. s. BTN360CS-0600 – Platba z účtu – MONETA Money Bank, a. s. BTN360CS-0710 – Platba z účtu – Česká národní banka BTN360CS-0800 – Platba z účtu – Česká spořitelna, a.s. BTN360CS-2010 – Platba z účtu – Fio banka, a.s. BTN360CS-2700 – Platba z účtu – UniCredit Bank Czech Republic and Slovakia, a.s. BTN360CS-3030 – Platba z účtu – Air Bank a.s. BTN360CS-5500 – Platba z účtu – Raiffeisenbank a.s. BTN360CS-6210 – Platba z účtu – mBank S.A. BTN360CS-6800 – Platba z účtu – Sberbank CZ, a.s.
        /// </summary>
        public string PAYMETHOD { get; set; }
        /// <summary>
        /// Seznam povolených platebních metod. Hodnoty jsou odděleny čárkou „,“. Pokud je současně definováno pole DISABLEPAYMETHOD, vytvoří se nejprve průnik hodnot a porovná se s polem PAYMETHOD. V případě rozdílnosti hodnot je vrácena chyba o nevhodné hodnotě v odpovídajícím poli.
        /// </summary>
        public string PAYMETHODS { get; set; }
        /// <summary>
        /// E-mail držitele karty, použije se pro notifikaci výsledku platby a v antifraud systémech (FDS). Pole musí obsahovat pouze jednu validní e-mail adresu. Pole může obsahovat jakékoli znaky, ale pokud se v e-mail adrese vyskytují národní znaky, doporučujeme použít BASE64 kódování.
        /// </summary>
        public string EMAIL { get; set; }
        /// <summary>
        /// Interní ID u obchodníka Podporované ASCII znaky: x20(space), x23(#), x24($), x2A-x3B(*+,-./0-9:;), x3D(=), x40-x5A(@A-Z), x5E(^), x5F(_), x61-x7A(a-z)
        /// </summary>
        public string REFERENCENUMBER { get; set; }
        /// <summary>
        /// Kontrolní podpis řetězce, který vznikne zřetězením zaslaných polí v pořadí, uvedeném v této tabulce. V případě chybného podpisu dat se chybové hlášení zasílá zpět do internetového prohlížeče, ze kterého tento požadavek přišel.
        /// </summary>
        public string DIGEST { get; set; }
        /// <summary>
        /// Hodnota určuje automatickou volbu jazyka na platební stránce. Musí být použita zkratka jednoho z podporovaných jazyků – viz seznam na platební bráně.
        /// </summary>
        public string LANG { get; set; }


        public bool IsTest { get; private set; }
        public string GpWebpayUrl { get; private set; }

        public GpwpCardPayRequest(bool isTest = true)
        {
            this.IsTest = isTest;
            this.GpWebpayUrl = this.IsTest ? "https://test.3dsecure.gpwebpay.com/pgw/order.do" : "https://3dsecure.gpwebpay.com/pgw/order.do";

            this.OPERATION = "CREATE_ORDER";
            this.DEPOSITFLAG = "1";
            this.PAYMETHOD = "CRD";
            this.PAYMETHODS = "CRD";
        }

        public GpwpSignature CreateSignature(string privateCertificateFile, string password)
        {
            string message = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}",
                MERCHANTNUMBER,
                OPERATION,
                ORDERNUMBER,
                AMOUNT,
                CURRENCY,
                DEPOSITFLAG,
                MERORDERNUM,
                URL,
                DESCRIPTION,
                MD,
                PAYMETHOD,
                PAYMETHODS,
                EMAIL,
                REFERENCENUMBER);
            //string message = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
            //    MERCHANTNUMBER,
            //    OPERATION,
            //    ORDERNUMBER,
            //    AMOUNT,
            //    CURRENCY,
            //    DEPOSITFLAG,
            //    MERORDERNUM,
            //    URL,
            //    DESCRIPTION,
            //    MD);

            //string pipe = Convert.ToChar(Convert.ToUInt32("007c", 16)).ToString();
            //string message = 
            //    MERCHANTNUMBER + pipe + 
            //    OPERATION + pipe + 
            //    ORDERNUMBER + pipe + 
            //    AMOUNT + pipe + 
            //    CURRENCY + pipe + 
            //    DEPOSITFLAG + pipe + 
            //    URL;

            return GpwpSignature.SignData(message, privateCertificateFile, password);
        }
    }
}
