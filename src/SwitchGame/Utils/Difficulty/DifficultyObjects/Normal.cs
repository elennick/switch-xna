using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Normal : Difficulty
    {
        public string getName()
        {
            return "Normal";
        }

        public string getDescription()
        {
            return "Normal";
        }

        public int getNumberOfTilesToDropPerRound()
        {
            return 3;
        }

        public int getNumberOfTilesInTheGameboardWidth()
        {
            return 5;
        }

        public int getNumberOfTilesInTheGameboardHeight()
        {
            return 9;
        }

        public int getStartingSpeed()
        {
            return 800;
        }

        public int getMaxSpeed()
        {
            return 300;
        }

        public int getSizeOfTileSet()
        {
            return 4;
        }
    }
}
