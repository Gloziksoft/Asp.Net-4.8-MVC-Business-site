using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace dufeksoft.lib.Model.Grid
{
    public class GridSorterModel
    {
        public const string DefaultSortParam = "Sort";

        public HttpRequest CurrentRequest { get; private set; }
        public List<GridSortItem> Items { get; private set; }

        string querySortParamName;
        string urlWithoutQuery;
        string queryWithoutSortParam;
        string currentSortParam;

        Hashtable htQueryStrings = new Hashtable();

        public GridSorterModel(HttpRequest request, Dictionary<string, string> items, NameValueCollection newQueryParams = null, string sortParamName = GridSorterModel.DefaultSortParam)
        {
            this.CurrentRequest = request;
            this.querySortParamName = string.IsNullOrEmpty(sortParamName) ? GridSorterModel.DefaultSortParam : sortParamName;

            if (newQueryParams != null)
            {
                foreach (string param in newQueryParams.AllKeys)
                {
                    htQueryStrings.Add(param, param.ToLower());
                }
            }

            // Create url without query parameters
            this.urlWithoutQuery = this.CurrentRequest.Url.GetLeftPart(UriPartial.Path);
            // Create query string without page number parameter
            StringBuilder str = new StringBuilder();
            foreach (string param in this.CurrentRequest.QueryString.AllKeys)
            {
                if (CanAddOldParamToQuery(param))
                {
                    str.Append(string.Format("&{0}={1}", param, this.CurrentRequest.QueryString[param]));
                }
                else
                {
                    this.currentSortParam = this.CurrentRequest.QueryString[param].ToLower();
                }
            }
            if (newQueryParams != null)
            {
                foreach (string param in newQueryParams.AllKeys)
                {
                    str.Append(string.Format("&{0}={1}", param, newQueryParams[param]));
                }
            }
            this.queryWithoutSortParam = str.ToString();

            this.Items = new List<GridSortItem>();
            foreach (KeyValuePair<string, string> kv in items)
            {
                string sort = kv.Key.ToLower();
                string name = kv.Value;
                this.Items.Add(
                    new GridSortItem()
                    {
                        Sort = sort,
                        Name = name,
                        IsCurrent = sort == this.currentSortParam,
                        Url = GetSortUrl(sort)
                    });
            }
        }
        bool CanAddOldParamToQuery(string param)
        {
            if (param.ToLower() == this.querySortParamName)
            {
                return false;
            }

            return !htQueryStrings.ContainsKey(param.ToLower());
        }

        string GetSortUrl(string sort)
        {
            return string.Format("{0}?{1}={2}{3}", this.urlWithoutQuery, this.querySortParamName, sort, this.queryWithoutSortParam);
        }
    }

    public class GridSortItem
    {
        public string Name { get; set; }
        public string Sort { get; set; }
        public string Url { get; set; }
        public bool IsCurrent { get; set; }
    }
}
