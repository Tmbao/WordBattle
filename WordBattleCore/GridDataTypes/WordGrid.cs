using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattleCore.GridDataTypes
{
    public class WordGrid
    {
        private static WordGrid wordGrid;

        public static WordGrid GetInstance()
        {
            if (wordGrid == null)
                wordGrid = new WordGrid();
            return wordGrid;
        }

        private WordGrid()
        {
        }

        int gridCols, gridRows;

        public int NumberOfRows
        {
            get { return gridRows; }
            private set { gridRows = value; }
        }

        public int NumberOfColumns
        {
            get { return gridCols; }
            private set { gridCols = value; }
        }

        char[,] grid;

        public char[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        public void Load(GridData gridData)
        {
            gridCols = gridData.NumberOfColumns;
            gridRows = gridData.NumberOfRows;

            grid = new char[gridRows, gridCols];
            for (int row = 0; row < gridRows; row++)
                for (int col = 0; col < gridCols; col++)
                    grid[row, col] = Consts.BLANK;

            for (int index = 0; index < gridData.Obstacle.Count; index++)
                grid[gridData.Obstacle[index].Item1, gridData.Obstacle[index].Item2] = Consts.OBSTACLE;
        }
    }
}
