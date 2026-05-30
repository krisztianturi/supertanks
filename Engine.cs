using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Overlays;
using SuperTanks.Systems;

namespace SuperTanks
{
    public class Engine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private static int _screenWidth, _screenHeight;

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            var dm = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            _screenWidth = _graphics.PreferredBackBufferWidth = dm.Width;
            _screenHeight = _graphics.PreferredBackBufferHeight = dm.Height;

            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            OverlayManager.SetOnExit(Exit);
            OverlayManager.SetCurrentOverlay(new MenuOverlay());
            base.Initialize();
            Renderer.SetGraphics(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets._up = Content.Load<Texture2D>("images/up");
            Assets._down = Content.Load<Texture2D>("images/down");
            Assets._left = Content.Load<Texture2D>("images/left");
            Assets._right = Content.Load<Texture2D>("images/right");


            Assets._edgeAll = Content.Load<Texture2D>("images/edges");

            Assets.Test = Content.Load<Texture2D>("images/test");
            Assets._enemy = Content.Load<Texture2D>("images/enemy");
            Assets._testProj = Content.Load<Texture2D>("images/testProj");

            Assets._wall = Content.Load<Texture2D>("images/wall");
            Assets._rock = Content.Load<Texture2D>("images/rock");
            Assets._water = Content.Load<Texture2D>("images/water");
            Assets._grass = Content.Load<Texture2D>("images/grass");
            Assets._ice = Content.Load<Texture2D>("images/ice");
            Assets._eagle = Content.Load<Texture2D>("images/eagle");

            Assets._projectile = Content.Load<Texture2D>("images/projectile");

            Assets._gameOver = Content.Load<Texture2D>("images/gameOver");

            Assets._boostStar = Content.Load<Texture2D>("images/boostStar");
            Assets._boostGun = Content.Load<Texture2D>("images/boostGun");
            Assets._boostLife = Content.Load<Texture2D>("images/boostLife");
            Assets._boostSpade = Content.Load<Texture2D>("images/boostSpade");
            Assets._boostShip = Content.Load<Texture2D>("images/boostShip");

            Assets._explosion[0] = Content.Load<Texture2D>("images/Explo1");
            Assets._explosion[1] = Content.Load<Texture2D>("images/Explo2");
            Assets._explosion[2] = Content.Load<Texture2D>("images/Explo3");

            Assets._invulnerable[0] = Content.Load<Texture2D>("images/invulne1");
            Assets._invulnerable[1] = Content.Load<Texture2D>("images/invulne2");


            Renderer.SetFont(Content.Load<SpriteFont>("File"));
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            OverlayManager.Update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Azure);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);
            OverlayManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        internal static int GetScreenWidth()
        {
            return _screenWidth;
        }

        internal static int GetScreenHeight()
        {
            return _screenHeight;
        }
    }

}
