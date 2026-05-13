using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Core;

namespace SuperTanks.Entities
{
    internal abstract class LivingObject : GameObject
    {
        private Direction _direction = Direction.Up;
        private int _power;
        private int _vitality;
        internal LivingObject(Texture2D currentImg, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality) : base(currentImg, vector, sizeX, sizeY, blocking, shootable)
        {
            _power = power;
            _vitality = vitality;
        }

        internal Direction GetDirection() { return _direction; }
        internal void SetDirection(Direction direction) { _direction = direction; }
        internal int GetPower() { return _power; }
        internal void SetPower(int power) { _power = power; }
        internal int GetVitality() { return _vitality; }
        internal void SetVitality(int vitality) {  _vitality = vitality; }

    }
}
