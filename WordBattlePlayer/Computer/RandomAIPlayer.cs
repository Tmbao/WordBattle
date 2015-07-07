using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattleCore.GridEntities;
using Microsoft.Xna.Framework;
using WordBattleCore;

namespace WordBattlePlayer.Computer
{
    public class RandomAIPlayer : GameController
    {

        string pressedCharaceter;

        Random rand;

        public RandomAIPlayer()
        {
            rand = new Random();
        }

        public override string PressedCharacters()
        {
            return pressedCharaceter;
        }

        Tuple<int, int> selectedIndex;

        public override Tuple<int, int> SelectedIndex()
        {
            return selectedIndex;
        }

        public override void Update(GameTime gameTime)
        {
            var grid = WordGrid.GetInstance();
            List<Tuple<int, int>> freeCells = new List<Tuple<int, int>>();
            for (int row = 0; row < grid.NumberOfRows; row++)
                for (int col = 0; col < grid.NumberOfColumns; col++)
                    if (grid.CanFill(new Tuple<int,int>(row, col)))
                        freeCells.Add(new Tuple<int, int>(row, col));

            
            selectedIndex = freeCells[rand.Next(freeCells.Count)];
            pressedCharaceter = ((char)((int)'A' + rand.Next(26))).ToString();
        }
    }
}
