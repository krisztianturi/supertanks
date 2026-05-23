using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperTanks.Systems
{
    internal class Renderer
    {
        private SpriteBatch _sb;
        private static Vector2 _offset;
        private static Texture2D _pixel;
        private static GraphicsDevice _graphicsDevice;
        private static SpriteFont _font;

        internal static void SetGraphics(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        internal static void SetFont(SpriteFont font) { _font = font; }

        internal Renderer(SpriteBatch sb)
        {
            _sb = sb;            
        }

        internal static Vector2 Offset
        {
            get => _offset;
            set => _offset = value;
        }


        internal void Draw(Texture2D tex, Vector2 vector, Color color)
        {
            _sb.Draw(tex, vector+Offset, color);
        }

        internal void DrawString(string text, Vector2 vector, Color color, float scale)
        {
            _sb.DrawString(_font, text, vector, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        internal Vector2 GetStringSize(string text)
        {
            return _font.MeasureString(text);
        }

        internal void DrawWithRotation(Texture2D tex, Vector2 pos, Color color, float rotation)
        {
            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height / 2f);
            Vector2 centerPos = pos + origin;
            _sb.Draw(tex, centerPos + _offset, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }

        internal void DrawWithRotationAndScale(Texture2D tex, Vector2 pos, int sizeX, int sizeY, Color color, float rotation, Vector2 scale)
        {
            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height / 2f);
            Vector2 centerPos = pos + new Vector2(sizeX / 2f, sizeY / 2f);

            _sb.Draw(tex, centerPos + _offset, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        internal void DrawPixelRect(Rectangle rect, Color color)
        {
            rect.X += (int)_offset.X;
            rect.Y += (int)_offset.Y;
            _sb.Draw(_pixel, rect, color);
        }

        internal void DrawRect(Texture2D tex, Rectangle rect, Color color)
        {
            rect.X += (int)_offset.X;
            rect.Y += (int)_offset.Y;
            _sb.Draw(tex, rect, color);
        }

        internal void DrawWithRect(Texture2D tex, Vector2 vector, Rectangle rect, Color color)
        {
            _sb.Draw(tex, vector + _offset, rect, color);
        }

        internal void DrawWithDepth(Texture2D tex, Vector2 vector, Color color)
        {
            _sb.Draw(tex, vector, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
        }

    }
}
