using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Impossible : Difficulty
    {
        public string getName()
        {
            return "Impossible";
        }

        public string getDescription()
        {
            return "Impossible";
        }

        public int getNumberOfTilesToDropPerRound()
        {
            return 5;
        }

        public int getNumberOfTilesInTheGameboardWidth()
        {
            return 7;
        }

        public int getNumberOfTilesInTheGameboardHeight()
        {
            return 13;
        }

        public int getStartingSpeed()
        {
            return 250;
        }

        public int getMaxSpeed()
        {
            return 50;
        }

        public int getSizeOfTileSet()
        {
            return 6;
        }
    }
}
