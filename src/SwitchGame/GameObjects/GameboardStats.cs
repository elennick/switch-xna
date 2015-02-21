using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.GameObjects
{
    class GameboardStats
    {
        public GameboardStats() : this(0) { }

        public GameboardStats(int startingPower)
        {
            power = startingPower;
            score = 0;
            level = 1;
            timeElapsed = 0;
            numberOfBlocksDestroyed = 0;
            numberOfBlocksDestroyedByLaser = 0;
            numberOfBlocksDestroyedByNuke = 0;
            numberOfBulletTimesFired = 0;
            numberOfLasersFired = 0;
            numberOfNukesFired = 0;
            totalPowerObtained = 0;
            numberOfCapsCompleted = 0;
            numberOfTilesDestroyedByCapping = 0;
            numberOf2xMulipliersCapped = 0;
            numberOf3xMulipliersCapped = 0;
            numberOf4xMulipliersCapped = 0;
            numberOfMultipliersCapped = 0;
            numberOfNukesFiredDuringActiveBulletTime = 0;
        }

        public int score { get; set; }
        public int power { get; set; }
        public int level { get; set; }
        public int timeElapsed { get; set; }
        public int numberOfNukesFired { get; set; }
        public int numberOfBulletTimesFired { get; set; }
        public int numberOfLasersFired { get; set; }
        public int numberOfBlocksDestroyed { get; set; }
        public int numberOfBlocksDestroyedByLaser { get; set; }
        public int numberOfBlocksDestroyedByNuke { get; set; }
        public int totalPowerObtained { get; set; }
        public int numberOfCapsCompleted { get; set; }
        public int numberOfTilesDestroyedByCapping { get; set; }
        public int numberOf2xMulipliersCapped { get; set; }
        public int numberOf3xMulipliersCapped { get; set; }
        public int numberOf4xMulipliersCapped { get; set; }
        public int numberOfMultipliersCapped { get; set; }
        public int numberOfNukesFiredDuringActiveBulletTime { get; set; }

        public List<StatValue> getValuesForStatsScreen()
        {
            List<StatValue> statValues = new List<StatValue>();

            statValues.Add(new StatValue("Lifetime Score", score));
            statValues.Add(new StatValue("Tiles Destroyed", numberOfBlocksDestroyed));
            statValues.Add(new StatValue("Sandwiches Completed", numberOfCapsCompleted));
            statValues.Add(new StatValue("Bullet Times Triggered", numberOfBulletTimesFired));
            statValues.Add(new StatValue("Lasers Fired", numberOfLasersFired));
            statValues.Add(new StatValue("Tiles Destroyed By Laser", numberOfBlocksDestroyedByLaser));
            statValues.Add(new StatValue("Nukes Fired", numberOfNukesFired));
            statValues.Add(new StatValue("Tiles Destroyed By Nuke", numberOfBlocksDestroyedByNuke));

            return statValues;
        }

        public void addStats(GameboardStats statsToAdd)
        {
            score += statsToAdd.score;
            power += statsToAdd.power;
            level += statsToAdd.level;
            timeElapsed += statsToAdd.timeElapsed;
            numberOfNukesFired += statsToAdd.numberOfNukesFired;
            numberOfBulletTimesFired += statsToAdd.numberOfBulletTimesFired;
            numberOfLasersFired += statsToAdd.numberOfLasersFired;
            numberOfBlocksDestroyed += statsToAdd.numberOfBlocksDestroyed;
            numberOfBlocksDestroyedByLaser += statsToAdd.numberOfBlocksDestroyedByLaser;
            numberOfBlocksDestroyedByNuke += statsToAdd.numberOfBlocksDestroyedByNuke;
            totalPowerObtained += statsToAdd.totalPowerObtained;
            numberOfCapsCompleted += statsToAdd.numberOfCapsCompleted;
            numberOfTilesDestroyedByCapping += statsToAdd.numberOfTilesDestroyedByCapping;
            numberOf2xMulipliersCapped += statsToAdd.numberOf2xMulipliersCapped;
            numberOf3xMulipliersCapped += statsToAdd.numberOf3xMulipliersCapped;
            numberOf4xMulipliersCapped += statsToAdd.numberOf4xMulipliersCapped;
            numberOfMultipliersCapped += statsToAdd.numberOfMultipliersCapped;
            numberOfNukesFiredDuringActiveBulletTime += statsToAdd.numberOfNukesFiredDuringActiveBulletTime;
        }

        public class StatValue
        {
            public String name { get; set; }
            public int value { get; set; }

            public StatValue(String name, int value)
            {
                this.name = name;
                this.value = value;
            }
        }
    }
}
