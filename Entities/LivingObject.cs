using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Core;
using SuperTanks.Systems;
using System;

namespace SuperTanks.Entities
{
    internal abstract class LivingObject : GameObject
    {
        private Direction _direction = Direction.Up;
        private int _power;
        private int _vitality;
        protected bool Invulnerable { get; set; }
        private double _invulnerableTime = 1000;
        public double BecameInvulnerableTime { get; set; }

        private Animation _animation;
        private readonly Texture2D _up, _down, _left, _right;

        internal LivingObject(Texture2D up, Texture2D down, Texture2D left, Texture2D right, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality, Animation animation) : base(vector, sizeX, sizeY, blocking, shootable)
        {
            _power = power;
            _vitality = vitality;
            _animation = animation;
            _up = up;
            _down = down;
            _left = left;
            _right = right;
            this.SetCurrentImg(up);
        }

        internal Animation GetAnimation() { return _animation; }

        internal Direction GetDirection() { return _direction; }
        internal void SetDirection(Direction direction) { _direction = direction; }
        internal int GetPower() { return _power; }
        internal void SetPower(int power) { _power = power; }
        internal int GetVitality() { return _vitality; }
        internal void SetVitality(int vitality) {  _vitality = vitality; }
        internal double GetInvulnerableTime() { return _invulnerableTime; }
        protected Texture2D GetUpImg() { return _up; }
        protected Texture2D GetDownImg() { return _down; }
        protected Texture2D GetLeftImg() { return _left; }
        protected Texture2D GetRightImg() { return _right; }

        internal void HandleBoost(BoostType boostType)
        {
            int maxPower = 7;
            switch (boostType)
            {
                case BoostType.Life: _vitality++; break;
                case BoostType.Star: if(_power< maxPower) _power++; break;
                case BoostType.Gun: if (_power < 4) _power = _power + 3; else _power = maxPower; break;
            }
        }
    }
}
