using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace SuperTanks.Systems
{
    public static class InputManager
    {
        private static KeyboardState _currentKeyboard;
        private static KeyboardState _previousKeyboard;

        private static GamePadState _currentPad;
        private static GamePadState _previousPad;

        public static void Update()
        {
            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();

            _previousPad = _currentPad;
            _currentPad = GamePad.GetState(PlayerIndex.One);
        }

        public static bool IsDown(Keys key)
        {
            return _currentKeyboard.IsKeyDown(key);
        }

        public static bool IsPressed(Keys key)
        {
            return _currentKeyboard.IsKeyDown(key) &&
                   !_previousKeyboard.IsKeyDown(key);
        }

        public static bool IsReleased(Keys key)
        {
            return !_currentKeyboard.IsKeyDown(key) &&
                   _previousKeyboard.IsKeyDown(key);
        }


        public static bool IsDown(Buttons button)
        {
            return _currentPad.IsButtonDown(button);
        }

        public static bool IsPressed(Buttons button)
        {
            return _currentPad.IsButtonDown(button) &&
                   !_previousPad.IsButtonDown(button);
        }

        public static bool IsReleased(Buttons button)
        {
            return !_currentPad.IsButtonDown(button) &&
                   _previousPad.IsButtonDown(button);
        }

        public static bool ExitPressed()
        {
            return IsPressed(Keys.Escape) ||
                   IsPressed(Buttons.Back);
        }

        public static bool ConfirmPressed()
        {
            return IsPressed(Keys.Enter) ||
                   IsPressed(Buttons.Start) ||
                   IsPressed(Buttons.A);
        }

    }
}

