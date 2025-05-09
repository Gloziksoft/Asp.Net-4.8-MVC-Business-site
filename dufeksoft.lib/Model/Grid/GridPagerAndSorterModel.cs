using System.Collections.Generic;

namespace dufeksoft.lib.Model.Grid
{
    public class GridPagerAndSorterModel
    {
        public int TotalItems { get; set; }
        public List<GridPageNumberModel> PagerItems { get; set; }
        public GridSorterModel Sorter { get; set; }

        public GridPagerAndSorterModel(int totalItems, List<GridPageNumberModel> pagerItems, GridSorterModel sorter)
        {
            this.TotalItems = totalItems;
            this.PagerItems = pagerItems;
            this.Sorter = sorter;
        }
    }
}
