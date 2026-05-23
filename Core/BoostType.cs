using System;


namespace SuperTanks.Core
{
    internal enum BoostType
    {
        Life,
        Star,
        Gun,
        Spade,
        Ship

    }

    static class BoostTypeHelper
    {
        private static Random random = new Random();

        public static BoostType GetRandom()
        {
            return (BoostType)random.Next(Enum.GetValues(typeof(BoostType)).Length);
        }
    }
}
