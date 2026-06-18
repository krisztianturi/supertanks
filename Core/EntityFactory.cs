using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Entities;
using SuperTanks.Systems;
using System;
using System.Collections.Generic;


namespace SuperTanks.Core
{
    internal class EntityFactory
    {

        private static List<Func<Vector2>> _getTargetVectors = new();
        private EntityFactory() { }

        private static int _tileSize = 100;

        internal static ObjectArea CreateArea(Vector2 vector, AreaType areaType)
        {
            switch (areaType)
            {
                case AreaType.WALL: return new ObjectArea(Assets._wall, vector, _tileSize / 10, _tileSize / 10, true, true, AreaType.WALL);
                case AreaType.ROCK: return new ObjectArea(Assets._rock, vector, _tileSize / 10, _tileSize / 10, true, true, AreaType.ROCK);
                case AreaType.WATER: return new ObjectArea(Assets._water, vector, _tileSize / 10, _tileSize / 10, true, false, AreaType.WATER);
                case AreaType.GRASS: return new ObjectArea(Assets._grass, vector, _tileSize / 10, _tileSize / 10, false, true, AreaType.GRASS);
                case AreaType.ICE: return new ObjectArea(Assets._ice, vector, _tileSize / 10, _tileSize / 10, false, false, AreaType.ICE);
                case AreaType.EAGLE: 
                    {
                        ObjectArea area = new ObjectArea(Assets._eagle, vector, _tileSize, _tileSize, true, true, AreaType.EAGLE);
                        _getTargetVectors.Add(area.GetVector);
                        return area;
                    }
                default: return null;
            }
            
        }

        internal static Player CreatePlayer(Vector2 vector,PlayerInput input, bool ship, int power, int vitality)
        {
            Player player = new Player(Assets._up, Assets._down, Assets._left, Assets._right, vector, _tileSize, _tileSize, false, true,ship, power, vitality, new Animation(Assets._invulnerable), input);
            _getTargetVectors.Add(player.GetVector);
            return player;
        }

        internal static Projectile CreateProjectile(Vector2 vector, int sizeX, int sizeY, Direction direction, Team owner, int power, float coordinate)
        {
            return new Projectile(Assets._projectile, vector, sizeX, sizeY, false, true, direction, owner, power, coordinate, new Animation(Assets._explosion));
        }

        internal static Enemy CreateEnemy(Vector2 vector, int power, int vitality, MoveType moveType, SpeedType speedType)
        {
            if (moveType == MoveType.UntilBlock)
            {
                return new Enemy(Assets._up, Assets._down, Assets._left, Assets._right, vector, _tileSize, _tileSize, false, true, 2, 3, new Animation(Assets._invulnerable), speedType);
            }
            return new Enemy(Assets._up, Assets._down, Assets._left, Assets._right, vector, _tileSize, _tileSize, false, true, 2, 3, new Animation(Assets._invulnerable), _getTargetVectors, speedType);
        }

        internal static Boost CreateBoost(Vector2 vector, BoostType type)
        {
            Texture2D tex = null;
            switch (type)
            {
                case BoostType.Life: tex = Assets._boostLife; break;
                case BoostType.Star: tex = Assets._boostStar; break;
                case BoostType.Gun: tex= Assets._boostGun; break;
                case BoostType.Spade: tex= Assets._boostSpade; break;
                case BoostType.Ship: tex = Assets._boostShip; break;
            }
            return new Boost(tex, vector, _tileSize, _tileSize, false, true,type);
        }

        internal static void Clear()
        {
            _getTargetVectors.Clear();
        }
    }
}
