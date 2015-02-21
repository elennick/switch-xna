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
    class SurviveMinutes10 : Challenge
    {
        public string getName()
        {
            return "Time Is Of The Essence";
        }

        public string getDescription()
        {
            return "Survive for ten minutes without losing the game. An ultimate\n" +
                   "test of endurance awaits...";
        }

        public Difficulty getDifficulty()
        {
            return new Impossible();
        }

        public bool isCompleted(GameboardStats stats)
        {
            return (stats.timeElapsed >= (60000 * 10));
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + (stats.timeElapsed / (1000 * 60))+ " / 10\nMinutes Survived";
        }

        public int isSpeedUpEnabled()
        {
            return 60000;
        }

        public int startingPower()
        {
            return 100;
        }
    }
}
