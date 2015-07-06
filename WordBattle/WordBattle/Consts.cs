using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordBattle
{
    public static class Consts
    {
        public static readonly string ContentDirectory = "Content";

        public static readonly string DEFALT_MAP_NAME = "Default";

        public static readonly string DEFAULT_DICTIONARY_NAME = "English";

        public static readonly int SCREEN_WIDTH = 1000;
        public static readonly int SCREEN_HEIGHT = 700;

        public static readonly int TILE_WIDTH = 64;
        public static readonly int TILE_HEIGHT = 64;

        public static readonly int GRID_COLS = 8;
        public static readonly int GRID_ROWS = 8;

        public static readonly float GRID_LEFT = 450;
        public static readonly float GRID_TOP = 150;

        public static readonly float LOGO_LEFT = 25;
        public static readonly float LOGO_TOP = 25;

        public static readonly int LOGO_WIDTH = 275;
        public static readonly int LOGO_HEIGHT = 200;

        public static readonly char NONE = WordBattleCore.Consts.NONE;
        public static readonly char SPACE = WordBattleCore.Consts.SPACE;
        public static readonly char BLANK = WordBattleCore.Consts.BLANK;
        public static readonly char OBSTACLE = WordBattleCore.Consts.OBSTACLE;
        public static readonly char LIGHT = WordBattleCore.Consts.LIGHT;
        public static readonly char LIGHT_BLUE = WordBattleCore.Consts.LIGHT_BLUE;

        public static readonly float INTENSITY_MAX = 0.75f;
        public static readonly float INTENSITY_DELTA = 0.075f;
        public static readonly float INTENSITY_SELECTED = 1f;
        public static readonly float INTENSITY_ACHIEVED = 60f;
        public static readonly float INTENSITY_ACHIEVED_DELTA = 0.06f;
        public static readonly float INTENSITY_PLAYER_TURN = 0.75f;

        public static readonly int DRAWING_EFFECT_TIME = 40;
        public static readonly int SCORE_EFFECT_TIME = 10;

        public static readonly int PLAYER_PANEL_WIDTH = 375;
        public static readonly int PLAYER_PANEL_HEIGHT = 100;
        public static readonly int PANEL_COMPONENT_SPACING = 5;
        public static readonly int PLAYER_FONT_SIZE = 36;
        public static readonly int PLAYER_TEXT_SPACING = 1;

        public static readonly int PLAYER1_PANEL_LEFT = 40;
        public static readonly int PLAYER1_PANEL_TOP = 300;

        public static readonly int PLAYER2_PANEL_LEFT = 40;
        public static readonly int PLAYER2_PANEL_TOP = 400;
    }
}
