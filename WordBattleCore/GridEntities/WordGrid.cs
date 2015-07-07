using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattleCore.Dictionary;

namespace WordBattleCore.GridEntities
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
            dictionary = TrieDictionary.GetInstance();
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

        int[] delta1 = { 0, 1, 0, -1 };
        int[] delta2 = { -1, 0, 1, 0 };

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

        bool firstMove;

        public bool FirstMove
        {
            get { return firstMove; }
            set { firstMove = value; }
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
            firstMove = true;
        }

        public bool CanFill(Tuple<int, int> selectedIndex)
        {
            // Not selected yet
            if (selectedIndex == null)
                return false;
            // The current position has been already filled
            else if (wordGrid.Grid[selectedIndex.Item1, selectedIndex.Item2] != Consts.BLANK)
                return false;
            // The first one can fill an arbitrary cell
            else if (firstMove)
                return true;
            else
            {
                // Check for adjacent 
                for (int k = 0; k < 4; k++)
                {
                    Tuple<int, int> adjacentIndex = new Tuple<int, int>(selectedIndex.Item1 + delta1[k], selectedIndex.Item2 + delta2[k]);
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

        TrieDictionary dictionary;
        Queue<Queue<Tuple<int, int>>> correctedWords;
        Stack<Tuple<int, int>> correctedWord;

        private void Travel(Tuple<int, int> index, TrieDictionary.TrieNode node)
        {
            if (node == null)
                return;

            correctedWord.Push(index);

            if (node.Word.Length > 0)
                correctedWords.Enqueue(new Queue<Tuple<int, int>>(correctedWord.ToList()));

            for (int k = 0; k < 4; k++)
            {
                Tuple<int, int> adjacentIndex = new Tuple<int, int>(index.Item1 + delta1[k], index.Item2 + delta2[k]);
                if (IsInside(adjacentIndex) && Char.IsLetter(grid[adjacentIndex.Item1, adjacentIndex.Item2]))
                    Travel(adjacentIndex, node.ChildAt(grid[adjacentIndex.Item1, adjacentIndex.Item2]));
            }

            correctedWord.Pop();
        }

        public Queue<Queue<Tuple<int, int>>> GetCorrectedWords(Tuple<int, int> index)
        {
            correctedWords = new Queue<Queue<Tuple<int, int>>>();
            correctedWord = new Stack<Tuple<int, int>>();

            Travel(index, dictionary.RootTree.ChildAt(grid[index.Item1, index.Item2]));

            return correctedWords;
        }
    }
}
