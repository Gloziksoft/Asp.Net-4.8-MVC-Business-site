using System.Collections.Generic;
using System.IO;
using System.Web;

namespace dufeksoft.lib.Mail
{
    /// <summary>
    /// Template text manipulator
    /// </summary>
    public class TextTemplate
    {
        public static string DefaultPath = "\\App_Data\\MailTemplates";
        public static string DefaultExtension = "html";

        /// <summary>
        /// Gets the template text
        /// </summary>
        /// <param name="templateName">Template file name</param>
        /// <returns>Returns template text</returns>
        public static string GetTemplateText(string templateName)
        {
            return GetTemplateText(null, templateName, null, null);
        }
        /// <summary>
        /// Gets the template text
        /// </summary>
        /// <param name="templateName">Template file name</param>
        /// <param name="paramList">Template parameters</param>
        /// <returns>Returns template text</returns>
        public static string GetTemplateText(string templateName, List<TextTemplateParam> paramList)
        {
            return GetTemplateText(null, templateName, null, paramList);
        }

        /// <summary>
        /// Gets the template text
        /// </summary>
        /// <param name="templatePath">Template file directory</param>
        /// <param name="templateName">Template file name</param>
        /// <param name="templateExtension">Template file extension</param>
        /// <param name="paramList">Template parameters</param>
        /// <returns>Returns template text</returns>
        public static string GetTemplateText(string templatePath, string templateName, string templateExtension, List<TextTemplateParam> paramList)
        {
            string templateText = string.Empty;
            string templateFullName = string.Format("{0}\\{1}.{2}",
                string.IsNullOrEmpty(templatePath) ? HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath) + TextTemplate.DefaultPath : templatePath,
                templateName,
                string.IsNullOrEmpty(templateExtension) ? TextTemplate.DefaultExtension : templateExtension);


            // Read template text
            using (TextReader tr = new StreamReader(templateFullName))
            {
                templateText = tr.ReadToEnd();
                tr.Close();
            }
            // Replace parameters
            if (paramList != null)
            {
                foreach (TextTemplateParam param in paramList)
                {
                    templateText = templateText.Replace("{" + param.ParamName + "}", param.ParamValue);
                }
            }

            return templateText;
        }
    }
}
