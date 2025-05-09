using System.Web;

namespace dufeksoft.lib.Mail
{
    /// <summary>
    /// Mail attachement class
    /// </summary>
    public class MailAttachement
    {
        /// <summary>
        /// Default mail attachement path
        /// </summary>
        public static string DefaultPath = "\\App_Data\\MailAttachement";

        /// <summary>
        /// Gets the attachement file full path name
        /// </summary>
        /// <param name="filePath">Attachement file directory</param>
        /// <param name="fileName">Attachement file name</param>
        /// <returns>Returns file full path name</returns>
        public static string GetAttachementPath(string filePath, string fileName)
        {
            string fileFullName = string.Format("{0}\\{1}",
                string.IsNullOrEmpty(filePath) ? HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath) + MailAttachement.DefaultPath : filePath,
                fileName);


            return fileFullName;
        }
    }
}
