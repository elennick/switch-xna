using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Easy : Difficulty
    {
        public string getName()
        {
            return "Easy";
        }

        public string getDescription()
        {
            return "Easy";
        }

        public int getNumberOfTilesToDropPerRound()
        {
            return 2;
        }

        public int getNumberOfTilesInTheGameboardWidth()
        {
            return 4;
        }

        public int getNumberOfTilesInTheGameboardHeight()
        {
            return 8;
        }

        public int getStartingSpeed()
        {
            return 1200;
        }

        public int getMaxSpeed()
        {
            return 1000;
        }

        public int getSizeOfTileSet()
        {
            return 3;
        }
    }
}
