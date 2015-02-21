using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukes3 : Challenge
    {
        public string getName()
        {
            return "Trifecta Of Death";
        }

        public string getDescription()
        {
            return "Fire three nuclear strikes. Save up 100 energy and let that\n" +
                   "sucker rip! Do it 3 times and you will pass this challenge\n" +
                   "of epic destruction! ";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFired >= 3)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFired + " / 3\nNukes Fired";
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
