using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukes15 : Challenge
    {
        public string getName()
        {
            return "Hell-Bringer";
        }

        public string getDescription()
        {
            return "Fire fifteen nuclear strikes. Save up 100 energy and let that\n" +
                   "sucker rip! This is your final and most difficult challenge...\n" +
                   "show how you have mastered the most powerful weapon in\n" +
                   "your arsenal!";
        }

        public Difficulty getDifficulty()
        {
            return new Impossible();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFired >= 15)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFired + " / 15\nNukes Fired";
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
