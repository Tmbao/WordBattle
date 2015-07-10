using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WordBattleCore.GridEntities;

namespace WordBattlePlayer.Computer
{
    public class RandomVowelAIPlayer : GameController
    {
        string pressedCharaceter;

        Random rand;

        public RandomVowelAIPlayer()
        {
            rand = new Random();
        }

        public override string PressedCharacter()
        {
            return pressedCharaceter;
        }

        Tuple<int, int> selectedIndex;

        public override Tuple<int, int> SelectedIndex()
        {
            return selectedIndex;
        }

        public static bool IsVowel(char character)
        {
            return "AEIOU".IndexOf(character) >= 0;
        }

        int[] delta1 = { 0, -1, 0, 1 };
        int[] delta2 = { -1, 0, 1, 0 };

        public override void Update(GameTime gameTime)
        {
            var grid = WordGrid.GetInstance();
            List<Tuple<int, int>> vowelAdjCells = new List<Tuple<int, int>>();
            List<Tuple<int, int>> consAdjCells = new List<Tuple<int, int>>();
            for (int row = 0; row < grid.NumberOfRows; row++)
                for (int col = 0; col < grid.NumberOfColumns; col++)
                    if (grid.CanFill(new Tuple<int, int>(row, col)))
                    {
                        bool check = false;
                        for (int k = 0; k < 4; k++)
                        {
                            var adj = new Tuple<int, int>(row + delta1[k], col + delta2[k]);
                            if (grid.IsInside(adj) && IsVowel(grid.Grid[adj.Item1, adj.Item2]))
                                check = true;
                        }

                        if (check)
                            vowelAdjCells.Add(new Tuple<int, int>(row, col));
                        else
                            consAdjCells.Add(new Tuple<int, int>(row, col));
                    }

            if (vowelAdjCells.Count > 0)
            {
                selectedIndex = vowelAdjCells[rand.Next(vowelAdjCells.Count)];
                pressedCharaceter = ((char)((int)'A' + rand.Next(26))).ToString();
            }
            else
            {
                selectedIndex = consAdjCells[rand.Next(consAdjCells.Count)];
                pressedCharaceter = ((char)((int)'A' + rand.Next(26))).ToString();
            }
        }
    }
}
