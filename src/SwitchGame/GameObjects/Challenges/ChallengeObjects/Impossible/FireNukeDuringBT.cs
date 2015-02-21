using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukeDuringBT : Challenge
    {
        public string getName()
        {
            return "Combo Platter";
        }

        public string getDescription()
        {
            return "Fire a nuke while bullet time is active. You will have fifteen\n" +
                "seconds after triggering bullet time with which to obtain another\n" +
                "25 energy and fire off the nuclear strike. Those tiles will never\n" +
                "see it coming! Or maybe they will.";
        }

        public Difficulty getDifficulty()
        {
            return new Hard();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFiredDuringActiveBulletTime >= 1)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFiredDuringActiveBulletTime + " / 1\nNukes Fired";
        }

        public int isSpeedUpEnabled()
        {
            return 0;
        }

        public int startingPower()
        {
            return 100;
        }
    }
}
