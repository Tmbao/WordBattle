using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattle
{
    public class Consts
    {
        public static readonly string ContentDirectory = "Content";

        public static readonly int SCREEN_WIDTH = 800;
        public static readonly int SCREEN_HEIGHT = 600;

        public static readonly int TILE_WIDTH = 64;
        public static readonly int TILE_HEIGHT = 64;

        public static readonly int GRID_COLS = 8;
        public static readonly int GRID_ROWS = 8;

        public static readonly float GRID_LEFT = 100;
        public static readonly float GRID_TOP = 10;

        public static readonly char BLANK = WordBattleCore.Consts.BLANK;
        public static readonly char OBSTACLE = WordBattleCore.Consts.OBSTACLE;
        public static readonly char LIGHT = WordBattleCore.Consts.LIGHT;

        public static readonly float INTENSITY_MAX = 0.5f;
        public static readonly float INTENSITY_DELTA = 0.025f;
        
    }
}
