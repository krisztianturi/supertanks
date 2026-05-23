using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Core;
using SuperTanks.Systems;
using System;
using System.Collections.Generic;


namespace SuperTanks.Entities
{
    internal abstract class GameObject
    {
        private Texture2D _currentImg;
        private Vector2 _vector;
        private bool _blocking;
        private bool _shootable;
        private int _sizeX;
        private int _sizeY;


        private List<Point> _occupiedCells = new();
        internal IReadOnlyList<Point> OccupiedCells => _occupiedCells;

        internal void SetOccupiedCells(List<Point> cells)
        {
            _occupiedCells = cells;
        }

        internal Rectangle Bounds(Vector2 vector)
        {
            return new Rectangle((int)vector.X, (int)vector.Y, _sizeX, _sizeY);
        }

        internal String toString()
        {
            return "X: "+_vector.X+" Y: "+_vector.Y+ " SizeX: "+_sizeX+ " SizeY: "+_sizeY;
        }


        protected GameObject(Texture2D currentImg, Vector2 vector, int sizeX,int sizeY, bool blocking, bool shootable)
        {
            _currentImg = currentImg;
            _vector = vector;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _blocking = blocking;
            _shootable = shootable;
        }

        protected GameObject( Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable)
        {
            _vector = vector;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _blocking = blocking;
            _shootable = shootable;
        }

        internal abstract void Update(GameTime gameTime, GameManager gm);
        internal abstract void Draw(Renderer renderer);

        internal Vector2 GetVector() { return _vector; }
        internal void SetVector(Vector2 vector) { _vector = vector; }
        internal bool IsBlocking() { return _blocking; }

        internal bool IsShootable() { return _shootable; }
        internal void SetShootable(bool value) { _shootable = value; }

        internal int GetSizeX() { return _sizeX; }
        internal int GetSizeY() { return _sizeY; }

        internal void SetSizeX(int sizeX) {  _sizeX = sizeX; }
        internal void SetSizeY(int sizeY) {  _sizeY = sizeY; }


        protected Texture2D GetCurrentImg() { return _currentImg; }
        protected void SetCurrentImg(Texture2D tex) { _currentImg = tex; }
    }
}
