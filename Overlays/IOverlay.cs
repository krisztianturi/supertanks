using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SuperTanks.Overlays
{
    internal interface IOverlay
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch renderer);
    }
}
