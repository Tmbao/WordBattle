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

        char[,] _grid;
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

            _grid = new char[gridRows, gridCols];
            for (int row = 0; row < gridRows; row++)
                for (int col = 0; col < gridCols; col++)
                    _grid[row, col] = Consts.BLANK;

            for (int index = 0; index < gridData.Obstacle.Count; index++)
                _grid[gridData.Obstacle[index].Item1, gridData.Obstacle[index].Item2] = Consts.OBSTACLE;
        }

        public void IntializeNewMap()
        {
            List<Tuple<int, int>> freeCells = new List<Tuple<int, int>>();

            // Make a deep copy
            grid = new char[gridRows, gridCols];
            for (int row = 0; row < gridRows; row++)
                for (int col = 0; col < gridCols; col++)
                {
                    grid[row, col] = _grid[row, col];
                    if (grid[row, col] == Consts.BLANK)
                        freeCells.Add(new Tuple<int, int>(row, col));
                }

            // Fill one of the freecells with an arbitrary letter
            int index = new Random().Next(freeCells.Count);
            char randChar = (char) (new Random().Next(26) + (int) 'A');
            grid[freeCells[index].Item1, freeCells[index].Item2] = randChar;
        }

        public bool CanFill(Tuple<int, int> selectedIndex)
        {
            // Not selected yet
            if (selectedIndex == null)
                return false;
            // The current position has been already filled
            else if (wordGrid.Grid[selectedIndex.Item1, selectedIndex.Item2] != Consts.BLANK)
                return false;
            else
            {
                // Check for adjacent 
                int[] d1 = { 0, 1, 0, -1 };
                int[] d2 = { -1, 0, 1, 0 };
                for (int k = 0; k < 4; k++)
                {
                    Tuple<int, int> adjacentIndex = new Tuple<int, int>(selectedIndex.Item1 + d1[k], selectedIndex.Item2 + d2[k]);
                    if (IsInside(adjacentIndex) && Char.IsLetter(grid[adjacentIndex.Item1, adjacentIndex.Item2]))
                        return true;
                }
                return false;
            }
        }

        private bool IsInside(Tuple<int, int> index)
        {
            return 0 <= index.Item1 && index.Item1 < gridRows && 0 <= index.Item2 && index.Item2 < gridCols;
        }
    }
}
