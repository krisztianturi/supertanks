using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SuperTanks.Systems
{
    internal class Assets
    {
        internal static Texture2D _playerUp;
        internal static Texture2D _playerDown;
        internal static Texture2D _playerLeft;
        internal static Texture2D _playerRight;

        
        internal static Texture2D _edgeAll;

        internal static Texture2D Test;

        internal static Texture2D _wall;
        internal static Texture2D _rock;
        internal static Texture2D _eagle;

        internal static Texture2D _testProj;
        internal static Texture2D _gameOver;
        internal static Texture2D _enemy;

        internal static Texture2D _projectile;

        internal static Rectangle[,] Load2D(Texture2D texture, int frameWidth, int frameHeight)
        {
            int countX = texture.Width / frameWidth;
            int countY = texture.Height / frameHeight;
            Rectangle[,] frames = new Rectangle[countX, countY];

            for (int i = 0; i < countX; i++)
            {
                for (int j = 0; j < countY; j++)
                {
                    frames[i, j] = new Rectangle(i * frameWidth, j * frameHeight, frameWidth, frameHeight);
                }
            }

            return frames;
        }

    }
}
