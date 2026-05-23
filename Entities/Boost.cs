using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Core;
using SuperTanks.Systems;

namespace SuperTanks.Entities
{
    internal class Boost : GameObject
    {
        private BoostType _boostType;

        internal Boost(Texture2D currentImg, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, BoostType type) : base(currentImg, vector, sizeX, sizeY, blocking, shootable)
        {
            _boostType = type;
        }

        internal override void Draw(Renderer renderer)
        {
            renderer.Draw(this.GetCurrentImg(), this.GetVector(),Color.White*0.5f);
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {
           
        }

        internal BoostType GetBoostType() { return _boostType; }
    }
}
