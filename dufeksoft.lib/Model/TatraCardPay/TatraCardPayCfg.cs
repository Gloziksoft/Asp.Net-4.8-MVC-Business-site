using System.Configuration;

namespace dufeksoft.lib.Model.TatraCardPay
{
    public abstract class TatraCardPayCfg
    {
        public const string RequestModelKey = "tatrabanka.CardPayRequestModel";

        public const string cfgUseTestData = "tatrabanka.CardPay.UseTestData";
        public const string cfgMID = "tatrabanka.CardPay.MID";
        public const string cfgKey = "tatrabanka.CardPay.Key";
        public const string cfgEmail = "tatrabanka.CardPay.Email";

        public static bool UseTestData()
        {
            return ConfigurationManager.AppSettings[TatraCardPayCfg.cfgUseTestData] == "1";
        }

        public static string GetMID()
        {
            if (UseTestData())
            {
                return ConfigurationManager.AppSettings[TatraCardPayCfg.cfgMID + "Test"];
            }
            else
            {
                return ConfigurationManager.AppSettings[TatraCardPayCfg.cfgMID];
            }
        }

        public static string GetKey()
        {
            if (UseTestData())
            {
                return ConfigurationManager.AppSettings[TatraCardPayCfg.cfgKey + "Test"];
            }
            else
            {
                return ConfigurationManager.AppSettings[TatraCardPayCfg.cfgKey];
            }
        }

        public static string GetEmail()
        {
            return ConfigurationManager.AppSettings[TatraCardPayCfg.cfgEmail];
        }
    }
}
