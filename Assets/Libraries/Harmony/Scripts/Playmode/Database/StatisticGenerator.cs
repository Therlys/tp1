using System;
using Game;

namespace Harmony
{
    public class StatisticGenerator
    {
        private Statistic statistic = null;
        private int worldSeed = 0;
        long timeStampInLong = 0;
        private int bunnyQuantity = 0;
        private int foxQuantity = 0;
        private int bunnyHungerDeathQuantity = 0;
        private int bunnyThirstDeathQuantity = 0;
        private int bunnyEatenDeathQuantity = 0;
        private int foxHungerDeathQuantity = 0;
        private int foxThirstDeathQuantity = 0;
        private int bunnyDeathQuantity = 0;
        private int foxDeathQuantity = 0;

        public void BunnySpawn()
        {
            bunnyQuantity += 1;
        }

        public void FoxSpawn()
        {
            foxQuantity += 1;
        }

        public void BunnyHungerDeath()
        {
            bunnyHungerDeathQuantity += 1;
            bunnyDeathQuantity += 1;
            bunnyQuantity -= 1;
        }

        public void BunnyThirstDeath()
        {
            bunnyThirstDeathQuantity += 1;
            bunnyDeathQuantity += 1;
            bunnyQuantity -= 1;
        }

        public void BunnyEatenDeath()
        {
            bunnyEatenDeathQuantity += 1;
            bunnyDeathQuantity += 1;
            bunnyQuantity -= 1;
        }

        public void FoxHungerDeath()
        {
            foxHungerDeathQuantity += 1;
            foxDeathQuantity += 1;
            foxQuantity -= 1;
        }

        public void FoxThirstDeath()
        {
            foxThirstDeathQuantity += 1;
            foxDeathQuantity += 1;
            foxQuantity -= 1;
        }

        public void SetWorldSeed(int seed)
        {
            worldSeed = seed;
        }

        public void AddStatisticToDatabase()
        {
            statistic = new Statistic();
            String timeStamp = GetTimestamp(DateTime.Now);
            long.TryParse(timeStamp, out timeStampInLong);
            statistic.Timestamp = timeStampInLong;
            statistic.Seed = worldSeed;
            statistic.BunnyQuantity = bunnyQuantity;
            statistic.FoxQuantity = foxQuantity;
            statistic.BunnyDeathQuantity = bunnyDeathQuantity;
            statistic.FoxDeathQuantity = foxDeathQuantity;
            statistic.BunnyHungerDeathQuantity = bunnyHungerDeathQuantity;
            statistic.BunnyThirstDeathQuantity = bunnyThirstDeathQuantity;
            statistic.BunnyEatenDeathQuantity = bunnyEatenDeathQuantity;
            statistic.FoxHungerDeathQuantity = foxHungerDeathQuantity;
            statistic.FoxThirstDeathQuantity = foxThirstDeathQuantity;
            statistic = Finder.GetStatisticRepository().create(statistic);
        }
        
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

    }
}