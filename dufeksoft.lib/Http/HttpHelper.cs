using System.Web;

namespace dufeksoft.lib.Http
{
    public abstract class HttpHelper
    {
        public static string GetCurrentRequestUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
    }
}
