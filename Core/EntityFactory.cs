using Microsoft.Xna.Framework;
using SuperTanks.Entities;
using SuperTanks.Systems;


namespace SuperTanks.Core
{
    internal class EntityFactory
    {
        private EntityFactory() { }

        private static int _tileSize = 100;

        internal static ObjectArea CreateArea(Vector2 vector, AreaType areaType)
        {
            switch (areaType)
            {
                case AreaType.WALL: return new ObjectArea(Assets._wall, vector, _tileSize / 10, _tileSize / 10, true, true, AreaType.WALL);
                case AreaType.ROCK: return new ObjectArea(Assets._rock, vector, _tileSize / 10, _tileSize / 10, true, false, AreaType.ROCK);
                case AreaType.EAGLE: return new ObjectArea(Assets._eagle, vector, _tileSize, _tileSize, true, true, AreaType.EAGLE);
                default: return null;
            }
            
        }

        internal static Player CreatePlayer(Vector2 vector)
        {
            return new Player(Assets.Test, vector, _tileSize, _tileSize, false, true,6,1);
        }

        internal static Projectile CreateProjectile(Vector2 vector, int sizeX, int sizeY, Direction direction, Team owner, int damage)
        {
            return new Projectile(Assets._projectile, vector, sizeX, sizeY, false, false, direction, owner,damage);
        }

    }
}
