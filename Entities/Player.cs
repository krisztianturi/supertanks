using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Core;
using SuperTanks.Systems;



namespace SuperTanks.Entities
{
    internal class Player : LivingObject
    {

        private float speed = 500f;
        private double _shootingTime=0;
        private static double _holdProjectile = 300;

        private Vector2 _originPosition;
        private Texture2D _up, _down, _left, _right, _currentImg;

        internal Player(Texture2D up, Texture2D down, Texture2D left, Texture2D right, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality, Animation animation) : base(up, down, left, right, vector, sizeX, sizeY, blocking, shootable, power,vitality,animation)
        {
            _currentImg = up;
           _originPosition = new Vector2(vector.X, vector.Y);
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {
            Move(gameTime, gm);
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (InputManager.IsDown(Keys.Space) && currentTime > _shootingTime+_holdProjectile)
            {
                gm.Shooting(this);
                _shootingTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (this.Invulnerable)
            {
                this.GetAnimation().Update();
                if (BecameInvulnerableTime==0)
                {
                    BecameInvulnerableTime = gameTime.TotalGameTime.TotalMilliseconds;
                }                
                else if (currentTime > GetInvulnerableTime() + BecameInvulnerableTime)
                {
                    this.Invulnerable = false;
                    BecameInvulnerableTime = 0;
                }
                
            }

        }

        private void Move(GameTime gameTime, GameManager gm)
        {
            Vector2 velocity = Vector2.Zero;

            if (InputManager.IsDown(Keys.Up))
            {
                base.SetDirection(Direction.Up);
                velocity.Y -= 1;
                base.SetCurrentImg(base.GetUpImg());
            }
            else if (InputManager.IsDown(Keys.Down))
            {
                base.SetDirection(Direction.Down);
                velocity.Y += 1;
                base.SetCurrentImg(base.GetDownImg());
            }
            else if (InputManager.IsDown(Keys.Left))
            {
                base.SetDirection(Direction.Left);
                velocity.X -= 1;
                base.SetCurrentImg(base.GetLeftImg());
            }
            else if (InputManager.IsDown(Keys.Right))
            {
                base.SetDirection(Direction.Right);
                velocity.X += 1;
                base.SetCurrentImg(base.GetRightImg());
            }


            
            Vector2 movement = velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            gm.MoveWithClamp(velocity, movement, this);
            gm.CheckBoost(this);
        }

        internal void HitHandle(GameManager gm)
        {
            if (!this.Invulnerable)
            {
                if (this.GetPower() >= 3)
                {
                    this.SetPower(this.GetPower() - 3);
                    this.Invulnerable = true;
                    return;
                }
                else if (this.GetVitality() == 1)
                {
                    gm.SetGameOver();
                    return;
                }
                this.SetVector(_originPosition);
                gm.UpdateObject(this);
                this.SetVitality(this.GetVitality() - 1);
                this.Invulnerable = true;
            }
            
        }

        internal override void Draw(Renderer renderer)
        {
            renderer.Draw(base.GetCurrentImg(), base.GetVector(), Color.White);
            if (this.Invulnerable)
            {
                renderer.Draw(this.GetAnimation().getImage(), base.GetVector(), Color.White);
            }
        }
    }
}
