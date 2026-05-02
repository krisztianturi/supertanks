using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Systems;
using System;
using System.Xml.Linq;


namespace SuperTanks.Overlays
{
    internal class MenuOverlay : IOverlay
    {        

        internal MenuOverlay()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.ExitPressed())
            {
                OverlayManager.GetExitAction().Invoke();
            }
            if (InputManager.ConfirmPressed())
            {
                OverlayManager.RequestChange(new SinglePlayerOverlay());

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int screenX = Engine.GetScreenWidth();
            int screenY = Engine.GetScreenHeight();
            Renderer renderer = new Renderer(spriteBatch);
            renderer.DrawString("Menu", new Vector2(screenX / 2, screenY / 2), Color.White, 5);
            float place = renderer.GetStringSize("Menu").Y * 5 +50;
            renderer.DrawString("Press enter to Single Player", new Vector2(screenX / 2, screenY / 2 +place), Color.White, 2);

        }


    }
}
