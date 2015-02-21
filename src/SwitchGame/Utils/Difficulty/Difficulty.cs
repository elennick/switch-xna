using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty
{
    public interface Difficulty
    {
        String getName();
        String getDescription();
        int getNumberOfTilesToDropPerRound(); //how many tiles to drop at a time
        int getNumberOfTilesInTheGameboardWidth(); //number of tiles across
        int getNumberOfTilesInTheGameboardHeight(); //number of tiles down
        int getStartingSpeed(); //in millis
        int getMaxSpeed(); //in millis, lower is faster so higher numbers are EASIER
        int getSizeOfTileSet(); //how many basic tile types to use, more is HARDER
    }
}
