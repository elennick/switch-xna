using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.GameObjects;
using Switch.GameObjects.GameDisplays;
using Switch.GameObjects.Sound;
using Microsoft.Xna.Framework.Content;
using Switch.Menus;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Switch.GameObjects.Challenges;
using Switch.HighScores;
using Switch.Utils.Difficulty;
using Microsoft.Xna.Framework.GamerServices;

namespace Switch
{
    abstract class GameplayScreen : GameScreen
    {
        protected ContentManager content;
        private List<GameBoard> gameBoards;
        private List<GameDisplay> gameDisplays;
        private List<DetailedSpriteObject> spriteObjects;
        private int numPlayers;
        protected Challenge challenge;
        public CountDownState currentCountDownState;
        private int timeSinceLastCountDownChange;
        private SpriteFont countDownFont;
        public enum CountDownState { NOT_STARTED, PAUSE, READY, SET, GO, DONE };
        public enum GameMode { SCORE_ATTACK, CHALLENGE_MODE, BATTLE_MODE };
        private bool gameStarted;
        protected PlayerIndex? playerIndex1, playerIndex2;
        bool trackingFPS = false;
        int lastKnownFPS = 0;
        int numberOfFramesSinceLastUpdate = 0;
        int timeSinceLastUpdate = 0;
        private Texture2D readyImage, setImage, goImage;

        public GameplayScreen(int numPlayers)
        {
            gameBoards = new List<GameBoard>();
            gameDisplays = new List<GameDisplay>();
            spriteObjects = new List<DetailedSpriteObject>();
            this.numPlayers = numPlayers;
            this.timeSinceLastCountDownChange = 0;
            this.gameStarted = false;

            playerIndex1 = null;
            playerIndex2 = null;

            SoundManager.Instance.setMusicPaused(true);
        }

        abstract public GameMode getGameMode();

        public override void LoadContent()
        {
            countDownFont = content.Load<SpriteFont>("Fonts\\ReadySetGoFont");
            readyImage = content.Load<Texture2D>("Sprites\\BoardComponents\\ReadySetGo\\ready");
            setImage = content.Load<Texture2D>("Sprites\\BoardComponents\\ReadySetGo\\set");
            goImage = content.Load<Texture2D>("Sprites\\BoardComponents\\ReadySetGo\\go");

            base.LoadContent();
        }

        public void addGameplayScreenObject(Object gameplayScreenObject)
        {
            if (gameplayScreenObject is GameBoard)
            {
                gameBoards.Add((GameBoard)gameplayScreenObject);
            }
            else if (gameplayScreenObject is GameDisplay)
            {
                gameDisplays.Add((GameDisplay)gameplayScreenObject);
            }
            else if (gameplayScreenObject is DetailedSpriteObject)
            {
                spriteObjects.Add((DetailedSpriteObject)gameplayScreenObject);
            }
            else
            {
                throw new ArgumentException("Object being passed in as a gameplay " + 
                    "screen object is not a GameBoard, GameDisplay or DetailedSpriteObject");
            }
        }

        public List<GameBoard> getGameBoards()
        {
            return gameBoards;
        }

        public List<GameDisplay> getGameDisplays()
        {
            return gameDisplays;
        }

        public List<DetailedSpriteObject> getSpriteObjects()
        {
            return spriteObjects;
        }

        public void startCountdown()
        {
            currentCountDownState = CountDownState.PAUSE;
            timeSinceLastCountDownChange = 0;
        }

        public void setPlayerOne(PlayerIndex playerIndex)
        {
            playerIndex1 = playerIndex;
        }

        public void setPlayerTwo(PlayerIndex playerIndex)
        {
            playerIndex2 = playerIndex;
        }

        public void startGameplay()
        {
            SoundManager.Instance.setMusicPaused(false);
            SoundManager.Instance.playSong("gameplay-song");

            foreach (GameBoard gameBoard in gameBoards)
            {
                gameBoard.startGame();
            }

            this.gameStarted = true;
        }

        public override void UnloadContent()
        {
            SoundManager.Instance.stopSong();
            AnimationManager.Instance.clearAllAnimations();
            VibrationManager.Instance.cancelAllVibrations();

            try
            {
                content.Unload();
            }
            catch (NullReferenceException nre)
            {
                //this gets called more than once sometimes for some reason and im a bad and lazy
                //programmer so this is how i am going to deal with it
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (trackingFPS)
            {
                timeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastUpdate >= 1000)
                {
                    lastKnownFPS = numberOfFramesSinceLastUpdate;
                    timeSinceLastUpdate = 0;
                    numberOfFramesSinceLastUpdate = 0;
                    System.Diagnostics.Debug.WriteLine("FPS = " + lastKnownFPS);
                }
            }

            if (currentCountDownState == CountDownState.NOT_STARTED)
            {
                this.startCountdown();
                return;
            }

            if (currentCountDownState != CountDownState.DONE)
            {
                timeSinceLastCountDownChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastCountDownChange >= 1000)
                {
                    timeSinceLastCountDownChange = 0;
                    currentCountDownState++;
                    
                    if(currentCountDownState == CountDownState.READY || currentCountDownState == CountDownState.SET) {
                        SoundManager.Instance.playSound("readySet");
                    }
                    else if (currentCountDownState == CountDownState.GO)
                    {
                        SoundManager.Instance.playSound("go");
                    }
                }
            }

            if (IsActive)
            {
                foreach (GameBoard gameBoard in gameBoards)
                {
                    if (gameBoard.isGameOver()
                        && !AnimationManager.Instance.areAnyAnimationsActive()
                        && !gameboardIsFiringNuke())
                    {
                        VibrationManager.Instance.cancelAllVibrations();

                        if (this.getGameMode() == GameMode.SCORE_ATTACK)
                        {
                            int score = gameBoard.getScore();
                            Difficulty diff = gameBoard.getDifficulty();

                            String name;
                            try
                            {
                                name = SignedInGamer.SignedInGamers[(int)playerIndex1].Gamertag;
                            }
                            catch (Exception e)
                            {
                                name = "Guest";
                            }

                            HighScore highScore = new HighScore(name, score, diff);
                            HighScoreManager.Instance.addHighScore(highScore);
                        }

                        if (this.getGameMode() != GameMode.BATTLE_MODE)
                        {
                            saveStats();
                            GameOverBackToMenuScreen confirmGameOverMessageBox = new GameOverBackToMenuScreen(this.getGameMode());
                            ScreenManager.AddScreen(confirmGameOverMessageBox, null);
                        }
                        else
                        {
                            //TODO - THIS WHOLE CHUNK OF CODE IS BROKEN FIX IT
                            String playerIndex;
                            if (gameBoard.getPlayerIndex() == 0)
                            {
                                //player 1 lost, so the winner is player 2
                                playerIndex = "2";
                            }
                            else
                            {
                                //player 2 lost, so the winner is player 1
                                playerIndex = "1";
                            }
                            //TODO - THE WHOLE CHUNK OF CODE ABOVE THIS IS BROKEN - FIX IT

                            saveStats();
                            BattleModeGameOverScreen battleModeGameOverScreen = new BattleModeGameOverScreen("Player " + playerIndex + " Wins!", (int)playerIndex1, (int)playerIndex2);
                            ScreenManager.AddScreen(battleModeGameOverScreen, null);
                        }
                    }

                    if (this.challenge != null 
                        && this.challenge.isCompleted(gameBoard.getStats())
                        && !AnimationManager.Instance.areAnyAnimationsActive()
                        && !gameboardIsFiringNuke()
                        && !gameBoard.isGameOver())
                    {
                        VibrationManager.Instance.cancelAllVibrations();
                        SoundManager.Instance.playSound("player-select");

                        ChallengeManager challengeManager = ChallengeManager.Instance;
                        if (!challengeManager.getChallengeStatus(this.challenge.getName()))
                        {
                            challengeManager.setChallengeCompleteStatus(this.challenge.getName(), true);
#if XBOX
                            StorageManager.Instance.saveChallengeStatuses(challengeManager.getChallengeSaveData());
                            saveStats();
#endif
                        }

                        ChallengeCompleteScreen challengeCompleteScreen = new ChallengeCompleteScreen("Challenge Completed!");
                        ScreenManager.AddScreen(challengeCompleteScreen, null);
                    }

                    gameBoard.update(gameTime);
                }

                foreach (GameDisplay gameDisplay in gameDisplays)
                {
                    gameDisplay.update(gameTime, false, false);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (trackingFPS)
            {
                numberOfFramesSinceLastUpdate++;
            }

            /*ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White, 0, 0);*/

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            //draw background images
            foreach (DetailedSpriteObject sprite in spriteObjects)
            {
                spriteBatch.Draw(sprite.getTexture(), sprite.getDestinationRect(), Color.White);
            }

            foreach (GameBoard gameBoard in gameBoards)
            {
                //draw the gameboard (tiles, rotater, etc)
                gameBoard.draw(spriteBatch, gameTime);

                //draw game displays (score, power meters, etc)
                foreach (GameDisplay display in gameDisplays)
                {
                    display.draw(spriteBatch, gameTime);
                }
            }

            if (!gameStarted)
            {
                Vector2 readySetGoImagePosition = new Vector2(640, 360);
                if (currentCountDownState == CountDownState.READY)
                {
                    Vector2 readyImageOrigin = new Vector2(readyImage.Width / 2, readyImage.Height / 2);
                    spriteBatch.Draw(readyImage, readySetGoImagePosition, null, Color.White, 0, readyImageOrigin, Vector2.One, SpriteEffects.None, 0);
                }
                else if (currentCountDownState == CountDownState.SET)
                {
                    Vector2 setImageOrigin = new Vector2(setImage.Width / 2, setImage.Height / 2);
                    spriteBatch.Draw(setImage, readySetGoImagePosition, null, Color.White, 0, setImageOrigin, Vector2.One, SpriteEffects.None, 0);
                }
                else if (currentCountDownState == CountDownState.GO)
                {
                    Vector2 goImageOrigin = new Vector2(goImage.Width / 2, goImage.Height / 2);
                    spriteBatch.Draw(goImage, readySetGoImagePosition, null, Color.White, 0, goImageOrigin, Vector2.One, SpriteEffects.None, 0);
                }
                else if(currentCountDownState == CountDownState.DONE)
                {
                    this.startGameplay();
                }
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
            {
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
            }
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            GamePadState gamePadState1 = input.CurrentGamePadStates[(int)playerIndex1];
            bool gamePadDisconnected1 = (!gamePadState1.IsConnected &&
                                         input.GamePadWasConnected[(int)playerIndex1]);

            GamePadState gamePadState2;
            bool gamePadDisconnected2 = false;
            if (playerIndex2 != null)
            {
                gamePadState2 = input.CurrentGamePadStates[(int)playerIndex2];
                gamePadDisconnected2 = (!gamePadState2.IsConnected &&
                                             input.GamePadWasConnected[(int)playerIndex2]);
            }

            if (input.IsPauseGame(playerIndex1) || gamePadDisconnected1)
            {
                pauseGame((PlayerIndex)playerIndex1);
            }
            else if (playerIndex2 != null && (input.IsPauseGame(playerIndex2) || gamePadDisconnected2))
            {
                pauseGame((PlayerIndex)playerIndex2);
            }
            else if(this.currentCountDownState == CountDownState.DONE)
            {
                foreach (GameBoard gameBoard in gameBoards)
                {
                    gameBoard.handleInput(input);
                }
            }

        }

        private bool gameboardIsFiringNuke()
        {
            foreach (GameBoard gameBoard in gameBoards)
            {
                if (gameBoard.isNukeAnimationOn())
                {
                    return true;
                }
            }

            return false;
        }

        private void saveStats()
        {
            if (gameBoards.Count > 1)
            {
                GameboardStats consolidatedStats = new GameboardStats();
                foreach (GameBoard gameBoardToSaveStatsFor in gameBoards)
                {
                    consolidatedStats.addStats(gameBoardToSaveStatsFor.getStats());
                }

                StorageManager.Instance.addStatsData(consolidatedStats);
            }
            else
            {
                StorageManager.Instance.addStatsData(gameBoards[0].getStats());
            }
        }

        private void pauseGame(PlayerIndex playerIndexThatPaused)
        {
            if (this.currentCountDownState == CountDownState.DONE)
            {
                VibrationManager.Instance.cancelAllVibrations();
                if (this.getGameMode() == GameMode.CHALLENGE_MODE)
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(this.challenge), playerIndexThatPaused);
                }
                else
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(null), playerIndexThatPaused);
                }
            }
        }
    }
}
