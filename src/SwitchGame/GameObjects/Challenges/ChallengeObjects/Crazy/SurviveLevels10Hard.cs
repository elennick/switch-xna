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
    class SurviveLevels10Hard : Challenge
    {
        public string getName()
        {
            return "I Will Survive";
        }

        public string getDescription()
        {
            return "Survive for ten levels of time without losing the game. It\n" +
                   "doesn't matter how you stay alive, but you must. Show the\n" +
                   "crowd how you can hold up during a marathon of puzzling!";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
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
            return 15000;
        }

        public int startingPower()
        {
            return 50;
        }
    }
}
