using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WordBattle.ControllerGameEntities
{
    public class MouseController : ControllerGameEntity<MouseState>
    {
        private static MouseController mouseController;

        public static MouseController GetInstance() 
        {
            if (mouseController == null)
                mouseController = new MouseController();
            return mouseController;
        }

        private MouseController() 
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            currentState = Mouse.GetState();
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
