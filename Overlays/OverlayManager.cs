using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace SuperTanks.Overlays
{
    internal class OverlayManager
    {
        private static IOverlay _current;
        private static IOverlay _next;

        private static IOverlay _previous;

        private static Action _onExit;

        private OverlayManager() { }

        internal static void RequestChange(IOverlay overlay)
        {
            _next = overlay;
        }

        public static void Update(GameTime gameTime)
        {
            _current.Update(gameTime);

            if (_next != null)
            {
                _previous = _current;
                _current = _next;
                _next = null;
            }
        }

        public static void Draw(SpriteBatch _spriteBatch)
        {
            _current.Draw(_spriteBatch);
        }

        internal static void SetOnExit(Action action) { _onExit = action; }
        internal static Action GetExitAction() { return _onExit; }
        internal static IOverlay GetPrevious() { return _previous; }
        internal static void SetCurrentOverlay(IOverlay overlay) 
        {
            if (_current != null)
                throw new Exception("IOverlay already initialized");

            _current = overlay;
        }
    }
}
