using System;

namespace WordBattle
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (WordBattleGame game = new WordBattleGame())
            {
                game.Run();
            }
        }
    }
#endif
}

