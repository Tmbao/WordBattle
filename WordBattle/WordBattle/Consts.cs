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

        public static readonly string SERVER_IP = "";
        public static readonly int PORT = 4510;
        public static readonly int MAX_CONNECTION_ATTEMPTS = 5;

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
        public static readonly float LOGO_TRANSLATION_TIME = 75;

        public static readonly char NONE = WordBattleCore.Consts.NONE;
        public static readonly char SPACE = WordBattleCore.Consts.SPACE;
        public static readonly char BLANK = WordBattleCore.Consts.BLANK;
        public static readonly char OBSTACLE = WordBattleCore.Consts.OBSTACLE;
        public static readonly char LIGHT = WordBattleCore.Consts.LIGHT;
        public static readonly char LIGHT_BLUE = WordBattleCore.Consts.LIGHT_BLUE;


        public static readonly float INTENSITY_LOADING_MAX = 1f;
        public static readonly float INTENSITY_LOADING_DELTA = 0.75f;
        public static readonly float INTENSITY_HOVER_MAX = 0.75f;
        public static readonly float INTENSITY_HOVER_DELTA = 0.075f;
        public static readonly float INTENSITY_SELECTED = 1f;
        public static readonly float INTENSITY_ACHIEVED_MAX = 1f;
        public static readonly float INTENSITY_ACHIEVED_DELTA = 0.02f;
        public static readonly float INTENSITY_NOTIFICATION = 1f;
        public static readonly float INTENSITY_NOTIFICATION_DELTA = 0.02f;
        public static readonly float INTENSITY_PLAYER_TURN = 0.75f;
        public static readonly float INTENSITY_MENU_MAX = 1f;
        public static readonly float INTENSITY_MENU_DELTA = 0.02f;

        public static readonly int DRAWING_EFFECT_TIME = 40;
        public static readonly int SCORE_EFFECT_TIME = 10;

        public static readonly int PLAYER_PANEL_WIDTH = 375;
        public static readonly int PLAYER_PANEL_HEIGHT = 100;
        public static readonly int PLAYER_IMAGE_WIDTH = 90;
        public static readonly int PLAYER_IMAGE_HEIGHT = 90;
        public static readonly int COMPONENT_SPACING = 5;
        public static readonly int PLAYER_FONT_SIZE = 36;
        public static readonly int TEXT_SPACING = 1;

        public static readonly int PLAYER1_PANEL_LEFT = 25;
        public static readonly int PLAYER1_PANEL_TOP = 250;

        public static readonly int PLAYER2_PANEL_LEFT = 25;
        public static readonly int PLAYER2_PANEL_TOP = 375;

        public static readonly int NOTIFICATION_LEFT = 450;
        public static readonly int NOTIFICATION_TOP = 100;
        public static readonly int NOTIFICATION_WIDTH = 512;
        public static readonly int NOTIFICATION_TEXT_SIZE = 36;

        public static readonly int MAX_NAME_LENGTH = WordBattleCore.Consts.MAX_NAME_LENGTH;
        public static readonly int MAX_SCORE_DIGIT = 6;

        public static readonly int TEXTBOX_TITLE_LENGTH = 7;
        public static readonly int TEXTBOX_TEXT_WIDTH = 40;
        public static readonly int TEXTBOX_TEXT_HEIGHT = 40;

        public static readonly int MENU_FONT_SIZE = 40;
        public static readonly int MENU_LEFT = 75;
        public static readonly int MENU_TOP = 260;

        public static readonly int BUTTON_WIDTH = 40;
        public static readonly int BUTTON_HEIGHT = 40;
    }
}
