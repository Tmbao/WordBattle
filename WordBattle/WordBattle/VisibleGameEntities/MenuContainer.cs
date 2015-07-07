using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordBattle.InvisibleGameEntities;

namespace WordBattle.VisibleGameEntities
{
    class MenuContainer : VisibleGameEntity
    {
        private static MenuContainer instance;

        public static MenuContainer GetInstance()
        {
            if (instance == null)
                instance = new MenuContainer();
            return instance;
        }

        MenuOption menuSingle;
        MenuOption menuMulti;
        MenuOption menuNetwork;

        TileTextBox textPlayer1;
        TileTextBox textPlayer2;
        TileTextBox textRoomName;

        public Phase EnityPhase
        {
            get {
                if (menuSingle.EntityPhase == Phase.MENU_LOADING_FINISHED &&
                    menuMulti.EntityPhase == Phase.MENU_LOADING_FINISHED &&
                    menuNetwork.EntityPhase == Phase.MENU_LOADING_FINISHED)
                    return Phase.MENU_LOADING_FINISHED;
                if (menuSingle.EntityPhase == Phase.MENU_SELECTED_ANIMATING_FINISHED &&
                    menuMulti.EntityPhase == Phase.MENU_SELECTED_ANIMATING_FINISHED &&
                    menuNetwork.EntityPhase == Phase.MENU_SELECTED_ANIMATING_FINISHED)
                    return Phase.MENU_SELECTED_ANIMATING_FINISHED;
                return Phase.NONE;
            }
        }

        private MenuContainer()
        {

            float textLeft = (Consts.SCREEN_WIDTH - Utils.GetTextboxWidth()) / 2f;

            textPlayer1 = new TileTextBox(
                textLeft,
                Consts.MENU_TOP,
                "PLAYER1");
            textPlayer2 = new TileTextBox(
                textLeft,
                Consts.MENU_TOP + Consts.MENU_FONT_SIZE + 2 * Consts.COMPONENT_SPACING,
                "PLAYER2");
            textRoomName = new TileTextBox(
                textLeft,
                Consts.MENU_TOP + 2 * Consts.MENU_FONT_SIZE + 4 * Consts.COMPONENT_SPACING,
                "ROOMID");

            menuSingle = new MenuOption(
                (Consts.SCREEN_WIDTH - Utils.GetMenuOptionWidth("SINGLE")) / 2f, 
                Consts.MENU_TOP + 3 * Consts.MENU_FONT_SIZE + 12 * Consts.COMPONENT_SPACING,
                "SINGLE",
                Utils.GetImageFileName("Single"));
            menuMulti = new MenuOption(
                (Consts.SCREEN_WIDTH - Utils.GetMenuOptionWidth("DUEL")) / 2f, 
                Consts.MENU_TOP + 4 * Consts.MENU_FONT_SIZE + 14 * Consts.COMPONENT_SPACING,
                "DUEL",
                Utils.GetImageFileName("Multi"));
            menuNetwork = new MenuOption(
                (Consts.SCREEN_WIDTH - Utils.GetMenuOptionWidth("ONLINE")) / 2f, 
                Consts.MENU_TOP + 5 * Consts.MENU_FONT_SIZE + 16 * Consts.COMPONENT_SPACING,
                "ONLINE",
                Utils.GetImageFileName("Network"));

            
        }

        public void UpdatePhase(Phase phase)
        {
            menuSingle.EntityPhase = phase;
            menuMulti.EntityPhase = phase;
            menuNetwork.EntityPhase = phase;

            textPlayer1.EntityPhase = phase;
            textPlayer2.EntityPhase = phase;
            textRoomName.EntityPhase = phase;
        }

        public override void Update(GameTime gameTime)
        {
            menuSingle.Update(gameTime);
            menuMulti.Update(gameTime);
            menuNetwork.Update(gameTime);
            textPlayer1.Update(gameTime);
            textPlayer2.Update(gameTime);
            textRoomName.Update(gameTime);
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menuSingle.Draw(gameTime, spriteBatch);
            menuMulti.Draw(gameTime, spriteBatch);
            menuNetwork.Draw(gameTime, spriteBatch);
            textPlayer1.Draw(gameTime, spriteBatch);
            textPlayer2.Draw(gameTime, spriteBatch);
            textRoomName.Draw(gameTime, spriteBatch);
        }

        public GameMode GetSelectedMode()
        {
            if (menuSingle.IsSelected)
                return GameMode.SINGLE;
            if (menuMulti.IsSelected)
                return GameMode.MULTI;
            if (menuNetwork.IsSelected)
                return GameMode.NETWORK;
            return GameMode.NONE;
        }

        public string GetPlayer1Name()
        {
            return textPlayer1.Text;
        }

        public string GetPlayer2Name()
        {
            return textPlayer2.Text;
        }

        public string GetRoomName()
        {
            return textRoomName.Text;
        }
    }
}
