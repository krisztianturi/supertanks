using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Systems
{
    internal static class EnumRandomizer
    {
        private static readonly Random _random = new();

        public static T GetRandom<T>() where T : struct, Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            return values[_random.Next(values.Length)];
        }

        public static T GetRandomExcept<T>(params T[] excluded) where T : struct, Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            List<T> filtered = new List<T>();

            foreach (var v in values)
            {
                bool isExcluded = false;

                foreach (var ex in excluded)
                {
                    if (EqualityComparer<T>.Default.Equals(v, ex))
                    {
                        isExcluded = true;
                        break;
                    }
                }

                if (!isExcluded)
                {
                    filtered.Add(v);
                }
            }

            if (filtered.Count == 0)
                throw new InvalidOperationException("No enum values left after exclusion.");

            return filtered[_random.Next(filtered.Count)];
        }


    }
}
