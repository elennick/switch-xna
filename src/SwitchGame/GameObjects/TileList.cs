using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Tiles;
using Switch.GameObjects;
using Switch.GameObjects.Sound;
using Switch;

namespace Switch.GameObjects
{
    class TileList
    {
        private List<List<Tile>> tiles;
        private List<Tile> previewTiles;
        private int tileGridHeight;
        private int tileGridWidth;
        private bool previewRowEnabled;
        private GameboardStats stats;

        /**
         * Creates a new two dimensional List. The outmost List represents the tile
         * game boards width and the Lists contained inside represent each column of
         * Tile objects. This makes it easy to visualize and work with the game board
         * as a collection of columns, rather than tons of individual tiles.
         */
        public TileList(int tileGridWidth, int tileGridHeight, GameboardStats stats)
        {
            this.tileGridWidth = tileGridWidth;
            this.tileGridHeight = tileGridHeight;
            this.previewRowEnabled = true;
            this.stats = stats;

            tiles = new List<List<Tile>>();
            for (int i = 0; i < tileGridWidth; i++)
            {
                List<Tile> columns = new List<Tile>();
                tiles.Add(columns);
            }

            previewTiles = new List<Tile>();
        }

        /**
         * Turn the preview row on or off.
         */
        public void setPreviewRowEnabled(bool enabled)
        {
            this.previewRowEnabled = enabled;
        }

        /**
         * Takes the two dimensional List and turns it into a one-dimensional List of
         * Tile objects containing every tile on the board regardless of state or column
         * location.
         */
        public List<Tile> getTilesAsList()
        {
            List<Tile> allTiles = new List<Tile>();

            for (int i = 0; i < tiles.Count; i++)
            {
                foreach (Tile tile in tiles[i])
                {
                    allTiles.Add(tile);
                }
            }

            return allTiles;
        }

        /**
         * Returns all the tiles in one of the columns.
         */
        public List<Tile> getTilesInColumn(int columnIndex)
        {
            return tiles[columnIndex];
        }

        /**
         * Returns all the tiles in one of the columns that are currently seated and not dropping.
         */
        public int getSeatedColumnHeight(int columnIndex)
        {
            List<Tile> column = tiles[columnIndex];
            int tilesSeated = 0;

            foreach (Tile tile in column)
            {
                if (tile.isSeated())
                {
                    tilesSeated++;
                }
            }

            return tilesSeated;
        }

        /**
         * Add a tile to the TileList. Must specify which column it is getting inserted into.
         */
        public void Add(Tile tile, int columnIndex)
        {
            tiles[columnIndex].Add(tile);
        }

        /**
         * Remove a tile from the TileList. Must specify which column it is getting removed from.
         */
        public void Remove(Tile tile, int columnIndex)
        {
            tiles[columnIndex].Remove(tile);
        }

        /**
         * An overload for the Remove() function that is less efficient but doesn't require 
         * the column be passed in.
         */
        public void Remove(Tile tile)
        {
            foreach (Tile thisTile in getTilesAsList())
            {
                if (thisTile.Equals(tile))
                {
                    this.Remove(tile, tile.getGridX());
                }
            }
        }

        /**
         * Checks to see if any tiles are still floating or if they've all become seated.
         */
        public bool areAllTilesSeated()
        {
            foreach (Tile tile in getTilesAsList())
            {
                if (!tile.isSeated())
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Checks to see if any tiles are still in the middle of an animation or not.
         */
        public bool areAllTilesDoneAnimating()
        {
            foreach (Tile tile in getTilesAsList())
            {
                if (tile.isAnimating())
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Checks to see if any tiles will be dropped based on the current game time. Same the
         * dropTilesOlderThanAge() function but does not drop any tiles.
         */
        public bool willTilesGetDropped(int ageThreshold)
        {
            bool tilesWillGetDropped = false;
            foreach (Tile tile in getTilesAsList())
            {
                if (!tile.isSeated() && tile.getAge() >= ageThreshold)
                {
                    tilesWillGetDropped = true;
                }
            }

            return tilesWillGetDropped;
        }

        /**
         * Traverses the TileList. Any Tile that is older in age (milliseconds) than the value
         * passed in will be "dropped" (moved one spot lower on the Y grid). Passes back a flag that indicates
         * whether or not any tiles were dropped during this check.
         */
        public bool dropTilesOlderThanAge(int ageThreshold)
        {
            bool tilesGotDropped = false;
            foreach (Tile tile in getTilesAsList())
            {
                if (!tile.isSeated() && tile.getAge() >= ageThreshold)
                {
                    int newY = tile.getGridY() + 1;
                    tile.setGridY(newY);
                    tile.resetAge();
                    tilesGotDropped = true;
                }
            }

            return tilesGotDropped;
        }

        /**
         * Increases the age of all tiles in the TileList by the amount passed in.
         * "age" is represented in milliseconds and represents how long the Tile has
         * remained in it's current spot on the grid.
         */
        public void ageTiles(int ageIncrement)
        {
            foreach (Tile tile in getTilesAsList())
            {
                //if (!tile.isSeated())
                //{
                    tile.bumpAge(ageIncrement);
                //}
            }
        }

        /**
         * Traverses the TileList to find any Tile objects that are at the bottom of the
         * grid. These objects are set "seated" so that they will no longer be dropped and
         * so that the game logic will be able to tell when all Tiles are seated and it is
         * time to drop a new set.
         */
        public void seatTilesAtFloor()
        {
            foreach (Tile tile in getTilesAsList())
            {
                if (!tile.isSeated() && tile.getGridY() >= this.tileGridHeight - 1)
                {
                    tile.setSeated(true);
                }
            }
        }

        /**
         * Check to see if a tile has another tile directly below it. If so, seat this tile
         * as it can go no further down. This tile will be checked to see if the tile below
         * is a match or if it should be destroyed for any reason in another call.
         */
        public void seatTilesOnTopOfOtherTiles()
        {
            List<Tile> allTiles = getTilesAsList();

            foreach (Tile tileA in allTiles)
            {
                foreach (Tile tileB in allTiles)
                {
                    if (tileB.getGridY() == tileA.getGridY() + 1 && 
                        tileB.getGridX() == tileA.getGridX())
                    {
                        tileA.setSeated(true);
                        break;
                    }
                }
            }
        }

        /**
         * Returns true if the column specified is completely full of tiles height-wise.
         */
        public bool isColumnFull(int columnIndex)
        {
            if (tiles[columnIndex].Count >= this.tileGridHeight)
            {
                return true;
            }

            return false;
        }

        /**
         * Check all the tiles to see if any of them match a condition that requires they be
         * destroyed (depends on the tile type when it get destroyed).
         */
        public GameboardStats markTilesForDeletion()
        {
            //iterate through all the columns
            foreach (List<Tile> column in tiles)
            {
                //iterate through all the tiles in this column and compare to the rest of the column
                foreach (Tile tile in column)
                {
                    //handle top capper tiles
                    if (tile.getType() == Tile.tileType.TOP_CAPPER)
                    {
                        if (tile.isSeated())
                        {
                            Tile bottomCapper;
                            if ((bottomCapper = bottomCapperExistsBelow(tile)) != null)
                            {
                                clearCappedTiles(tile, bottomCapper);
                                stats.numberOfCapsCompleted++;
                            }
                            else
                            {
                                tile.markForDeletion();
                                stats.numberOfBlocksDestroyed++;
                            }
                        }
                    }
                    //handle multplier tiles
                    else if (tile.getType() == Tile.tileType.MULTIPLIER)
                    {
                        if (tile.isSeated())
                        {
                            Tile bottomCapper;
                            if ((bottomCapper = bottomCapperExistsBelow(tile)) == null)
                            {
                                tile.markForDeletion();
                                stats.numberOfBlocksDestroyed++;
                            }
                        }
                    }
                    //handle normal tiles or a bottom capper
                    else
                    {
                        foreach (Tile tileToCompare in column)
                        {
                            if ((tileToCompare.getGridY() == tile.getGridY() + 1 || tileToCompare.getGridY() == tile.getGridY() - 1)
                                && tileToCompare.getStaticTexture() == tile.getStaticTexture()
                                && tile.isSeated()
                                && tileToCompare.isSeated()
                                && !tile.isMarkedForDeletion()
                                && !tileToCompare.isMarkedForDeletion())
                            {
                                tile.markForDeletion();
                                tileToCompare.markForDeletion();

                                stats.numberOfBlocksDestroyed += 2;
                                stats.score += (Tile.BASE_SCORE_VALUE * 2);
                                stats.power += (Tile.BASE_SCORE_VALUE / 8);
                            }
                        }
                    }
                }
            }

            return stats;
        }

        /**
         * Check every tile to see if any are marked for deletion. If so, remove them from the tile list. We do this separately
         * from the check above because we can't remove objects from the List while in the middle of iterating through it.
         */
        public void deleteTilesMarkedForDeletion(GameBoard gameBoard)
        {
            bool atLeastOneTileDeleted = false;

            foreach (List<Tile> column in tiles)
            {
                Tile[] columnArray = column.ToArray();
                for (int i = 0; i < columnArray.Length; i++)
                {
                    Tile thisTile = columnArray[i];
                    if (thisTile.isMarkedForDeletion())
                    {
                        column.Remove(thisTile);
                        AnimationManager.Instance.startAnimation("tile-explode", 25, gameBoard.getTileRectangle(thisTile));
                        atLeastOneTileDeleted = true;
                    }
                }
            }

            if (atLeastOneTileDeleted)
            {
                SoundManager.Instance.playSound("explode-tile");
            }
        }

        /**
         * 
         */
        public void moveTileIntoColumn(Tile tile, int newColumnIndex)
        {
            //first move its position in the tile list
            int oldColumnIndex = tile.getGridX();
            tiles[oldColumnIndex].Remove(tile);
            tiles[newColumnIndex].Add(tile);

            //then set the tiles new position in the tile object itself
            tile.setGridX(newColumnIndex);
        }

        /**
         * 
         */
        private Tile bottomCapperExistsBelow(Tile topCapper)
        {
            Tile bottomCapper = null;
            int columnIndex = topCapper.getGridX();
            List<Tile> bottomCappersInColumn = new List<Tile>();

            foreach (Tile tile in tiles[columnIndex])
            {
                if (tile.getGridY() > topCapper.getGridY() && tile.getType() == Tile.tileType.BOTTOM_CAPPER)
                {
                    bottomCapper = tile;
                    bottomCappersInColumn.Add(tile);
                }
            }

            if(bottomCappersInColumn.Count > 1) 
            {
                foreach(Tile tile in bottomCappersInColumn) 
                {
                    if(tile.getGridY() > bottomCapper.getGridY())
                    {
                        bottomCapper = tile;
                    }
                }
            }

            return bottomCapper;
        }

        /**
         * 
         */
        private void clearCappedTiles(Tile topCapper, Tile bottomCapper)
        {
            if (topCapper.getGridX() != bottomCapper.getGridX())
            {
                return;
            }

            int columnIndex = topCapper.getGridX();
            int numberOfTilesDestroyed = 0;
            int multiplierValue = 1;
            bool regularTileExistsInSandwich = false;

            foreach (Tile tile in tiles[columnIndex])
            {
                if (tile.getGridY() >= topCapper.getGridY() &&
                    tile.getGridY() <= bottomCapper.getGridY() &&
                    !tile.isMarkedForDeletion())
                {
                    tile.startAnimation("explode", 25);
                    tile.markForDeletion();

                    numberOfTilesDestroyed++;

                    if (tile.getType() == Tile.tileType.MULTIPLIER)
                    {
                        stats.numberOfMultipliersCapped++;

                        if (tile.getMultiplier() > multiplierValue)
                        {
                            multiplierValue = tile.getMultiplier();
                        }

                        if (tile.getMultiplier() == 2)
                        {
                            stats.numberOf2xMulipliersCapped++;
                        }
                        else if (tile.getMultiplier() == 3)
                        {
                            stats.numberOf3xMulipliersCapped++;
                        }
                        else
                        {
                            stats.numberOf4xMulipliersCapped++;
                        }
                    }

                    if (tile.getType() == Tile.tileType.NORMAL)
                    {
                        regularTileExistsInSandwich = true;
                    }
                }
            }

            if (!regularTileExistsInSandwich)
            {
                multiplierValue = 1;
            }

            stats.score += (numberOfTilesDestroyed * Tile.BASE_SCORE_VALUE * multiplierValue);
            stats.power += (numberOfTilesDestroyed * (Tile.BASE_SCORE_VALUE / 8));
            stats.numberOfTilesDestroyedByCapping += numberOfTilesDestroyed;
            stats.numberOfBlocksDestroyed += numberOfTilesDestroyed;
        }

        /**
         * Swaps the tiles in two columns. Usually called when the player requests the rotater to spin so that stacks get swapped.
         **/
        public void swapColumns(int colLeft, int colRight)
        {
            List<Tile> leftColumn = this.getTilesInColumn(colLeft);
            List<Tile> rightColumn = this.getTilesInColumn(colRight);

            int leftColumnHeight = this.getSeatedColumnHeight(colLeft);
            int rightColumnHeight = this.getSeatedColumnHeight(colRight);
            int tallestColumn;

            if (leftColumnHeight >= rightColumnHeight)
            {
                tallestColumn = (tileGridHeight - leftColumnHeight) - 1;
            }
            else
            {
                tallestColumn = (tileGridHeight - rightColumnHeight) - 1;
            }

            Tile[] leftColumnArray = leftColumn.ToArray();
            Tile[] rightColumnArray = rightColumn.ToArray();

            for (int i = 0; i < leftColumnArray.Length; i++)
            {
                Tile thisTile = leftColumnArray[i];
                if (thisTile.isSeated() || thisTile.getGridY() > tallestColumn)
                {
                    this.moveTileIntoColumn(thisTile, colLeft + 1);
                }
            }

            for (int i = 0; i < rightColumnArray.Length; i++)
            {
                Tile thisTile = rightColumnArray[i];
                if (thisTile.isSeated() || thisTile.getGridY() > tallestColumn)
                {
                    this.moveTileIntoColumn(thisTile, colRight - 1);
                }
            }
        }

        public int clearColumn(int columnIndex)
        {
            int tilesDestroyed = 0;
            List<Tile> column = tiles[columnIndex];

            foreach (Tile tile in column)
            {
                tile.markForDeletion();
                tilesDestroyed++;

                stats.score += Tile.BASE_SCORE_VALUE;
            }

            return tilesDestroyed;
        }

        public int clearBoard()
        {
            int tilesDestroyed = getTilesAsList().Count;

            for (int i = 0; i < tiles.Count; i++)
            {
                clearColumn(i);
            }

            return tilesDestroyed;
        }

        public List<Tile> getUnseatedTiles()
        {
            List<Tile> unseatedTiles = new List<Tile>();

            foreach (Tile tile in getTilesAsList())
            {
                if (!tile.isSeated())
                {
                    unseatedTiles.Add(tile);
                }
            }

            return unseatedTiles;
        }
    }
}
