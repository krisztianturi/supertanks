

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Systems;

namespace SuperTanks.Overlays
{
    internal class GameOverOverlay : IOverlay
    {

        public void Update(GameTime gameTime)
        {
            if (InputManager.ExitPressed())
            {
                OverlayManager.RequestChange(new MenuOverlay());
            }
            if (InputManager.ConfirmPressed())
            {
                OverlayManager.RequestChange(new SinglePlayerOverlay());

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D gameOverImg = Assets._gameOver;
            float screenWidth = Engine.GetScreenWidth();
            float screenHeight = Engine.GetScreenHeight();
            spriteBatch.Draw(gameOverImg, new Vector2(screenWidth/2 - gameOverImg.Width/2, screenHeight/2 -gameOverImg.Height/2 ), Color.White);
        }


    }
}
