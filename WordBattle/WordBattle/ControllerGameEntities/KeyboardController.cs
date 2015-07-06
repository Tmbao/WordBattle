﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WordBattle.ControllerGameEntities
{
    public class KeyboardController : ControllerEntity<KeyboardState>
    {
        private static KeyboardController keyboardController;

        public static KeyboardController GetInstance()
        {
            if (keyboardController == null)
                keyboardController = new KeyboardController();
            return keyboardController;
        }

        private KeyboardController()
        {
        }

        public override void Update(GameTime gameTime)
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
