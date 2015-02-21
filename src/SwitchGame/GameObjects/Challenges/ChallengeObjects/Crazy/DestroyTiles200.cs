using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class DestroyTiles200 : Challenge
    {
        public string getName()
        {
            return "Tile Time";
        }

        public string getDescription()
        {
            return "Destroy 200 tiles. It's a tile genocide and you're responsible. You\n" +
                   "should be ashamed :(";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyed >= 200)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyed + " / 200\nTiles Destroyed";
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
