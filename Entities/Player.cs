using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Core;
using SuperTanks.Systems;



namespace SuperTanks.Entities
{
    internal class PlayerInput
    {
        internal Keys Left { get; }
        internal Keys Right { get; }
        internal Keys Up { get; }
        internal Keys Down { get; }
        internal Keys Fire { get; }

        internal PlayerInput (Keys left, Keys right, Keys up, Keys down, Keys fire)
        {
            Left = left;
            Right = right;
            Up = up;
            Down = down;
            Fire = fire;            
        }

    }


    internal class Player : LivingObject
    {

        private float _speed = 500f;
        private double _shootingTime=0;
        private static double _holdProjectile = 300;

        private Vector2 _originPosition;
        private Texture2D _up, _down, _left, _right, _currentImg;

        private PlayerInput _input;

        internal Player(Texture2D up, Texture2D down, Texture2D left, Texture2D right, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable,bool ship, int power, int vitality, Animation animation, PlayerInput input) : base(up, down, left, right, vector, sizeX, sizeY, blocking, shootable, power, vitality, animation)
        {
            _currentImg = up;
            HasShip = ship;
            _originPosition = new Vector2(vector.X, vector.Y);
            _input = input;
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {
            Move(gameTime, gm);
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (InputManager.IsDown(_input.Fire) && currentTime > _shootingTime+_holdProjectile)
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

            if (InputManager.IsDown(_input.Up))
            {
                base.SetDirection(Direction.Up);
                velocity.Y -= 1;
                base.SetCurrentImg(base.GetUpImg());
            }
            else if (InputManager.IsDown(_input.Down))
            {
                base.SetDirection(Direction.Down);
                velocity.Y += 1;
                base.SetCurrentImg(base.GetDownImg());
            }
            else if (InputManager.IsDown(_input.Left))
            {
                base.SetDirection(Direction.Left);
                velocity.X -= 1;
                base.SetCurrentImg(base.GetLeftImg());
            }
            else if (InputManager.IsDown(_input.Right))
            {
                base.SetDirection(Direction.Right);
                velocity.X += 1;
                base.SetCurrentImg(base.GetRightImg());
            }

            float finalSpeed = OnIce ? _speed / 2 : _speed;
            
            Vector2 movement = velocity * finalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            gm.MoveWithClamp(velocity, movement, this);
            gm.CheckBoost(this);
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
            renderer.DrawWithDepth(base.GetCurrentImg(), base.GetVector(), Color.White,0.1f);
            if (this.Invulnerable)
            {
                renderer.DrawWithDepth(this.GetAnimation().getImage(), base.GetVector(), Color.White,0.1f);
            }
        }
    }
}
