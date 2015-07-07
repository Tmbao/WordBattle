using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.InvisibleGameEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordBattle.Utilities;

namespace WordBattle.VisibleGameEntities
{
    class GameNotification : VisibleGameEntity
    {
        private static GameNotification instance;

        public static GameNotification GetInstance()
        {
            if (instance == null)
                instance = new GameNotification();
            return instance;
        }

        private GameNotification()
        {
            messages = new List<string>();
            intensity = new List<float>();
            entityPhase = Phase.IN_GAME_ACHIEVING_FINISHED;
        }

        List<string> messages;
        List<float> intensity;

        Phase entityPhase;

        public Phase EntityPhase
        {
            get { return entityPhase; }
            set { entityPhase = value; }
        }

        public void PushMessage(string message)
        {
            messages.Add(message.ToUpper());
            intensity.Add(Consts.INTENSITY_NOTIFICATION + Consts.INTENSITY_NOTIFICATION_DELTA);

            entityPhase = Phase.IN_GAME_ACHIEVING;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateIntensity();

            base.Update(gameTime);
        }

        private void UpdateIntensity()
        {
            for (int index = 0; index < intensity.Count; index++) 
            {
                intensity[index] -= Consts.INTENSITY_NOTIFICATION_DELTA;
                if (intensity[index] < 0)
                    intensity[index] = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = TileSpriteContainer.GetInstance();
            float top = Consts.NOTIFICATION_TOP;
            for (int index = messages.Count - 1; index >= 0; index--)
            {
                float shift;
                if (messages[index].Length <= 14)
                {
                    shift = (Consts.NOTIFICATION_WIDTH - Utils.GetTextWidth(messages[index], Consts.NOTIFICATION_TEXT_SIZE, Consts.TEXT_SPACING)) / 2f;
                    tiles.DrawText(gameTime, spriteBatch, messages[index],
                        Consts.NOTIFICATION_LEFT + shift,
                        top,
                        Consts.NOTIFICATION_TEXT_SIZE,
                        intensity[index]);
                    top -= Consts.NOTIFICATION_TEXT_SIZE + Consts.COMPONENT_SPACING;
                }
                else
                {
                    string message1 = messages[index].Substring(0, (messages[index].Length + 1) / 2);
                    shift = (Consts.NOTIFICATION_WIDTH - Utils.GetTextWidth(message1, Consts.NOTIFICATION_TEXT_SIZE, Consts.TEXT_SPACING)) / 2f;
                    tiles.DrawText(gameTime, spriteBatch, message1,
                        Consts.NOTIFICATION_LEFT + shift,
                        top,
                        Consts.NOTIFICATION_TEXT_SIZE,
                        intensity[index]);
                    top -= Consts.NOTIFICATION_TEXT_SIZE + Consts.COMPONENT_SPACING / 2f;

                    string message2 = messages[index].Substring(message1.Length, messages[index].Length - message1.Length);
                    shift = (Consts.NOTIFICATION_WIDTH - Utils.GetTextWidth(message2, Consts.NOTIFICATION_TEXT_SIZE, Consts.TEXT_SPACING)) / 2f;
                    tiles.DrawText(gameTime, spriteBatch, message2,
                        Consts.NOTIFICATION_LEFT + shift,
                        top,
                        Consts.NOTIFICATION_TEXT_SIZE,
                        intensity[index]);
                    top -= Consts.NOTIFICATION_TEXT_SIZE + Consts.COMPONENT_SPACING;
                }
            }

            switch (entityPhase)
            {
                case Phase.IN_GAME_ACHIEVING:
                    // Finish drawing notification
                    if (IsIntensityAllZeros())
                    {
                        entityPhase = Phase.IN_GAME_ACHIEVING_FINISHED;
                        messages.Clear();
                        intensity.Clear();
                    }
                    break;
            }
        }

        // Return true if intensity array is all zeros, false otherwise
        private bool IsIntensityAllZeros()
        {
            for (int index = 0; index < intensity.Count; index++)
                if (intensity[index] != 0)
                    return false;
            return true;
        }
    }
}
