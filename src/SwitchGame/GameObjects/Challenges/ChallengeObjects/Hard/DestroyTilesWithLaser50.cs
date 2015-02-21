using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.GameObjects.Challenges;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class DestroyTilesWithLaser50 : Challenge
    {
        public string getName()
        {
            return "Gone In A Flash";
        }

        public string getDescription()
        {
            return "Destroy 50 tiles... using the laser! You must demolish a total\n" +
                   "of fifty tiles but only ones removed by using the laser weapon\n" +
                   "will count towards the total! Demonstrate your mastery of this\n" +
                   "precision tool by pressing (Y)!\n";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyedByLaser >= 50)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyedByLaser + " / 50\nTiles Destroyed";
        }

        public int isSpeedUpEnabled()
        {
            return 0;
        }

        public int startingPower()
        {
            return 25;
        }
    }
}

