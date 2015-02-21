using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukes7 : Challenge
    {
        public string getName()
        {
            return "Nuclear Winter";
        }

        public string getDescription()
        {
            return "Fire seven nuclear strikes. Save up 100 energy and let that\n" +
                   "sucker rip! Do it 7 times and you will pass this challenge!\n";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFired >= 7)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFired + " / 7\nNukes Fired";
        }

        public int isSpeedUpEnabled()
        {
            return 30000;
        }

        public int startingPower()
        {
            return 25;
        }
    }
}
