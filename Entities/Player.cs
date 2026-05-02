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
        private double _currentTime, _shootingTime=0;
        private static double _holdProjectile = 300;

        internal Player(Texture2D currentImg, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality) : base(currentImg, vector, sizeX, sizeY, blocking, shootable, power,vitality)
        {
           
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {
            move(gameTime, gm);
            _currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (InputManager.IsDown(Keys.Space) && _currentTime > _shootingTime+_holdProjectile)
            {
                gm.Shooting(this);
                _shootingTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

        }

        private void move(GameTime gameTime, GameManager gm)
        {
            Vector2 velocity = Vector2.Zero;

            if (InputManager.IsDown(Keys.Up))
            {
                base.SetDirection(Direction.Up);
                velocity.Y -= 1;
            }
            else if (InputManager.IsDown(Keys.Down))
            {
                base.SetDirection(Direction.Down);
                velocity.Y += 1;
            }
            else if (InputManager.IsDown(Keys.Left))
            {
                base.SetDirection(Direction.Left);
                velocity.X -= 1;
            }
            else if (InputManager.IsDown(Keys.Right))
            {
                base.SetDirection(Direction.Right);
                velocity.X += 1;
            }


            
            Vector2 movement = velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            gm.MoveWithClamp(velocity, movement, this);
        }

        internal override void Draw(Renderer renderer)
        {
            renderer.Draw(base.GetCurrentImg(), base.GetVector(), Color.White);
        }
    }
}
