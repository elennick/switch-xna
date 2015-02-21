using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireLasers2 : Challenge
    {
        public string getName()
        {
            return "Fire Da Laser!";
        }

        public string getDescription()
        {
            return "Fire the laser twice! Simply save up 50 energy and fire by\n" +
                   "pressing (Y). Two times the lasering, two times the fun! That\n" +
                   "joke was really lame!";
        }

        public Difficulty getDifficulty()
        {
            return new Easy();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfLasersFired >= 2)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfLasersFired + " / 2\nLasers Fired";
        }

        public int isSpeedUpEnabled()
        {
            return 0;
        }

        public int startingPower()
        {
            return 0;
        }
    }
}
