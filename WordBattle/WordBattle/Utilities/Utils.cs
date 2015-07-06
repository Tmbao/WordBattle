using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WordBattleCore.GridDataTypes;

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
            else if (c == Consts.LIGHT_YELLOW)
            {
                return @"Characters\LightYellow";
            }
            else if (Char.IsLetter(c) || Char.IsDigit(c))
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

        public static List<Texture2D> LoadSprite(string path)
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
    }
}
