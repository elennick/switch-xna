using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class CapTiles20 : Challenge
    {
        public string getName()
        {
            return "Caught In The Middle";
        }

        public string getDescription()
        {
            return "Destroy fifteen tiles by capping them. You must remove 15\n" +
                   "tiles from the screen but only ones destroyed by being\n" +
                   "sandwiched will count towards this total! Mmmm... sandwich.";
        }

        public Difficulty getDifficulty()
        {
            return new Normal();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfTilesDestroyedByCapping >= 15)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfTilesDestroyedByCapping + " / 15\nTiles Destroyed";
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
