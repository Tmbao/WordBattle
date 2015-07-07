using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WordBattle.ControllerGameEntities
{
    public class KeyboardController : ControllerEntity<KeyboardState>
    {
        private static KeyboardController instance;

        public static KeyboardController GetInstance()
        {
            if (instance == null)
                instance = new KeyboardController();
            return instance;
        }

        private KeyboardController()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Program.game.IsActive)
            {
                // The keyboard has already updated
                if (lastUpdate == gameTime.TotalGameTime.Milliseconds)
                    return;

                lastUpdate = gameTime.TotalGameTime.Milliseconds;

                base.Update(gameTime);

                currentState = Keyboard.GetState();

                var pressedKeys = currentState.GetPressedKeys();
                pressedKey = Keys.None;
                foreach (var key in pressedKeys)
                {
                    if (IsKeyPressed(key))
                        pressedKey = key;
                }

                if (pressedKey != Keys.None)
                    Global.clickSound.Play();
            }
        }

        Keys pressedKey;

        public Keys PressedKey
        {
            get { return pressedKey; }
        }

        public bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return currentState.IsKeyUp(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return previousState.IsKeyDown(key) && currentState.IsKeyUp(key);
        }
    }
}
