using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Tiles;

namespace Switch.GameObjects.Tiles
{
    class Tile : SpriteObject
    {
        private bool seated; //is this tile done dropping or not?
        private int age; //measured in milliseconds, used to measure how long since tile was last moved
        private int X;
        private int Y;
        private int boostY = 0;
        private bool markedForDeletion;
        private int timeSinceSeated;
        private tileType type;
        private int idleTimer;
        private Random random;
        private int multiplier;
        private int timeSinceLastDisplayedDrop = 0;
        public enum tileType { NORMAL, BOTTOM_CAPPER, TOP_CAPPER, MULTIPLIER };
        public const int BASE_SCORE_VALUE = 25;

        public Tile(Tile tile) : base(tile.getTexture(), tile.getBackgroundTexture())
        {      
            this.seated = false;
            this.age = 0;
            this.markedForDeletion = false;
            this.type = tile.getType();
            this.multiplier = tile.getMultiplier();
            this.setSpriteSheetDictionary(tile.getSpriteSheetDictionary());
            resetIdleTimer();
        }

        public Tile(Texture2D texture, tileType type) : base(texture)
        {
            this.seated = false;
            this.age = 0;
            this.markedForDeletion = false;
            this.type = type;
            resetIdleTimer();
        }

        public Tile(Texture2D texture, Texture2D backgroundTexture, tileType type)
            : base(texture, backgroundTexture)
        {
            this.seated = false;
            this.age = 0;
            this.markedForDeletion = false;
            this.type = type;
            resetIdleTimer();
        }

        public override void updateGameTime(int elapsedGameTime, int currentGameSpeed, int currentTilePixelHeight)
        {
            base.updateGameTime(elapsedGameTime, currentGameSpeed, currentTilePixelHeight);

            if (!this.isSeated())
            {
                int divisor = 4;

                this.timeSinceLastDisplayedDrop += elapsedGameTime;
                if (this.timeSinceLastDisplayedDrop >= currentGameSpeed / divisor)
                {
                    this.timeSinceLastDisplayedDrop = 0;
                    this.setBoostY(this.boostY += currentTilePixelHeight / divisor);
                }
            }
        }

        public int getBoostY()
        {
            return this.boostY;
        }

        public void setBoostY(int Y)
        {
            this.boostY = Y;
        }

        /**
         * Used to determine when to run an idle animation
         */ 
        public void resetIdleTimer()
        {
            random = new Random();
            idleTimer = (random.Next(8) + 5) * 1000; //5 to 13 seconds
        }

        public bool isSeated()
        {
            return this.seated;
        }

        public void setSeated(bool seated)
        {
            this.seated = seated;
        }

        public void setAge(int age)
        {
            this.age = age;
        }

        public void resetAge()
        {
            this.age = 0;
        }

        public int getAge()
        {
            return this.age;
        }

        public int getMultiplier()
        {
            return this.multiplier;
        }

        public void setMultiplier(int multi)
        {
            this.multiplier = multi;
        }

        public void bumpAge(int increment)
        {
            this.age += increment;

            if (this.seated)
            {
                this.timeSinceSeated = this.age;
            }

            this.idleTimer -= increment;

            if (this.idleTimer <= 0)
            {
                this.startAnimation("idle", 5);
                resetIdleTimer();
            }
        }

        public int getGridX()
        {
            return this.X;
        }

        public void setGridX(int X)
        {
            this.X = X;
        }

        public int getGridY()
        {
            return this.Y;
        }

        public void setGridY(int Y)
        {
            this.Y = Y;
            this.boostY = 0;
            this.timeSinceLastDisplayedDrop = 0;
        }

        public void markForDeletion()
        {
            this.markedForDeletion = true;
        }

        public bool isMarkedForDeletion()
        {
            return this.markedForDeletion;
        }

        public int getTimeSinceSeated()
        {
            return this.timeSinceSeated;
        }

        public tileType getType()
        {
            return this.type;
        }
    }
}
