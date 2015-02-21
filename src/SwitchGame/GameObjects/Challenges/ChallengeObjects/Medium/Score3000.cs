using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class Score3000 : Challenge
    {
        public string getName()
        {
            return "Mark it 3, Dude";
        }

        public string getDescription()
        {
            return "Score 3000 points! Use those cappers and multipliers well!\n" +
                   "Please the crowd with a super high score of over three-thousand!\n" +
                   "What? There is no crowd? Just pretend! Maybe your dog counts!\n";
        }

        public Difficulty getDifficulty()
        {
            return new Normal();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.score >= 3000)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.score + " / 3000\nPoints Scored";
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
