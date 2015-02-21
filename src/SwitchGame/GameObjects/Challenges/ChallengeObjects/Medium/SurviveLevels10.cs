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
    class SurviveLevels10 : Challenge
    {
        public string getName()
        {
            return "Still Alive";
        }

        public string getDescription()
        {
            return "Survive for 10 levels worth of time without losing the game. Use\n" +
                   "all the tools at your disposal to survive until the time limit is\n" +
                   "up! IT'S SO VERY INTENSE AHHHHHH!!!";
        }

        public Difficulty getDifficulty()
        {
            return new Normal();
        }

        public bool isCompleted(GameboardStats stats)
        {
            return (stats.level >= 10);
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.level + " / 10\nLevels Survived";
        }

        public int isSpeedUpEnabled()
        {
            return 10000;
        }

        public int startingPower()
        {
            return 25;
        }
    }
}
