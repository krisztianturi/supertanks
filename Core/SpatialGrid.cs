using Microsoft.Xna.Framework;
using SuperTanks.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Core
{
    internal class SpatialGrid
    {
        private readonly int _cellSize;
        private readonly Dictionary<Point, List<GameObject>> _grid;
        private static Vector2 _origin;

        public SpatialGrid(int cellSize)
        {
            _cellSize = cellSize;
            _grid = new Dictionary<Point, List<GameObject>>();
        }

        internal static void SetOrigin(float x, float y) { _origin = new Vector2(x, y); }

        private List<Point> GetCells(Rectangle bounds)
        {
            List<Point> cells = new List<Point>();

            int minX = (int)(bounds.Left - _origin.X) / _cellSize;
            int maxX = (int)(bounds.Right - 1 - _origin.X) / _cellSize;

            int minY = (int)(bounds.Top - _origin.Y) / _cellSize;
            int maxY = (int)(bounds.Bottom - 1 - _origin.Y) / _cellSize;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    cells.Add(new Point(x, y));
                }
            }

            return cells;
        }

        public void Add(GameObject obj)
        {
            var cells = GetCells(obj.Bounds(obj.GetVector()));

            obj.SetOccupiedCells(cells);

            foreach (var cell in cells)
            {
                if (!_grid.TryGetValue(cell, out var list))
                {
                    list = new List<GameObject>();
                    _grid[cell] = list;
                }

                list.Add(obj);
            }

        }

        public void Update(GameObject obj)
        {
            var newCells = GetCells(obj.Bounds(obj.GetVector()));

            foreach (var cell in obj.OccupiedCells)
            {
                if (_grid.TryGetValue(cell, out var list))
                {
                    list.Remove(obj);
                }
            }

            foreach (var cell in newCells)
            {
                if (!_grid.TryGetValue(cell, out var list))
                {
                    list = new List<GameObject>();
                    _grid[cell] = list;
                }

                list.Add(obj);
            }

            obj.SetOccupiedCells(newCells);
        }

        public void Remove(GameObject obj)
        {
            foreach (var cell in obj.OccupiedCells)
            {
                if (_grid.TryGetValue(cell, out var list))
                {
                    list.Remove(obj);
                }
            }
        }

        public List<GameObject> GetNearby(Rectangle bounds)
        {
            HashSet<GameObject> result = new HashSet<GameObject>();
            var cells = GetCells(bounds);

            foreach (var cell in cells)
            {
                if (_grid.TryGetValue(cell, out var list))
                {
                    foreach (var obj in list)
                    {
                        result.Add(obj);
                    }
                }
            }

            return new List<GameObject>(result);
        }
    }
}
