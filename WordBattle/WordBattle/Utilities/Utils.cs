using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WordBattleCore.GridEntities;

namespace WordBattle.Utilities
{
    static class Utils
    {
        public static string GetCharacterFileName(char c)
        {
            c = Char.ToUpper(c);
            if (c == Consts.SPACE)
            {
                return @"Characters\Space";
            }
            else if (c == Consts.BLANK)
            {
                return @"Characters\Blank";
            }
            else if (c == Consts.OBSTACLE)
            {
                return @"Characters\Obstacle";
            }
            else if (c == Consts.LIGHT)
            {
                return @"Characters\Light";
            }
            else if (c == Consts.LIGHT_BLUE)
            {
                return @"Characters\LightBlue";
            }
            else if (Utils.IsLetter(c) || Char.IsDigit(c))
            {
                return String.Format(@"Characters\{0}", c);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string GetMapFileName(string mapName)
        {
            return @"Maps\" + mapName;
        }

        public static string GetFontFileName(string fontName)
        {
            return @"Fonts\" + fontName;
        }

        public static string GetImageFileName(string fileName)
        {
            return @"Images\" + fileName;
        }

        public static string GetDictionaryFileName(string fileName)
        {
            return @"Dictionaries\" + fileName;
        }

        public static List<Texture2D> LoadTextures(string path)
        {
            var result = new List<Texture2D>();
            result.Add(Global.Content.Load<Texture2D>(path));
            return result;
        }

        public static int GetTextWidth(string text, int fontSize, int textSpacing)
        {
            return text.Length * fontSize + (text.Length - 1) * textSpacing;
        }

        public static Tuple<int, int> GetIndexOfMouse(Vector2 currentPosition)
        {
            // Outside of grid
            if (currentPosition.X < Consts.GRID_LEFT || currentPosition.Y < Consts.GRID_TOP)
                return null;
            else
            {
                int col = (int)(currentPosition.X - Consts.GRID_LEFT) / Consts.TILE_WIDTH;
                int row = (int)(currentPosition.Y - Consts.GRID_TOP) / Consts.TILE_HEIGHT;

                // Outside of grid
                if (col >= WordGrid.GetInstance().NumberOfColumns || row >= WordGrid.GetInstance().NumberOfRows)
                    return null;
                else
                    return new Tuple<int, int>(row, col);
            }
        }

        public static bool IsLetter(char c)
        {
            c = Char.ToUpper(c);
            return 'A' <= c && c <= 'Z';
        }

        public static bool IsLetter(string s)
        {
            return s.Length == 1 && IsLetter(s[0]);
        }

        public static bool IsDigit(char c)
        {
            return Char.IsDigit(c);
        }

        public static bool IsDigit(string s)
        {
            return IsDigit(s[s.Length - 1]);
        }

        public static int GetTextboxWidth()
        {
            return Consts.TEXTBOX_TEXT_WIDTH * Consts.TEXTBOX_TITLE_LENGTH + (Consts.TEXTBOX_TITLE_LENGTH - 1) * Consts.TEXT_SPACING + 4 * Consts.COMPONENT_SPACING +
                Consts.MAX_NAME_LENGTH * Consts.TEXTBOX_TEXT_WIDTH + (Consts.MAX_NAME_LENGTH - 1) * Consts.TEXT_SPACING;
        }

        public static float GetMenuOptionWidth(string text)
        {
            return 2 * (Consts.MENU_FONT_SIZE + Consts.COMPONENT_SPACING) + text.Length * Consts.MENU_FONT_SIZE + (text.Length - 1) * Consts.TEXT_SPACING;
        }

        internal static string GetImageFileName(char c)
        {
            c = Char.ToUpper(c);
            if (c == Consts.SPACE)
            {
                return @"Images\Space";
            }
            else if (c == Consts.BLANK)
            {
                return @"Images\Blank";
            }
            else if (c == Consts.OBSTACLE)
            {
                return @"Images\Obstacle";
            }
            else if (c == Consts.LIGHT)
            {
                return @"Images\Light";
            }
            else if (c == Consts.LIGHT_BLUE)
            {
                return @"Images\LightBlue";
            }
            else if (Utils.IsLetter(c) || Char.IsDigit(c))
            {
                return String.Format(@"Images\{0}", c);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
