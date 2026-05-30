using Microsoft.Xna.Framework;
using SuperTanks.Entities;
using SuperTanks.Overlays;
using SuperTanks.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Core
{
    internal class GameCreator
    {

        private static readonly Lazy<GameCreator> _instance = new Lazy<GameCreator>(() => new GameCreator());

        internal static GameCreator Instance => _instance.Value;

        private static int _screenX, _screenY;
        private static int _countX, _countY;
        public static int _restX, _restY;
        private static int _drawSizeX, _drawSizeY;

        public static readonly int _tileSize = 100;
        public static readonly int _edgeSize = 50;
        public static readonly int _displaySize = 150;

        private List<GameObject> gameObjects;
        private SpatialGrid _spatialGrid;
        private Player _player1;
        private Rectangle _eagleRect;
        private Rectangle[] _enemyRects = new Rectangle[3];

        internal Player GetPlayer1() { return _player1; }
        

        static GameCreator()
        {
            _screenX = Engine.GetScreenWidth();
            _screenY = Engine.GetScreenHeight();

            _screenX -= _displaySize;

            _countX = _screenX / _tileSize;
            _countY = _screenY / _tileSize;

            _drawSizeX = _countX * _tileSize;
            _drawSizeY = _countY * _tileSize;


            _restX = _screenX - _drawSizeX;
            _restY = _screenY - _drawSizeY;

            if (_restX % 2 != 0) _restX--;
            if (_restY % 2 != 0) _restY--;

            GameManager.SetEdges(_edgeSize, _drawSizeX - _edgeSize, _edgeSize, _drawSizeY - _edgeSize);
            SinglePlayerOverlay.SetEdgeSize(_edgeSize, _drawSizeX, _drawSizeY);
            Renderer.Offset = new Vector2(_restX / 2, _restY / 2);
            SpatialGrid.SetOrigin(_edgeSize, _edgeSize);
        }

        internal GameCreator() 
        {
        }



        internal GameManager CreateSinglePlayerGame()
        {
            gameObjects = new List<GameObject>();
            _spatialGrid = new SpatialGrid(_tileSize);
            GenerateSinglePlayerMap();
            GameManager gameManager = new GameManager(_spatialGrid,_eagleRect, _enemyRects);
            gameManager.AddObject(_player1);
            _spatialGrid.Add(_player1);
            gameManager.AddObjects(gameObjects);
            return gameManager;
            
        }

        private void CreateTile(int pixelX, int pixelY, AreaType type)
        {
            int objectSize = _tileSize / 10;
            int originPixelX = pixelX;
            for (int i = 0; i < objectSize; i++)
            {
                for (int j = 0; j < objectSize; j++)
                {
                    GameObject obj = EntityFactory.CreateArea(new Vector2(pixelX, pixelY), type);
                    gameObjects.Add(obj);
                    _spatialGrid.Add(obj);
                    pixelX += objectSize;
                }
                pixelX = originPixelX;
                pixelY += objectSize;
            }
        }

        private void GenerateSinglePlayerMap()
        {
            Random random = new Random();
            for (int x = 0; x < _countX-1; x++)
            {
                for (int y = 0; y < _countY-1; y++)
                {
                    int pixelX =_edgeSize + x * _tileSize;
                    int pixelY = _edgeSize + y * _tileSize;

                    if (y==0)
                    {
                        continue;
                    }

                    if ((x == 0 && y == 1) || (x == _countX - 2 && y == 1))
                    {
                        CreateTile(pixelX, pixelY, AreaType.ROCK);
                        continue;

                    }
                    if (y==1)
                    {
                        continue;
                    }

                    if (y == 2)
                    {
                        if (x == (_countX - 1) / 2 - 1 || x == (_countX - 1) / 2 + 1 || x == (_countX - 1) / 2)
                        {
                            CreateTile(pixelX, pixelY, AreaType.ROCK);
                        }
                        else CreateTile(pixelX, pixelY, AreaType.WALL);
                        continue;
                    }

                    if ((x == (_countX - 1) / 2 - 1 && y == _countY - 2) || (x == (_countX - 1) / 2 + 1 && y == _countY - 2) ||
                        (x == (_countX - 1) / 2 - 1 && y == _countY - 3) || (x == (_countX - 1) / 2 + 1 && y == _countY - 3) || (x == (_countX - 1) / 2 && y == _countY - 3)
                        )
                    {
                        CreateTile(pixelX, pixelY, AreaType.WALL);
                        continue;
                    }

                    if (x == (_countX - 1) / 2 -2 && y == _countY - 2)
                    {
                        _player1 = EntityFactory.CreatePlayer(new Vector2(pixelX,pixelY));
                        pixelX += _player1.GetSizeX();
                        pixelY += _player1.GetSizeY();
                        continue;
                    }

                    if (x == (_countX - 1) / 2 && y == _countY - 2)
                    {
                        GameObject obj = EntityFactory.CreateArea(new Vector2(pixelX, pixelY), AreaType.EAGLE);
                        gameObjects.Add(obj);
                        _spatialGrid.Add(obj);
                        pixelX += obj.GetSizeX();
                        pixelY += obj.GetSizeY();
                        _eagleRect = obj.Bounds(obj.GetVector());
                        continue;
                    }

                    double randomNum = random.NextDouble();
                    if (randomNum < 0.3)
                    {
                        CreateTile(pixelX, pixelY, EnumRandomizer.GetRandomExcept<AreaType>(AreaType.EAGLE));
                    }
                }
            }

            GenerateEnemy(_edgeSize, _edgeSize, 0);
            GenerateEnemy(_edgeSize+ (((_countX - 1) / 2)*_tileSize), _edgeSize, 1);
            GenerateEnemy(_edgeSize + ((_countX - 2) * _tileSize), _edgeSize, 2);
        }

        private void GenerateEnemy(float pixelX, float pixelY, int number)
        {
            Enemy enemy = EntityFactory.CreateEnemy(new Vector2(pixelX, pixelY), 1, 1, EnumRandomizer.GetRandom<MoveType>(), EnumRandomizer.GetRandom<SpeedType>());
            //Enemy enemy = EntityFactory.CreateEnemy(new Vector2(pixelX, pixelY), 1, 1, MoveType.Follower, SpeedType.Various);
            gameObjects.Add(enemy);
            _spatialGrid.Add(enemy);
            _enemyRects[number] = enemy.Bounds(enemy.GetVector());
        }

    }
}
