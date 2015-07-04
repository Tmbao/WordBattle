using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattle.Utilities
{
    class Utils
    {
        public static String GetCharacterFileName(char c)
        {
            c = Char.ToUpper(c);
            if (c == Consts.BLANK)
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
            else if ('A' <= c && c <= 'Z')
            {
                return String.Format(@"Characters\{0}", c);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static String GetMapFileName(string mapName)
        {
            return @"Map\" + mapName;
        }
    }
}
