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
    class SurviveLevels5 : Challenge
    {
        public string getName()
        {
            return "Surival Of The Fittest";
        }

        public string getDescription()
        {
            return "Survive for 5 levels! As the clock counts down, your level goes\n" +
                   "up and the tiles start coming down faster! Can you handle the\n" +
                   "pressure that comes with colorful, smiley blocks moving down\n" +
                   "the screen slightly faster than normal? We'll soon find out...";
        }

        public Difficulty getDifficulty()
        {
            return new Easy();
        }

        public bool isCompleted(GameboardStats stats)
        {
            return (stats.level >= 5);
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.level + " / 5\nLevels Survived";
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
