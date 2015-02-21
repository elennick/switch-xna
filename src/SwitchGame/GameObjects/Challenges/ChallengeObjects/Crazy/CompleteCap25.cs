using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class CompleteCap25 : Challenge
    {
        public string getName()
        {
            return "Cap It";
        }

        public string getDescription()
        {
            return "Complete twenty-five caps. All you have to do is drop a bottom\n" +
                   "cap anywhere on the board and then place a top cap anywhere\n" +
                   "above it. It can be directly above, or five tiles above. Either\n" +
                   "way works!";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfCapsCompleted >= 25)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfCapsCompleted + " / 25\nCaps Completed";
        }

        public int isSpeedUpEnabled()
        {
            return 30000;
        }

        public int startingPower()
        {
            return 25;
        }
    }
}
