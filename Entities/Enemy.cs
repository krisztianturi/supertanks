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
        private float _speed = 200f;
        internal Enemy(Texture2D up, Texture2D down, Texture2D left, Texture2D right, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality, Animation animation) : base(up, down, left, right, vector, sizeX, sizeY, blocking, shootable, power, vitality, animation)
        {
            Invulnerable = true;
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;

            if (this.Invulnerable)
            {
                if (this.BecameInvulnerableTime == 0)
                {
                    BecameInvulnerableTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (currentTime > BecameInvulnerableTime+ this.GetInvulnerableTime())
                {
                    Invulnerable = false;
                    BecameInvulnerableTime = 0;
                }
            }



            Vector2 velocity = Vector2.Zero;


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
                        base.SetCurrentImg(base.GetLeftImg());
                    }
                    break;
                case Direction.Right:
                    {
                        velocity.X += 1;
                        base.SetCurrentImg(base.GetRightImg());
                    }
                    break;
                case Direction.Up:
                    {
                        velocity.Y -= 1;
                        base.SetCurrentImg(base.GetUpImg());
                    }
                    break;
                case Direction.Down:
                    {
                        velocity.Y += 1;
                        base.SetCurrentImg(base.GetDownImg());
                    }
                    break;
            }
            float finalSpeed = OnIce ? _speed / 2 : _speed;
            Vector2 movement = velocity * finalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            gm.MoveWithClamp(velocity, movement, this);

            if (currentTime > _shootingTime + 1000)
            {
                gm.Shooting(this);
                _shootingTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            
        }

        internal void HitHandle(GameManager gm)
        {
            if (!this.Invulnerable)
            {
                if (this.HasShip)
                {
                    HasShip = false;
                    return;
                }
                else if (this.GetPower() >= 3)
                {
                    this.SetPower(this.GetPower() - 3);
                    return;
                }
                else if (this.GetVitality() == 1)
                {
                    gm.RemoveEnemy(this);
                    return;
                }
                this.SetVitality(this.GetVitality() - 1);
                gm.MakeBoost();
            }

        }

        internal override void Draw(Renderer renderer)
        {
            if (GetVitality()>1)
            {
                renderer.DrawWithDepth(base.GetCurrentImg(), base.GetVector(), Color.Pink,0.1f);
            }
            else
            {
                renderer.DrawWithDepth(base.GetCurrentImg(), base.GetVector(), Color.White, 0.1f);
            }

            if (this.Invulnerable)
            {
                renderer.DrawWithDepth(this.GetAnimation().getImage(), base.GetVector(), Color.White, 0.1f);
            }
        }


    }
}
