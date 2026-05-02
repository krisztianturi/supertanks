using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Core;
using SuperTanks.Systems;

namespace SuperTanks.Overlays
{
    internal class SinglePlayerOverlay : IOverlay
    {

        private static int _edgeSize;
        private static int _drawSizeX, _drawSizeY;
        private static Rectangle[] _edges;

        private GameManager _gameManager;

        internal SinglePlayerOverlay()
        {
            _gameManager = GameCreator.Instance.CreateSinglePlayerGame();
        }


        public void Update(GameTime gameTime)
        {
            _gameManager.UpdateGame(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Renderer renderer = new Renderer(spriteBatch);

            renderer.DrawPixelRect(new Rectangle(0, 0, _drawSizeX, _drawSizeY), Color.Black);
            renderer.DrawWithRect(Assets._edgeAll, new Vector2(0, 0), _edges[0], Color.White);
            renderer.DrawWithRect(Assets._edgeAll, new Vector2(_drawSizeX - _edgeSize, 0), _edges[1], Color.White);
            renderer.DrawWithRect(Assets._edgeAll, new Vector2(0, _drawSizeY - _edgeSize), _edges[2], Color.White);
            renderer.DrawWithRect(Assets._edgeAll, new Vector2(_drawSizeX - _edgeSize, _drawSizeY - _edgeSize), _edges[3], Color.White);

            for (int i = 1; i < _drawSizeX / _edgeSize - 1; i++)
            {
                renderer.DrawWithRect(Assets._edgeAll, new Vector2(i * _edgeSize, 0), _edges[4], Color.White);
                renderer.DrawWithRect(Assets._edgeAll, new Vector2(i * _edgeSize, _drawSizeY - _edgeSize), _edges[4], Color.White);
            }

            for (int i = 1; i < _drawSizeY / _edgeSize - 1; i++)
            {
                renderer.DrawWithRect(Assets._edgeAll, new Vector2(0, i * _edgeSize), _edges[6], Color.White);
                renderer.DrawWithRect(Assets._edgeAll, new Vector2(_drawSizeX - _edgeSize, i * _edgeSize), _edges[7], Color.White);
            }

            _gameManager.Draw(renderer);
        }

        internal static void SetEdgeSize(int edgeSize, int drawSizeX, int drawSizeY)
        {
            _edgeSize = edgeSize;
            _drawSizeX = drawSizeX;
            _drawSizeY = drawSizeY;
            _edges = Load1D(Assets._edgeAll, _edgeSize, _edgeSize);
        }

        internal static Rectangle[] Load1D(Texture2D texture, int frameWidth, int frameHeight)
        {
            int count = texture.Width / frameWidth;
            Rectangle[] frames = new Rectangle[count];

            for (int i = 0; i < count; i++)
            {
                frames[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
            }

            return frames;
        }


    }
}
