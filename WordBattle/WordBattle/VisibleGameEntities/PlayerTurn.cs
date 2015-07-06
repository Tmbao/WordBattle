using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.VisibleGameEntities;
using WordBattle.InvisibleGameEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordBattle.VisibleGameEntities
{
    class PlayerTurn : VisibleGameEntity
    {
        private static PlayerTurn playerTurn;

        public static PlayerTurn GetInstance()
        {
            if (playerTurn == null)
                playerTurn = new PlayerTurn();
            return playerTurn;
        }

        private PlayerTurn()
        {
            players = new PlayerEntity[2];
        }

        public void NewGame(PlayerEntity p1, PlayerEntity p2, int turn)
        {
            players[0] = p1;
            players[1] = p2;
            this.turn = turn;
        }

        int turn;
        PlayerEntity[] players;

        public PlayerEntity CurrentPlayer
        {
            get { return players[turn]; }
            set { players[turn] = value; }
        }

        public override void Update(GameTime gameTime)
        {
            switch (Global.CurrentPhase)
            {
                case Phase.IN_GAME_END_TURN:
                    // Change turn
                    turn ^= 1;
                    Global.CurrentPhase = Phase.IN_GAME_MOVING;
                    TilingGrid.GetInstance().EntityPhase = Phase.IN_GAME_MOVING;
                    break;
            }

            players[0].Update(gameTime);
            // players[1].Update(gameTime);

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            players[0].Draw(gameTime, spriteBatch);
            players[1].Draw(gameTime, spriteBatch);
        }
    }
}
