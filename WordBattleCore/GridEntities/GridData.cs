using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattleCore.GridEntities
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

        int nObstacles;

        public int NumberOfObstacles
        {
            get { return nObstacles; }
            set { nObstacles = value; }
        }

        int[] obstacleCol, obstacleRow;

        public int[] ObstacleRow
        {
            get { return obstacleRow; }
            set { obstacleRow = value; }
        }

        public int[] ObstacleColumn
        {
            get { return obstacleCol; }
            set { obstacleCol = value; }
        }

        public Tuple<int, int> ObstacleAt(int index)
        {
            return new Tuple<int, int>(obstacleRow[index], obstacleCol[index]);
        }
    }
}
