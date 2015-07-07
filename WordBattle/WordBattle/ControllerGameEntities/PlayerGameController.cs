using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WordBattleCore.GridEntities;
using WordBattle.Utilities;
using Microsoft.Xna.Framework.Input;
using WordBattlePlayer;

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
            var currentPosition = mouseController.GetCurrentMousePosition();
            if (mouseController.IsLeftButtonPressed())
                selectedIndex = Utils.GetIndexOfMouse(currentPosition);
        }

        private void UpdatePressedCharacters()
        {
            var pressedKey = keyboardController.PressedKey;
            if (pressedKey != Keys.None)
                pressedCharacters = pressedKey.ToString();
            else
                pressedCharacters = null;
        }

        string pressedCharacters;

        public override string PressedCharacters()
        {
            return pressedCharacters;
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
