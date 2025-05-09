using System.Collections;
using System.Collections.Generic;

namespace dufeksoft.lib.Model.Grid
{
    public class GridRowsModel
    {
        public List<List<object>> Rows { get; set; }

        public GridRowsModel(IEnumerable itemList, int columns)
        {
            this.Rows = new List<List<object>>();

            int colCnt = 0;
            List<object> row = null;
            foreach (object item in itemList)
            {
                if (colCnt % columns == 0)
                {
                    row = new List<object>();
                    this.Rows.Add(row);
                }
                row.Add(item);

                colCnt++;
            }
        }
    }
}
