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
    class DestroyTiles500 : Challenge
    {
        public string getName()
        {
            return "Destroy Tiles";
        }

        public string getDescription()
        {
            return "Destroy 500 tiles. You may use any method to do so including\n" + 
                   "matching, capping, lasering and nuking.";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyed >= 500)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyed + " / 500\nTiles Destroyed";
        }

        public int isSpeedUpEnabled()
        {
            return 0;
        }

        public int startingPower()
        {
            return 100;
        }
    }
}
