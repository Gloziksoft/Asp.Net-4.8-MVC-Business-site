using System.Collections.Generic;
using System.IO;
using System.Web;

namespace dufeksoft.lib.Files
{
    public class FileList
    {

        public static string RootPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath);
            }
        }
        public static List<string> GetFilesInDirectory(string pathToSearch)
        {
            string searchPath = string.Format("{0}\\{1}", FileList.RootPath.TrimEnd('\\'), pathToSearch.TrimStart('\\'));
            int fileNamePos = searchPath.Length + 1;

            List<string> lstFilesFound = new List<string>();
            if (Directory.Exists(searchPath))
            {
                foreach (string f in Directory.GetFiles(searchPath))
                {
                    lstFilesFound.Add(f.Substring(fileNamePos));
                }
            }

            return lstFilesFound;
        }
    }
}
