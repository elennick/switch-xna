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
using Switch.GameObjects.GameDisplays;
using Switch;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects
{
    class GameBoard
    {
        private Vector2 boardPosition; //top left corner of the board, might vary if we are utilizing effects such as the board shaking
        private Vector2 originalBoardPosition; //top left corner of the board, should always stay the same after init
        private TileSet tileSet; //a set of tiles for this gameboard to utilize
        private Rectangle gameBoardRect; //a Rectangle object representing the size of the board
        private Rotater rotater; //the object at the bottom of the board that swaps rows
        private GameMessageBoxDisplay messageBox;
        private List<SpriteObject> animatableSprites;
        private int height; //height of the board in pixels
        private int width; //width of the board in pixels
        private int numTilesHeight; //number of tiles high that this board has as a max
        private int numTilesWidth; //number of tiles across that this board has as a max
        private int startingSpeed; //the number of milliseconds to wait before moving tiles down initially
        private int currentSpeed; //the number of milliseconds to wait before moving tiles down currently
        private int timeBeforeSpeedUp; //the amount of milliseconds before the game increments up in speed
        private int speedUpIncrement; //the amount of time removed before each tile drop... higher means the game will speed up more each time
        private int minGameSpeed; //the point at which the game stops speeding things up
        private int timeSinceLastSpeedUp;
        private int playerIndex; //the player controlling this game board
        private int numTilesToDrop; //how many tiles to drop everytime a new set come down
        private TileList tiles; //a List of all tiles currently on the board
        private bool paused;
        private bool scaleTiles; // whether or not to scale tile graphics to fit the board size
        private bool bulletTimeActive;
        private int bulletTimeLeft;
        private bool gameOver;
        private int timeDownHeld;
        private GameboardStats stats;
        private bool speedupEnabled;
        private bool isShaking;
        private int timeLeftShaking;
        private int timeSinceLastShake;
        private DetailedSpriteObject boardBackgroundSpriteObject;
        private DetailedSpriteObject nukeSpriteObject;
        private DetailedSpriteObject blankWhiteSpritObject;
        private int maxBulletTime;
        private Random random;
        private byte fadeFromWhiteAlpha;
        private Difficulty difficulty;

        /**
         * Constructor
         */
        public GameBoard(Vector2 boardPosition, 
                         TileSet tileSet,
                         Difficulty difficulty,
                         int width, 
                         int height, 
                         int playerIndex)
        {
            //set constructor params
            this.boardPosition = boardPosition;
            this.originalBoardPosition = boardPosition;
            this.difficulty = difficulty;
            this.height = height;
            this.width = width;
            this.numTilesHeight = difficulty.getNumberOfTilesInTheGameboardHeight();
            this.numTilesWidth = difficulty.getNumberOfTilesInTheGameboardWidth();
            this.startingSpeed = difficulty.getStartingSpeed(); //milliseconds
            this.currentSpeed = startingSpeed;
            this.playerIndex = playerIndex;
            this.tileSet = tileSet;

            //init stuff
            this.random = new Random();
            this.animatableSprites = new List<SpriteObject>();
            this.timeBeforeSpeedUp = 60000; //game speeds up every 60 seconds by default
            this.speedUpIncrement = 50; //tiles drop 1/20th of a second faster every time the game speeds up
            this.minGameSpeed = difficulty.getMaxSpeed();
            this.numTilesToDrop = difficulty.getNumberOfTilesToDropPerRound();
            this.paused = true;
            this.scaleTiles = false;
            this.bulletTimeActive = false;
            this.bulletTimeLeft = 0;
            this.gameOver = false;
            this.timeDownHeld = 0;
            this.speedUpIncrement = 15;
            this.timeBeforeSpeedUp = 60000;
            this.speedupEnabled = true;
            this.minGameSpeed = 150;
            this.timeSinceLastSpeedUp = 0;
            this.isShaking = false;
            this.timeLeftShaking = 0;
            this.timeSinceLastShake = 0;
            this.fadeFromWhiteAlpha = 255;
            this.maxBulletTime = 15000; //bullet time is 15 seconds long by default

            this.rotater = new Rotater(this.tileSet.getRotater(), 0, this.numTilesWidth - 2, 1);
            this.animatableSprites.Add(this.rotater);

            int startingPower;
            if (!SwitchGame.DEBUG_MODE)
            {
                startingPower = 25;
            }
            else
            {
                startingPower = 100;
            }
            this.stats = new GameboardStats(startingPower);
            this.tiles = new TileList(numTilesWidth, numTilesHeight, stats);

            this.gameBoardRect = new Rectangle((int)boardPosition.X, 
                                               (int)boardPosition.Y, 
                                               (int)(boardPosition.X + width), 
                                               (int)(boardPosition.Y + height));

            boardBackgroundSpriteObject = new DetailedSpriteObject(tileSet.getBoardBackground(), new Rectangle((int)(boardPosition.X - 10), 0, width + 20, height + 111));
            nukeSpriteObject = new DetailedSpriteObject(tileSet.getNukeImage(), new Rectangle((int)originalBoardPosition.X, (int)originalBoardPosition.Y, width, height));
            blankWhiteSpritObject = new DetailedSpriteObject(tileSet.getBlankImage(), new Rectangle((int)originalBoardPosition.X, (int)originalBoardPosition.Y, width, height));
        }

        public int getPlayerIndex()
        {
            return this.playerIndex;
        }

        public Difficulty getDifficulty()
        {
            return this.difficulty;
        }

        public void setMessageBoxDisplay(GameMessageBoxDisplay messageBox)
        {
            this.messageBox = messageBox;
        }

        public void addMessageBoxMessage(String message, Color color)
        {
            if (this.messageBox != null)
            {
                this.messageBox.addMessage(message, color);
            }
        }

        public void addMessageBoxMessage(String message)
        {
            addMessageBoxMessage(message, new Color(217, 217, 217));
        }

        public GameboardStats getStats()
        {
            return this.stats;
        }

        public int getMaxBulletTime()
        {
            return this.maxBulletTime;
        }

        public Rectangle getRect()
        {
            return this.gameBoardRect;
        }

        public void setSpeedUpTimer(int milliseconds)
        {
            timeBeforeSpeedUp = milliseconds;
        }

        public void setSpeedUpEnabled(bool enabled)
        {
            this.speedupEnabled = enabled;
        }

        public bool isSpeedUpEnabled()
        {
            return this.speedupEnabled;
        }

        public TileSet getTileSet()
        {
            return this.tileSet;
        }

        public Vector2 getPosition()
        {
            return this.boardPosition;
        }

        public int getHeight()
        {
            return this.height;
        }

        public int getWidth()
        {
            return this.width;
        }

        public float getTilePixelWidth()
        {
            return this.width / this.numTilesWidth;
        }

        public float getTilePixelHeight()
        {
            return this.height / this.numTilesHeight;
        }

        public void setScaleTiles(bool scaleTiles)
        {
            this.scaleTiles = scaleTiles;
        }

        public bool isScaleTiles()
        {
            return this.scaleTiles;
        }

        public void setPaused(bool paused)
        {
            this.paused = paused;
        }

        public int getNumTilesToDrop()
        {
            return this.numTilesToDrop;
        }

        public void setNumTilesToDrop(int num)
        {
            this.numTilesToDrop = num;
        }

        public int getScore()
        {
            return this.stats.score;
        }

        public int getPower()
        {
            return this.stats.power;
        }

        public int getBulletTimeLeft()
        {
            return this.bulletTimeLeft;
        }

        public bool isGameOver()
        {
            return this.gameOver;
        }

        public bool isBulletTimeOn()
        {
            return this.bulletTimeActive;
        }

        public bool isNukeAnimationOn()
        {
            return this.isShaking;
        }

        public int getCurrentSpeed()
        {
            int speed = this.currentSpeed;

            if (this.timeDownHeld > 200)
            {
                speed /= 4;
            }
            else if (this.bulletTimeActive)
            {
                speed *= 3;
            }

            return speed;
        }

        public Rectangle getTileRectangle(Tile tile)
        {
            int thisTilePosX = (int)(this.getPosition().X + (this.getTilePixelWidth() * tile.getGridX()));
            int thisTilePosY = (int)(this.getPosition().Y + (this.getTilePixelHeight() * tile.getGridY()));
            Vector2 thisTilesPosition = new Vector2(thisTilePosX, thisTilePosY);

            return new Rectangle(thisTilePosX, thisTilePosY, (int)getTilePixelWidth(), (int)getTilePixelHeight());
        }

        //this method is really ineffecient, needs to be refactored badly
        public void dropNewTileSet(int numTiles)
        {
            if (numTiles >= this.numTilesWidth)
            {
                numTiles = this.numTilesWidth - 1;
            }

            //get random tile types to drop
            Tile[] tilesToDrop = this.getRandomTiles(numTiles);

            //drop them into different columns
            List<int> columnsToDropIn = new List<int>();
            
            while(true) {
                int columnToDropIn = random.Next(numTilesWidth);

                if (!columnsToDropIn.Contains(columnToDropIn))
                {
                    columnsToDropIn.Add(columnToDropIn);
                    if (columnsToDropIn.Count >= numTiles)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //check to see if any of the columns we dropped into are already full
            for (int i = 0; i < columnsToDropIn.Count; i++)
            {
                if (tiles.isColumnFull(columnsToDropIn[i]))
                {
                    this.paused = true;
                    this.gameOver = true;
                }
            }

            //update the tile grid with the new tiles
            for (int i = 0; i < columnsToDropIn.Count; i++)
            {
                tilesToDrop[i].setGridX(columnsToDropIn[i]);
                tilesToDrop[i].setGridY(0);
                tiles.Add(tilesToDrop[i], columnsToDropIn[i]);
            }
        }

        public void shakeBoard(int millisToShakeFor)
        {
            if (!this.isShaking)
            {
                this.isShaking = true;
                this.timeLeftShaking = millisToShakeFor;
                this.timeSinceLastShake = 0;
                this.setPaused(true);
                this.fadeFromWhiteAlpha = 255;
            }
        }

        public void levelUp()
        {
            this.currentSpeed = (int)MathHelper.Clamp(this.currentSpeed - this.speedUpIncrement, this.minGameSpeed, this.startingSpeed);
            this.timeSinceLastSpeedUp = 0;
            addMessageBoxMessage("Level Up!", Color.Silver);
            this.stats.level++;
            SoundManager.Instance.playSound("levelup");
        }

        /**
         * Called by the screen that is utilizing this board during every Update() cycle
         */
        public void update(GameTime gameTime)
        {
            int elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

            stats.timeElapsed += elapsedTime;
            timeDownHeld += elapsedTime;
            this.timeSinceLastSpeedUp += elapsedTime;

            //let all sprite objects know the current elapsed game time in case they need to update a frame
            foreach(SpriteObject sprite in animatableSprites) {
                sprite.updateGameTime(elapsedTime);
            }

            foreach(Tile sprite in tiles.getTilesAsList())
            {
                sprite.updateGameTime(elapsedTime, this.getCurrentSpeed(), (int)this.getTilePixelHeight());
            }

            AnimationManager.Instance.updateGameTime(elapsedTime);
            VibrationManager.Instance.update(elapsedTime);

            //make sure the power level isnt over max
            this.stats.power = (int)MathHelper.Clamp(this.stats.power, 0, 100);

            //shake the board if shaking is enabled
            if (this.isShaking)
            {
                VibrationManager.Instance.vibrateController((PlayerIndex)this.playerIndex, 200);

                if (this.timeLeftShaking <= 0)
                {
                    this.isShaking = false;
                    setPaused(false);
                    this.boardPosition = this.originalBoardPosition;
                }
                else if(this.timeSinceLastShake >= 75)
                {
                    this.timeSinceLastShake = 0;
                    int xVariation = random.Next(20) - 10;
                    int YVariation = random.Next(20) - 10;

                    this.boardPosition = new Vector2(this.originalBoardPosition.X + xVariation,
                                                     this.originalBoardPosition.Y + YVariation);
                }

                this.fadeFromWhiteAlpha -= 3;
                this.timeLeftShaking -= elapsedTime;
                this.timeSinceLastShake += elapsedTime;
            }

            //if bullet time is currently engaged, check to see if it is time to turn it off
            if (this.bulletTimeActive)
            {
                this.bulletTimeLeft -= elapsedTime;
                if (this.bulletTimeLeft <= 0)
                {
                    this.stopBulletTime();
                }
            }

            //this is the core logic that defines gameplay
            if (!this.paused)
            {
                if (tiles.areAllTilesSeated())
                {
                    dropNewTileSet(this.numTilesToDrop);
                }
                else
                {
                    if (tiles.willTilesGetDropped(getCurrentSpeed()))
                    {
                        tiles.seatTilesOnTopOfOtherTiles();
                        tiles.seatTilesAtFloor();

                        if (this.timeSinceLastSpeedUp >= this.timeBeforeSpeedUp
                            && this.speedupEnabled)
                        {
                            levelUp();
                        }

                        tiles.dropTilesOlderThanAge(getCurrentSpeed());

                        int scoreBeforeTileDeletion = stats.score;
                        stats = tiles.markTilesForDeletion();
                        pointsGained(stats.score - scoreBeforeTileDeletion);
                    }

                    tiles.deleteTilesMarkedForDeletion(this);
                }

                tiles.ageTiles(elapsedTime);
            }
        }

        public void startGame()
        {
            addMessageBoxMessage("Game Start!", Color.GreenYellow);
            setPaused(false);
            dropNewTileSet(this.numTilesToDrop);
            startRotaterIdleAnimation();
        }

        public void startRotaterIdleAnimation()
        {
            if (!this.rotater.isAnimating())
            {
                this.rotater.startAnimation("idle", 2, true);
            }
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //draw the board background
            spriteBatch.Draw(boardBackgroundSpriteObject.getTexture(), boardBackgroundSpriteObject.getDestinationRect(), Color.White);

            //draw the nuke image if the board is currently shaking due to a nuke
            if (this.isShaking)
            {
                spriteBatch.Draw(nukeSpriteObject.getTexture(), nukeSpriteObject.getDestinationRect(), Color.White);
            }

            //if scaling is turned on, figure out the scale size
            Vector2 scale = new Vector2(1, 1);
            Vector2 centerImageScale = new Vector2(1, 1);
            Tile referenceTile = this.getTileSet().getRefTile();

            if (this.isScaleTiles())
            {
                try
                {
                    float scaleX = this.getTilePixelWidth() / referenceTile.getTexture().Width;
                    float scaleY = this.getTilePixelHeight() / referenceTile.getTexture().Height;
                    scale = new Vector2(scaleX, scaleY);

                    if (scaleX <= scaleY)
                    {
                        centerImageScale = new Vector2(scaleX, scaleX);
                    }
                    else
                    {
                        centerImageScale = new Vector2(scaleY, scaleY);
                    }
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }
            }

            //draw the tiles
            List<Tile> tilesToDraw = tiles.getTilesAsList();
            foreach (Tile tile in tilesToDraw)
            {
                //figure out where on the screen to draw the tile background
                int thisTilePosX = (int)(this.getPosition().X + (this.getTilePixelWidth() * tile.getGridX()));
                int thisTilePosY = (int)(this.getPosition().Y + (this.getTilePixelHeight() * tile.getGridY()));
                //int thisTilePosY = (int)(this.getPosition().Y + (this.getTilePixelHeight() * tile.getGridY()) + tile.getBoostY());

                Vector2 thisTilesPosition = new Vector2(thisTilePosX, thisTilePosY);

                //draw the background if necessary
                if (tile.getBackgroundTexture() != null)
                {                
                    //draw the background sprite
                    spriteBatch.Draw(tile.getBackgroundTexture(), thisTilesPosition, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }

                //figure out where on screen to draw the tile foreground image
                Vector2 thisTilesCenterOrigin = new Vector2(tile.getTexture().Width / 2, tile.getTexture().Height / 2);
                Vector2 thisTilesCenterPosition = new Vector2(thisTilePosX + (this.getTilePixelWidth() / 2), thisTilePosY + (this.getTilePixelHeight() / 2));
                Vector2 thisTilesCenterOriginDuringIdleAnimation = new Vector2(tile.getCurrentCelRect().Width / 2, tile.getCurrentCelRect().Height / 2);

                //finally, draw the foreground of the tile
                if (tile.isAnimating() && tile.getCurrentAnimation() == "idle")
                {
                    spriteBatch.Draw(tile.getTexture(), thisTilesCenterPosition, tile.getCurrentCelRect(), Color.White, 0,
                                     thisTilesCenterOriginDuringIdleAnimation, centerImageScale, SpriteEffects.None, 0);
                }
                else if (tile.isAnimating())
                {
                    spriteBatch.Draw(tile.getTexture(), thisTilesPosition, tile.getCurrentCelRect(), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(tile.getTexture(), thisTilesCenterPosition, tile.getCurrentCelRect(), Color.White, 0, thisTilesCenterOrigin, centerImageScale, SpriteEffects.None, 0);
                }
            }

            //draw the nuke white overlay image if the board is currently shaking due to a nuke
            if (this.isShaking)
            {
                Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp(fadeFromWhiteAlpha, 0, 255));
                spriteBatch.Draw(blankWhiteSpritObject.getTexture(), blankWhiteSpritObject.getDestinationRect(), color);
            }

            //draw the rotater
            startRotaterIdleAnimation();

            int rotaterYOffset = 12;
            int rotaterPosX = (int)(this.getPosition().X + (this.getTilePixelWidth() * rotater.getHorizontalPosition()));
            int rotaterPosY = (int)(this.boardPosition.Y + this.height + rotaterYOffset);

            Vector2 rotaterPosition = new Vector2(rotaterPosX, rotaterPosY);
            Vector2 rotaterScale = new Vector2(scale.X * 2, 1.15f);

            spriteBatch.Draw(rotater.getTexture(), rotaterPosition, rotater.getCurrentCelRect(), Color.White, 0, Vector2.Zero, rotaterScale, SpriteEffects.None, 0);

            //draw any animations that are going
            AnimationManager.Instance.drawAnimations(spriteBatch);
        }

        public void handleInput(InputState input)
        {
            //game controller stuff
            GamePadState gamePadState = input.CurrentGamePadStates[this.playerIndex];
            GamePadState previousGamePadState = input.LastGamePadStates[this.playerIndex];

            if (gamePadState.ThumbSticks.Left.X < -0.3 && previousGamePadState.ThumbSticks.Left.X >= -0.3)
            {
                rotater.moveLeft();
            }
            if (gamePadState.ThumbSticks.Left.X > 0.3 && previousGamePadState.ThumbSticks.Left.X <= 0.3)
            {
                rotater.moveRight();
            }

            if (gamePadState.DPad.Left == ButtonState.Pressed && previousGamePadState.DPad.Left == ButtonState.Released)
            {
                rotater.moveLeft();
            }
            if (gamePadState.DPad.Right == ButtonState.Pressed && previousGamePadState.DPad.Right == ButtonState.Released)
            {
                rotater.moveRight();
            }

            if (gamePadState.IsButtonDown(Buttons.A) && previousGamePadState.IsButtonUp(Buttons.A))
            {
                swapColumns();
            }
            if (gamePadState.IsButtonDown(Buttons.X) && previousGamePadState.IsButtonUp(Buttons.X))
            {
                engageBulletTime();
            }
            if (gamePadState.IsButtonDown(Buttons.Y) && previousGamePadState.IsButtonUp(Buttons.Y))
            {
                fireDaLasersLawl(rotater.getHorizontalPosition(), rotater.getHorizontalPosition() + 1);
            }
            if (gamePadState.IsButtonDown(Buttons.B) && previousGamePadState.IsButtonUp(Buttons.B))
            {
                fireNuke();
            }

            if (gamePadState.ThumbSticks.Left.Y >= -0.3 && gamePadState.DPad.Down != ButtonState.Pressed)
            {
                timeDownHeld = 0;
            }

            //keyboard stuff
            KeyboardState keyboardState = input.CurrentKeyboardStates[this.playerIndex];
            KeyboardState previousKeyboardState = input.LastKeyboardStates[this.playerIndex];

            if (keyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
            {
                rotater.moveLeft();
            }
            if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
            {
                rotater.moveRight();
            }

            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                swapColumns();
            }
            if (keyboardState.IsKeyDown(Keys.LeftControl) && previousKeyboardState.IsKeyUp(Keys.LeftControl))
            {
                engageBulletTime();
            }
            if (keyboardState.IsKeyDown(Keys.LeftAlt) && previousKeyboardState.IsKeyUp(Keys.LeftAlt))
            {
                fireDaLasersLawl(rotater.getHorizontalPosition(), rotater.getHorizontalPosition() + 1);
            }
            if (keyboardState.IsKeyDown(Keys.LeftShift) && previousKeyboardState.IsKeyUp(Keys.LeftShift))
            {
                fireNuke();
            }
        }

        public List<DetailedSpriteObject> getSpritesToBlur()
        {
            List<DetailedSpriteObject> tilesToBlur = new List<DetailedSpriteObject>();

            foreach (Tile tile in tiles.getUnseatedTiles())
            {
                tilesToBlur.Add(new DetailedSpriteObject(tile.getBackgroundTexture(),
                                                         this.getTileRectangle(tile)));
            }

            return tilesToBlur;
        }

        private void swapColumns()
        {
            rotater.startAnimation("rotate", 80);
            SoundManager.Instance.playSound("flip");
            tiles.swapColumns(rotater.getHorizontalPosition(), rotater.getHorizontalPosition() + 1);
        }

        private void engageBulletTime()
        {
            if (!this.bulletTimeActive && this.stats.power >= 25)
            {
                addMessageBoxMessage("Bullet Time Engaged!", Color.Blue);
                SoundManager.Instance.playSound("bullettime");

                this.bulletTimeActive = true;
                this.bulletTimeLeft += 15000;
                this.maxBulletTime = this.bulletTimeLeft;
                this.stats.numberOfBulletTimesFired++;
                this.stats.score += 25;

                if (!SwitchGame.DEBUG_MODE)
                {
                    this.stats.power -= 25;
                }
            }
        }

        private void stopBulletTime()
        {
            if (this.bulletTimeActive)
            {
                addMessageBoxMessage("Bullet Time Over!", Color.Blue);

                this.bulletTimeActive = false;
                this.bulletTimeLeft = 0;
            }
        }

        private void fireDaLasersLawl(int leftMostColumnToBlastWithALaserLawl, int rightMostColumnToBlastWithALaserLawl)
        {
            if (this.stats.power >= 50)
            {
                for (int i = leftMostColumnToBlastWithALaserLawl; i <= rightMostColumnToBlastWithALaserLawl; i++)
                {
                    int numTilesDestroyed = tiles.clearColumn(i); //todo - update this to give points for stuff destroyed
                    stats.numberOfBlocksDestroyed += numTilesDestroyed;
                    stats.numberOfBlocksDestroyedByLaser += numTilesDestroyed;
                }

                this.stats.numberOfLasersFired++;
                this.stats.score += 50;
                addMessageBoxMessage("Laser Fired!", Color.Yellow);
                SoundManager.Instance.playSound("laser");

                Rectangle animationRect = new Rectangle((int)(getPosition().X + (leftMostColumnToBlastWithALaserLawl * getTilePixelWidth())), 
                                                        (int)getPosition().Y, 
                                                        (int)(getTilePixelWidth() * 2),
                                                        getHeight());
                AnimationManager.Instance.startAnimation("laser", 50, animationRect);
                VibrationManager.Instance.vibrateController((PlayerIndex)this.playerIndex, 300);

                if (!SwitchGame.DEBUG_MODE)
                {
                    this.stats.power -= 50;
                }
            }
        }

        private void fireNuke()
        {
            if (this.stats.power >= 100)
            {
                int numTilesDestroyed = tiles.clearBoard(); //todo - update this to give points for stuff destroyed
                stats.numberOfBlocksDestroyed += numTilesDestroyed;
                stats.numberOfBlocksDestroyedByNuke += numTilesDestroyed;
                stats.score += 100;

                if (this.bulletTimeActive)
                {
                    stats.numberOfNukesFiredDuringActiveBulletTime++;
                }

                this.stats.numberOfNukesFired++;
                addMessageBoxMessage("Nuke Dropped!!!", Color.Red);
                SoundManager.Instance.playSound("nuke-explode");

                Rectangle animationRect = new Rectangle((int)getPosition().X,
                                                        (int)getPosition().Y,
                                                        getWidth(),
                                                        getHeight());

                this.shakeBoard(1250);

                if (!SwitchGame.DEBUG_MODE)
                {
                    this.stats.power -= 100;
                }
            }
        }

        /**
         * Returns an array of tiles, randomly chosen from the TileSet. Duplicates are a possibility.
         **/
        private Tile[] getRandomTiles(int numOfTiles)
        {
            Tile[] allTiles = tileSet.toArray();

            List<Tile> tilePool = new List<Tile>();
            foreach (Tile tile in allTiles)
            {
                int probabilityCounter;

                if (tile.getType() == Tile.tileType.NORMAL)
                {
                    probabilityCounter = 6;
                }
                else if (tile.getType() == Tile.tileType.BOTTOM_CAPPER ||
                    tile.getType() == Tile.tileType.TOP_CAPPER)
                {
                    probabilityCounter = 6;
                }
                else
                {
                    probabilityCounter = 1;
                }

                for (int i = 0; i < probabilityCounter; i++)
                {
                    tilePool.Add(tile);
                }
            }

            Tile[] randomTileArray = new Tile[numOfTiles];

            for (int i = 0; i < numOfTiles; i++)
            {
                int randomTilePosition = random.Next(tilePool.Count);
                randomTileArray[i] = new Tile(tilePool[randomTilePosition]);
            }

            return randomTileArray;
        }

        private void pointsGained(int pointsGained)
        {
            if (pointsGained > 0 && pointsGained <= 100)
            {
                addMessageBoxMessage("Good! +" + pointsGained + " Points!");
            }
            else if (pointsGained > 100 && pointsGained <= 250)
            {
                addMessageBoxMessage("Nice One! +" + pointsGained + " Points!", Color.Gold);
            }
            else if (pointsGained > 250 && pointsGained <= 500)
            {
                addMessageBoxMessage("Stupendous! +" + pointsGained + " Points!", Color.Gold);
            }
            else if (pointsGained > 500 && pointsGained <= 1000)
            {
                addMessageBoxMessage("Spectacular! +" + pointsGained + " Points!", Color.Gold);
            }
            else if (pointsGained > 1000)
            {
                addMessageBoxMessage("Incredible! +" + pointsGained + " Points!", Color.Gold);
            }
        }
    }
}