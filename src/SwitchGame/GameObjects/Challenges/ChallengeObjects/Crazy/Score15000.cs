using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class Score15000 : Challenge
    {
        public string getName()
        {
            return "Score!";
        }

        public string getDescription()
        {
            return "Score 15000 points! Show the crowd what you're made of!";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.score >= 15000)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.score + " / 15000\nPoints Scored";
        }

        public int isSpeedUpEnabled()
        {
            return 60000;
        }

        public int startingPower()
        {
            return 25;
        }
    }
}
