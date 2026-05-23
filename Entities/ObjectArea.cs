using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Core;
using SuperTanks.Systems;
using System;

namespace SuperTanks.Entities
{
    internal class ObjectArea : GameObject
    {

        private AreaType _areaType;

        internal AreaType GetAreaType() { return _areaType; }

        internal ObjectArea(Texture2D currentImg, Vector2 vector,int sizeX, int sizeY, bool blocking, bool shootable, AreaType areaType) : base(currentImg, vector, sizeX, sizeY, blocking, shootable)
        {
            _areaType = areaType;
        }

        internal override void Draw(Renderer renderer)
        {
            if (_areaType!=AreaType.GRASS)
            {
                renderer.DrawWithDepth(base.GetCurrentImg(), base.GetVector(), Color.White, 0f);
            }
            else
            {
                renderer.DrawWithDepth(base.GetCurrentImg(), base.GetVector(), Color.White, 0.2f);
            }

        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {
            
        }

        internal bool IsRemovable(Rectangle targetRect, Rectangle damageBox, Direction direction)
        {
            Rectangle intersect = Rectangle.Intersect(targetRect, damageBox);

            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    {
                        if (intersect.Height >= targetRect.Height) return true;
                        else
                        {
                            int newSizeY = 0;
                            if (intersect.Top > targetRect.Top)
                            {
                                newSizeY = this.GetSizeY() - intersect.Height;
                            }
                            else if(intersect.Bottom < targetRect.Bottom)
                            {
                                this.SetVector(new Vector2(this.GetVector().X, this.GetVector().Y+intersect.Height));
                                newSizeY = this.GetSizeY() - intersect.Height;
                            }
                            if(newSizeY==0)  throw new InvalidOperationException("NewSize is 0 !");
                            this.SetSizeY(newSizeY);
                            return false;
                        }
                    }
                case Direction.Up:
                case Direction.Down:
                    {
                        if (intersect.Width >= targetRect.Width) return true;
                        else
                        {
                            int newSizeX = 0;
                            if (intersect.Left > targetRect.Left)
                            {
                                newSizeX = this.GetSizeX() - intersect.Width;
                            }
                            else if (intersect.Right < targetRect.Right)
                            {
                                this.SetVector(new Vector2(this.GetVector().X+ intersect.Width, this.GetVector().Y));
                                newSizeX = this.GetSizeX() - intersect.Width;
                            }
                            if (newSizeX == 0) throw new InvalidOperationException("NewSize is 0 !");
                            this.SetSizeX(newSizeX);
                            return false;
                        }
                    }

                default: return false;
            }
        }
    }

    enum AreaType
    {
        WALL,
        ROCK,
        WATER,
        GRASS,
        ICE,
        EAGLE          
    }

    static class AreaTypeHelper
    {
        private static Random random = new Random();

        public static AreaType GetRandom()
        {
            return (AreaType)random.Next(Enum.GetValues(typeof(AreaType)).Length-1);
        }
    }
}
