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
    class DestroyTiles100 : Challenge
    {
        public string getName()
        {
            return "Annihilation";
        }

        public string getDescription()
        {
            return "Destroy 100 tiles. Explode them. Melt them. Eat them. Destroy\n" +
                   "them by simply loving them too much. Just get it done.\n";
        }

        public Difficulty getDifficulty()
        {
            return new Normal();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyed >= 100)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyed + " / 100\nTiles Destroyed";
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
