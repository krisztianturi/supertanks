using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Core
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    static class DirectionHelper
    {
        private static Random random = new Random();

        public static Direction GetRandom()
        {
            return (Direction)random.Next(Enum.GetValues(typeof(Direction)).Length);
        }
    }
}
