using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace dufeksoft.lib.Model.Grid
{
    public class GridPagerModel
    {
        public const string DefaultPageParam = "Page";
        public const string DefaultReplacePageNb = "{REPLACE_PAGE_NB}";

        public HttpRequest CurrentRequest { get; private set; }
        public int ItemCnt { get; private set; }
        public int PageSize { get; private set; }

        string queryPageParamName;
        string urlWithoutQuery;
        string queryWithoutPageParam;
        bool showPrevNextPager;

        Hashtable htQueryStrings = new Hashtable();

        public GridPagerModel(HttpRequest request, int itemCnt, int pageSize = 25, NameValueCollection newQueryParams = null, string queryPageParam = GridPagerModel.DefaultPageParam, bool showPrevNext = false)
        {
            this.CurrentRequest = request;
            this.ItemCnt = itemCnt;
            this.PageSize = pageSize;
            this.queryPageParamName = string.IsNullOrEmpty(queryPageParam) ? GridPagerModel.DefaultPageParam : queryPageParam;
            this.showPrevNextPager = showPrevNext;

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
            }
            if (newQueryParams != null)
            {
                foreach (string param in newQueryParams.AllKeys)
                {
                    str.Append(string.Format("&{0}={1}", param, newQueryParams[param]));
                }
            }
            this.queryWithoutPageParam = str.ToString();
        }
        bool CanAddOldParamToQuery(string param)
        {
            if (param.ToLower() == this.queryPageParamName.ToLower())
            {
                return false;
            }

            return !htQueryStrings.ContainsKey(param.ToLower());
        }

        public List<GridPageNumberModel> GetPagerList()
        {
            List<GridPageNumberModel> resultList = new List<GridPageNumberModel>();

            int totalPages = GetTotalPagesCount();
            if (totalPages < 2)
            {
                // No pager needed
                return resultList;
            }

            int currentPage = GetCurrentPageNumber();

            if (totalPages < 11)
            {
                if (showPrevNextPager)
                {
                    if (currentPage > 1)
                    {
                        // Previous page
                        resultList.Add(
                                new GridPageNumberModel()
                                {
                                    Url = GetPageUrl(currentPage - 1),
                                    Name = "<",
                                }
                            );
                    }
                }
                // Page numbers
                for (int i = 1; i < totalPages + 1; i++)
                {
                    resultList.Add(GetPageNumberModel(i, currentPage));
                }
                if (showPrevNextPager)
                {
                    if (currentPage < totalPages)
                    {
                        // Next page
                        resultList.Add(
                                new GridPageNumberModel()
                                {
                                    Url = GetPageUrl(currentPage + 1),
                                    Name = ">",
                                }
                            );
                    }
                }
                return resultList;
            }

            if (currentPage > 1)
            {
                // Previous page
                resultList.Add(
                        new GridPageNumberModel()
                        {
                            Url = GetPageUrl(currentPage - 1),
                            Name = "<",
                        }
                    );
            }
            // First page
            resultList.Add(GetPageNumberModel(1, currentPage));
            if (currentPage > 3)
            {
                // ...
                resultList.Add(
                        new GridPageNumberModel()
                        {
                            TemplateUrl = GetPageUrl(DefaultReplacePageNb),
                            Name = "...",
                        }
                    );
            }
            if (currentPage == 1)
            {
                resultList.Add(GetPageNumberModel(2, currentPage));
                resultList.Add(GetPageNumberModel(3, currentPage));
            }
            else
            {
                if (currentPage == totalPages)
                {
                    resultList.Add(GetPageNumberModel(totalPages - 2, currentPage));
                    resultList.Add(GetPageNumberModel(totalPages - 1, currentPage));
                }
                else
                {
                    // Current page - 1
                    if (currentPage > 2)
                    {
                        resultList.Add(GetPageNumberModel(currentPage - 1, currentPage));
                    }
                    // Current page
                    resultList.Add(GetPageNumberModel(currentPage, currentPage));
                    // Current page + 1
                    if (currentPage < totalPages - 1)
                    {
                        resultList.Add(GetPageNumberModel(currentPage + 1, currentPage));
                    }
                }
            }


            if (currentPage < totalPages - 2)
            {
                // ...
                resultList.Add(
                        new GridPageNumberModel()
                        {
                            TemplateUrl = GetPageUrl(DefaultReplacePageNb),
                            Name = "...",
                        }
                    );
            }
            // Last page
            resultList.Add(GetPageNumberModel(totalPages, currentPage));
            if (currentPage < totalPages)
            {
                // Next page
                resultList.Add(
                        new GridPageNumberModel()
                        {
                            Url = GetPageUrl(currentPage + 1),
                            Name = ">",
                        }
                    );
            }


            return resultList;
        }

        GridPageNumberModel GetPageNumberModel(int pageNumber, int currentPageNumber)
        {
            return new GridPageNumberModel()
            {
                Url = GetPageUrl(pageNumber),
                Name = pageNumber.ToString(),
                IsCurrent = pageNumber == currentPageNumber
            };
        }

        string GetPageUrl(int pageNumber)
        {
            return GetPageUrl(pageNumber.ToString());
        }
        string GetPageUrl(string pageNumber)
        {
            return string.Format("{0}?{1}={2}{3}", this.urlWithoutQuery, this.queryPageParamName, pageNumber, this.queryWithoutPageParam);
        }

        public int GetCurrentPageNumber()
        {
            int pageNumber = GetRequestPageNumber(this.CurrentRequest, this.queryPageParamName);
            int totalPagesCount = GetTotalPagesCount();
            if (pageNumber > totalPagesCount)
            {
                pageNumber = totalPagesCount;
            }
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            return pageNumber;
        }

        public static int GetRequestPageNumber(HttpRequest request, string queryPageParam = GridPagerModel.DefaultPageParam)
        {
            string pageNumberParam = request.Params[queryPageParam];
            int pageNumber;
            if (!int.TryParse(pageNumberParam, out pageNumber))
            {
                pageNumber = 1;
            }

            return pageNumber;
        }

        public int GetTotalPagesCount()
        {
            int totalPages = this.ItemCnt / this.PageSize;
            if (totalPages * this.PageSize < this.ItemCnt)
            {
                totalPages++;
            }

            return totalPages;
        }

        public GridPageInfo GetCurrentPageInfo()
        {
            int currentPage = this.GetCurrentPageNumber();
            int startIndex = this.PageSize * (currentPage - 1);
            int lastIndex = this.PageSize * currentPage;

            return new GridPageInfo()
            {
                PageNumber = currentPage,
                FirsItemIndex = startIndex,
                LastItemIndex = lastIndex - 1
            };
        }
    }

    public class GridPageNumberModel
    {
        public string TemplateUrl { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class GridPageInfo
    {
        public int PageNumber { get; set; }
        public int FirsItemIndex { get; set; }
        public int LastItemIndex { get; set; }
    }
}
