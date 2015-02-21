using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukes1 : Challenge
    {
        public string getName()
        {
            return "Radioactive";
        }

        public string getDescription()
        {
            return "Fire one nuke. It's just that easy. Save up 100 energy, press (B)\n" +
                   "and watch the mushroom cloud rise. Every Joe Shmoe and his\n" +
                   "dog gets access to nuclear weapons these days, eh?";
        }

        public Difficulty getDifficulty()
        {
            return new Easy();
        }

        public bool isCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFired >= 1)
            {
                return true;
            }

            return false;
        }

        public string getStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFired + " / 1\nNukes Fired";
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
