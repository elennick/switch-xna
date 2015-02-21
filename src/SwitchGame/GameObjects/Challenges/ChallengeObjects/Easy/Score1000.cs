using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class Score1000 : Challenge
    {
        public string getName()
        {
            return "High Scoring";
        }

        public string getDescription()
        {
            return "Score 1000 points! Each tile destroyed is worth 25 points.\n" +
                   "Match tiles for small time points. Cap tiles in between cappers\n" +
                   "for slightly more points. Throw a multiplier in there for a lot\n" +
                   "of points!";
        }

        public Difficulty getDifficulty()
        {
            return new Easy();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.score >= 1000)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.score + " / 1000\nPoints Scored";
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
