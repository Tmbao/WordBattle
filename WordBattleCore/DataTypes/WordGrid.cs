using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattleCore.DataTypes
{
    public class WordGrid
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

        public char[,] Grid()
        {
            char[,] grid = new char[gridRows, gridCols];
            for (int row = 0; row < gridRows; row++)
                for (int col = 0; col < gridCols; col++)
                    grid[row, col] = Consts.BLANK;

            for (int index = 0; index < obstacle.Count; index++)
                grid[obstacle[index].Item1, obstacle[index].Item2] = Consts.OBSTACLE;

            return grid;
        }
    }
}
