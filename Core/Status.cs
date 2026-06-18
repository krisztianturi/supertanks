
namespace SuperTanks.Core
{
    internal static class Status
    {
        private static int power1, power2;
        private static int vitality1=3, vitality2=3;
        private static bool ship1, ship2;

        internal static void SetPlayer1(int power, int life, bool ship)
        {
            power1 = power;
            vitality1 = life;
            ship1 = ship;
        }

        internal static void SetPlayer2(int power, int life, bool ship)
        {
            power2 = power;
            vitality2 = life;
            ship2 = ship;
        }

        internal static int GetPower1() { return power1; }
        internal static int GetPower2() { return power2; }
        internal static int GetVitality1() { return vitality1; }
        internal static int GetVitality2() { return vitality2; }
        internal static bool GetShip1() { return ship1; }
        internal static bool GetShip2() { return ship2; }

        internal static void Reset()
        {
            power1 = power2 = 0;
            vitality1 = vitality2 = 3;
            ship1 = ship2 = false;
        }




    }
}
