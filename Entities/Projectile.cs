using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Core;
using SuperTanks.Systems;


namespace SuperTanks.Entities
{
    enum Team
    {
        Player,
        Enemy
    }

    internal class Projectile : GameObject
    {
        private Direction _direction;
        private float _rotation;
        private float _speed = 800f;
        private Team _owner;
        private static int _damage = 10;

        internal Team GetTeam() {  return _owner; }
        internal Direction GetDirection() { return _direction; }
        public Projectile(Texture2D currentImg, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, Direction direction, Team owner, int damage) : base(currentImg, vector, sizeX, sizeY, blocking, shootable)
        {
            this._direction = direction;
            switch (direction)
            {
                case Direction.Up:
                    _rotation = 0;
                    break;

                case Direction.Down:
                    _rotation = MathHelper.ToRadians(180f);
                    break;

                case Direction.Left:
                    _rotation = MathHelper.ToRadians(90f);
                    break;

                case Direction.Right:
                    _rotation = MathHelper.ToRadians(270f);
                    break;
            }

            _owner = owner;
            _damage = damage;
        }

        internal bool SizeReduction(Vector2 newPosition, int reduction)
        {
            int minSize = 25;
            switch (_direction)
            {
                case Direction.Left:
                    {
                        this.SetSizeX(this.GetSizeX() - reduction);
                        if (this.GetSizeX() <= minSize) return true;
                        this.SetVector(new Vector2(newPosition.X + reduction, newPosition.Y));
                    }
                    break;
                case Direction.Right:
                    {
                        this.SetSizeX(this.GetSizeX() - reduction);
                        if (this.GetSizeX() <= minSize) return true;
                        this.SetVector(newPosition);
                    }
                    break;
                case Direction.Up:
                    {
                        this.SetSizeY(this.GetSizeY() - reduction);
                        if (this.GetSizeY() <= minSize) return true;
                        this.SetVector(new Vector2(newPosition.X, newPosition.Y + reduction));

                    }
                    break;
                case Direction.Down:
                    {
                        this.SetSizeY(this.GetSizeY() - reduction);
                        if (this.GetSizeY() <= minSize) return true;
                        this.SetVector(newPosition);
                    }
                    break;

            }

            return false;
        }


        internal bool IsRemovableOrSizeReduction(Vector2 newPosition)
        {
            return SizeReduction(newPosition, _damage);
        }

        internal bool IsRemovableOrSizeReduction(Vector2 newPosition, int reduction)
        {
            return SizeReduction(newPosition, reduction);
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {
            Vector2 velocity = Vector2.Zero;

            switch (_direction)
            {
                case Direction.Up: velocity.Y -= 1; break;
                case Direction.Down: velocity.Y += 1; break;
                case Direction.Left: velocity.X -= 1; break;
                case Direction.Right: velocity.X += 1; break;
            }

            Vector2 position = base.GetVector();
            position += velocity * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            gm.MoveProjectile(position,this);


        }


        internal override void Draw(Renderer renderer)
        {
            Texture2D tex = base.GetCurrentImg();
            Vector2 scale;

            if (_direction == Direction.Up || _direction == Direction.Down)
            {
                scale.X = GetSizeX() / (float)tex.Width;
                scale.Y = GetSizeY() / (float)tex.Height;
            }
            else
            {
                scale.X = GetSizeY() / (float)tex.Width;
                scale.Y = GetSizeX() / (float)tex.Height;
            }
            renderer.DrawWithRotationAndScale(tex, this.GetVector(), this.GetSizeX(), this.GetSizeY(), Color.White, _rotation, scale);
        }

        internal static int GetDamage() {  return _damage; }
    }
}
