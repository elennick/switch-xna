using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Hard : Difficulty
    {
        public string getName()
        {
            return "Hard";
        }

        public string getDescription()
        {
            return "Hard";
        }

        public int getNumberOfTilesToDropPerRound()
        {
            return 4;
        }

        public int getNumberOfTilesInTheGameboardWidth()
        {
            return 6;
        }

        public int getNumberOfTilesInTheGameboardHeight()
        {
            return 11;
        }

        public int getStartingSpeed()
        {
            return 450;
        }

        public int getMaxSpeed()
        {
            return 200;
        }

        public int getSizeOfTileSet()
        {
            return 6;
        }
    }
}
