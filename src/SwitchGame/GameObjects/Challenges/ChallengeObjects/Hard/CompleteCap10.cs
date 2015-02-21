using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class CompleteCap10 : Challenge
    {
        public string getName()
        {
            return "Cap'ten Kirk";
        }

        public string getDescription()
        {
            return "Complete ten caps. All you have to do is drop a bottom cap\n" +
                   "anywhere on the board and then place a top cap anywhere\n" +
                   "above it. It can be directly above, or 5 tiles above. Either way\n" +
                   "works! Full speed ahead Mister Sulu!";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfCapsCompleted >= 10)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfCapsCompleted + " / 10\nCaps Completed";
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
