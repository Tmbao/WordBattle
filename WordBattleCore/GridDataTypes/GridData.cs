using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattleCore.GridDataTypes
{
    public class GridData
    {
        int gridCols, gridRows;

        public int NumberOfRows
        {
            get { return gridRows; }
            set { gridRows = value; }
        }

        public int NumberOfColumns
        {
            get { return gridCols; }
            set { gridCols = value; }
        }

        List<Tuple<int, int>> obstacle;

        public List<Tuple<int, int>> Obstacle
        {
            get { return obstacle; }
            set { obstacle = value; }
        }
    }
}
