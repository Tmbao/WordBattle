﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WordBattle.ControllerGameEntities
{
    public class MouseController : ControllerEntity<MouseState>
    {
        private static MouseController instance;

        public static MouseController GetInstance() 
        {
            if (instance == null)
                instance = new MouseController();
            return instance;
        }

        private MouseController() 
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Program.game.IsActive)
            {
                // The mouse has been already updated
                if (lastUpdate == gameTime.TotalGameTime.Milliseconds)
                    return;

                lastUpdate = gameTime.TotalGameTime.Milliseconds;

                base.Update(gameTime);
                currentState = Mouse.GetState();

                if (IsLeftButtonPressed())
                    Global.clickSound.Play();
            }
        }

        public Vector2 GetCurrentMousePosition()
        {
            return new Vector2(currentState.X, currentState.Y);
        }

        public Vector2 GetMousePositionDifferent()
        {
            return new Vector2(currentState.X - previousState.X, currentState.Y - previousState.Y);
        }

        public bool IsLeftButtonDown()
        {
            return currentState.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonUp()
        {
            return currentState.LeftButton == ButtonState.Released;
        }

        public bool IsLeftButtonPressed()
        {
            return currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;
        }

        public bool IsLeftButtonReleased()
        {
            return currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightButtonDown()
        {
            return currentState.RightButton == ButtonState.Pressed;
        }

        public bool IsRightButtonUp()
        {
            return currentState.RightButton == ButtonState.Released;
        }

        public bool IsRightButtonPressed()
        {
            return currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released;
        }

        public bool IsRightButtonReleased()
        {
            return currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed;
        }

    }
}
