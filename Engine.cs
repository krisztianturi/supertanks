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

            _graphics.IsFullScreen = false;
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

            Assets._playerUp = Content.Load<Texture2D>("images/up");
            Assets._playerDown = Content.Load<Texture2D>("images/down");
            Assets._playerLeft = Content.Load<Texture2D>("images/left");
            Assets._playerRight = Content.Load<Texture2D>("images/right");


            Assets._edgeAll = Content.Load<Texture2D>("images/edges");

            Assets.Test = Content.Load<Texture2D>("images/test");
            Assets._enemy = Content.Load<Texture2D>("images/enemy");
            Assets._testProj = Content.Load<Texture2D>("images/testProj");

            Assets._wall = Content.Load<Texture2D>("images/wall");
            Assets._rock = Content.Load<Texture2D>("images/rock");
            Assets._eagle = Content.Load<Texture2D>("images/eagle");

            Assets._projectile = Content.Load<Texture2D>("images/projectile");

            Assets._gameOver = Content.Load<Texture2D>("images/gameOver"); ;


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

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
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
