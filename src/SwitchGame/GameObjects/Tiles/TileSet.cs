using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Tiles
{
    class TileSet
    {
        private List<Tile> tiles;
        private Tile topCapper;
        private Tile bottomCapper;
        private List<Tile> multiplierTiles;
        private Rotater rotater;
        private Texture2D boardBackground;
        private Texture2D nukeImage;
        private Texture2D blankImage;

        public TileSet()
        {
            tiles = new List<Tile>();
            multiplierTiles = new List<Tile>();
        }

        public void addNormalTile(Tile tile)
        {
            this.tiles.Add(tile);
        }

        public void setTopCapperTile(Tile tile)
        {
            this.topCapper = tile;
        }

        public Tile getTopCapperTile()
        {
            return this.topCapper;
        }

        public void setBottomCapperTile(Tile tile)
        {
            this.bottomCapper = tile;
        }

        public Tile getBottomCapperTile()
        {
            return this.bottomCapper;
        }

        public void addMultiplierTile(Tile tile)
        {
            this.multiplierTiles.Add(tile);
        }

        public void setRotater(Rotater rotater)
        {
            this.rotater = rotater;
        }

        public Rotater getRotater()
        {
            return this.rotater;
        }

        public int getSize()
        {
            return this.tiles.Count + this.multiplierTiles.Count + 2;
        }

        public Texture2D getBoardBackground()
        {
            return this.boardBackground;
        }

        public void setBoardBackground(Texture2D texture)
        {
            this.boardBackground = texture;
        }

        public Texture2D getNukeImage()
        {
            return this.nukeImage;
        }

        public void setNukeImage(Texture2D texture)
        {
            this.nukeImage = texture;
        }

        public Texture2D getBlankImage()
        {
            return this.blankImage;
        }

        public void setBlankImage(Texture2D texture)
        {
            this.blankImage = texture;
        }

        /**
         * Just returns this first normal tile in the TileSet. Used by the GameBoard to determine the height/width of tiles in this tileset.
         */
        public Tile getRefTile()
        {
            if (this.tiles[0] != null)
            {
                return this.tiles[0];
            }
            else
            {
                return null;
            }
        }

        public Tile[] toArray()
        {
            Tile[] tileArray = new Tile[this.getSize()];

            int index = 0;

            foreach (Tile thisTile in tiles)
            {
                tileArray[index] = thisTile;
                index++;
            }

            foreach (Tile thisTile in multiplierTiles)
            {
                tileArray[index] = thisTile;
                index++;
            }

            tileArray[index] = this.topCapper;
            tileArray[index + 1] = this.bottomCapper;

            return tileArray;
        }

        public int getNumberOfNormalTilesInTileSet()
        {
            return tiles.Count;
        }

        public static TileSet loadAndGetDefaultTileset(ContentManager content, Difficulty difficulty)
        {
            TileSet tileSet = new TileSet();

            //load spritesheets
            SpriteSheet tileExplosionSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\explosion-spritesheet"), 5);
            SpriteSheet rotateSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\rotater-spritesheet"), 6);
            SpriteSheet idleRotaterSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\rotater-idle"), 2);
            SpriteSheet laserSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\laser-spritesheet"), 5);

            SpriteSheet idleDiamondSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\diamond-spritesheet"), 3);
            SpriteSheet idleDropSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\drop-spritesheet"), 4);
            SpriteSheet idleTriangleSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\triangle-spritesheet"), 4);
            SpriteSheet idleOctagonSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\octagon-spritesheet"), 4);
            SpriteSheet idleWingSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\wing-spritesheet"), 3);
            SpriteSheet idleCrescentSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\crescent-spritesheet"), 3);
            SpriteSheet idleBottomCapperSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\cap_bottom-spritesheet"), 2);
            SpriteSheet idleTopCapperSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\cap_top-spritesheet"), 2);

            //add normal tiles
            Tile crescentTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\crescent"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\blue"),
                Tile.tileType.NORMAL);
            crescentTile.addAnimation("explode", tileExplosionSpriteSheet);
            crescentTile.addAnimation("idle", idleCrescentSpriteSheet);
            tileSet.addNormalTile(crescentTile);

            if (tileSet.getNumberOfNormalTilesInTileSet() < difficulty.getSizeOfTileSet())
            {
                Tile diamondTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\diamond"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\green"),
                    Tile.tileType.NORMAL);
                diamondTile.addAnimation("explode", tileExplosionSpriteSheet);
                diamondTile.addAnimation("idle", idleDiamondSpriteSheet);
                tileSet.addNormalTile(diamondTile);
            }

            if (tileSet.getNumberOfNormalTilesInTileSet() < difficulty.getSizeOfTileSet())
            {
                Tile dropTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\drop"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\orange"),
                    Tile.tileType.NORMAL);
                dropTile.addAnimation("explode", tileExplosionSpriteSheet);
                dropTile.addAnimation("idle", idleDropSpriteSheet);
                tileSet.addNormalTile(dropTile);
            }

            if (tileSet.getNumberOfNormalTilesInTileSet() < difficulty.getSizeOfTileSet())
            {
                Tile triangleTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\triangle"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\red"),
                    Tile.tileType.NORMAL);
                triangleTile.addAnimation("explode", tileExplosionSpriteSheet);
                triangleTile.addAnimation("idle", idleTriangleSpriteSheet);
                tileSet.addNormalTile(triangleTile);
            }

            if (tileSet.getNumberOfNormalTilesInTileSet() < difficulty.getSizeOfTileSet())
            {
                Tile octagonTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\octagon"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\purple"),
                    Tile.tileType.NORMAL);
                octagonTile.addAnimation("explode", tileExplosionSpriteSheet);
                octagonTile.addAnimation("idle", idleOctagonSpriteSheet);
                tileSet.addNormalTile(octagonTile);
            }

            if (tileSet.getNumberOfNormalTilesInTileSet() < difficulty.getSizeOfTileSet())
            {
                Tile wingTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\wing_triangle"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\teal"),
                    Tile.tileType.NORMAL);
                wingTile.addAnimation("explode", tileExplosionSpriteSheet);
                wingTile.addAnimation("idle", idleWingSpriteSheet);
                tileSet.addNormalTile(wingTile);
            }

            //add capper tiles
            Tile topCapper = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\cap_top"), Tile.tileType.TOP_CAPPER);
            topCapper.addAnimation("explode", tileExplosionSpriteSheet);
            topCapper.addAnimation("idle", idleTopCapperSpriteSheet);
            tileSet.setTopCapperTile(topCapper);

            Tile bottomCapper = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\cap_bottom"), Tile.tileType.BOTTOM_CAPPER);
            bottomCapper.addAnimation("explode", tileExplosionSpriteSheet);
            bottomCapper.addAnimation("idle", idleBottomCapperSpriteSheet);
            tileSet.setBottomCapperTile(bottomCapper);

            //add rotater
            Rotater rotater = new Rotater(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\rotater-idle"));
            rotater.addAnimation("rotate", rotateSpriteSheet);
            rotater.addAnimation("idle", idleRotaterSpriteSheet);
            tileSet.setRotater(rotater);

            //add multiplier tiles
            Tile multiplierTile2X = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\2x"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\mult_bronze"),
                Tile.tileType.MULTIPLIER);
            multiplierTile2X.addAnimation("explode", tileExplosionSpriteSheet);
            multiplierTile2X.setMultiplier(2);
            tileSet.addMultiplierTile(multiplierTile2X);

            Tile mutliplierTile3X = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\3x"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\mult_silver"),
                Tile.tileType.MULTIPLIER);
            mutliplierTile3X.addAnimation("explode", tileExplosionSpriteSheet);
            mutliplierTile3X.setMultiplier(3);
            tileSet.addMultiplierTile(mutliplierTile3X);

            Tile multiplierTile4X = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\4x"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\mult_gold"),
                Tile.tileType.MULTIPLIER);
            multiplierTile4X.addAnimation("explode", tileExplosionSpriteSheet);
            multiplierTile4X.setMultiplier(4);
            tileSet.addMultiplierTile(multiplierTile4X);

            //load board background
            tileSet.setBoardBackground(content.Load<Texture2D>("Sprites\\BoardComponents\\gameboard"));

            //load nuke images
            tileSet.setNukeImage(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\nuke-image"));
            tileSet.setBlankImage(content.Load<Texture2D>("blank"));

            return tileSet;
        }
    }
}
