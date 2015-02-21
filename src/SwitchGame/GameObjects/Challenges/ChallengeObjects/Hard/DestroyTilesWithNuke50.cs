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
    class DestroyTilesWithNuke50 : Challenge
    {
        public string getName()
        {
            return "Duke Nuked";
        }

        public string getDescription()
        {
            return "Destroy 50 tiles using only the nuclear strike! You must remove\n" +
                   "50 tiles from the screen to pass this challenge but only tiles\n" +
                   "destroyed using the nuke will count. Hail to the king, baby!\n";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyedByNuke >= 50)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyedByNuke + " / 50\nTiles Destroyed";
        }

        public int isSpeedUpEnabled()
        {
            return 0;
        }

        public int startingPower()
        {
            return 50;
        }
    }
}

