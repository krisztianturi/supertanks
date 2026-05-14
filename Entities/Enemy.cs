using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Core;
using SuperTanks.Systems;
using System;

namespace SuperTanks.Entities
{
    internal class Enemy : LivingObject
    {
        private double _directionTime,_shootingTime, _LimitTime=100;
        private float speed = 200f;

        internal Enemy(Texture2D currentImg, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality) : base(currentImg, vector, sizeX, sizeY, blocking, shootable, power, vitality)
        {
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {

            Vector2 velocity = Vector2.Zero;

            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (currentTime > _directionTime + _LimitTime)
            {
                this.SetDirection(DirectionHelper.GetRandom());
                _directionTime = gameTime.TotalGameTime.TotalMilliseconds;
            }


            switch (this.GetDirection())
            {
                case Direction.Left:
                    {
                        velocity.X -= 1;
                    }
                    break;
                case Direction.Right:
                    {
                        velocity.X += 1;
                    }
                    break;
                case Direction.Up:
                    {
                        velocity.Y -= 1;
                    }
                    break;
                case Direction.Down:
                    {
                        velocity.Y += 1;
                    }
                    break;
            }

            Vector2 movement = velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            gm.MoveWithClamp(velocity, movement, this);

            if (currentTime > _shootingTime + 1000)
            {
                gm.Shooting(this);
                _shootingTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            
        }

        internal void HitHandle(GameManager gm)
        {
            if (this.GetVitality() == 1)
            {
                gm.RemoveObject(this);
                return;
            }
            this.SetVitality(this.GetVitality() - 1);
        }

        internal override void Draw(Renderer renderer)
        {
            renderer.Draw(base.GetCurrentImg(), base.GetVector(), Color.White);
        }


    }
}
