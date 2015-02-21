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
    class CapMultipliers : Challenge
    {
        public string getName()
        {
            return "Multiplicity";
        }

        public string getDescription()
        {
            return "Cap any five multipliers. When you cap a set of tiles, there\n" +
                   "must be at least one multiplier in between them (2x, 3x or 4x).\n" +
                   "Do it five times! DO IT!";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            return (stats.numberOfMultipliersCapped >= 5);
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfMultipliersCapped + " / 5\nMultipliers Capped";
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
