using System;

namespace WordBattle
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static WordBattleGame game;

        static void Main(string[] args)
        {
            game = new WordBattleGame();
            game.Run();
        }
    }
#endif
}

