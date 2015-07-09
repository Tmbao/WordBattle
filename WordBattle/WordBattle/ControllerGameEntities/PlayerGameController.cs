using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WordBattleCore.GridEntities;
using WordBattle.Utilities;
using Microsoft.Xna.Framework.Input;
using WordBattlePlayer;
using System.Net;

namespace WordBattle.ControllerGameEntities
{
    public class PlayerGameController : GameController
    {
        private static PlayerGameController playerGameController;

        public static PlayerGameController GetInstance() {
            if (playerGameController == null) 
                playerGameController = new PlayerGameController();
            return playerGameController;
        }

        KeyboardController keyboardController;
        MouseController mouseController;

        private PlayerGameController()
        {
            keyboardController = KeyboardController.GetInstance();
            mouseController = MouseController.GetInstance();
        }

        public override void Update(GameTime gameTime)
        {
            keyboardController.Update(gameTime);
            mouseController.Update(gameTime);

            UpdatePressedCharacters();
            UpdateSelectedIndex();
        }

        private void UpdateSelectedIndex()
        {
            var currentPosition = Vector2.Transform(mouseController.GetCurrentMousePosition(), Global.MainCamera.InvertWVP);
            if (mouseController.IsLeftButtonPressed())
                selectedIndex = Utils.GetIndexOfMouse(currentPosition);
        }

        private void UpdatePressedCharacters()
        {
            var pressedKey = keyboardController.PressedKey;
            if (pressedKey != Keys.None)
            {
                pressedCharacter = pressedKey.ToString();
                if (pressedCharacter != null && pressedCharacter.Length == 1 && Utils.IsLetter(pressedCharacter[0])) ;
                else
                    pressedCharacter = null;
            }
            else
                pressedCharacter = null;
        }

        string pressedCharacter;

        public override string PressedCharacter()
        {
            return pressedCharacter;
        }

        Tuple<int, int> selectedIndex;

        public override Tuple<int, int> SelectedIndex()
        {
            if (WordGrid.GetInstance().CanFill(selectedIndex))
                return selectedIndex;
            else 
                return null;
        }
    }
}
