using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class Score5000 : Challenge
    {
        public string getName()
        {
            return "Score Attack";
        }

        public string getDescription()
        {
            return "Score 5000 points! I don't have any other funny things to say!";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.score >= 5000)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.score + " / 5000\nPoints Scored";
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
