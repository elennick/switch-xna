using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Switch.GameObjects;
using Switch.GameObjects.Tiles;
using System.Collections.Generic;
using Switch.GameObjects.Challenges;
using Switch.GameObjects.Challenges.ChallengeObjects;
using Switch.GameObjects.Sound;
using Switch.GameObjects.GameDisplays;
using Switch.Menus;
using Switch;

namespace Switch
{
    class ChallengeModeScreen : GameplayScreen
    {
        Random random = new Random();

        public ChallengeModeScreen(Challenge challenge, PlayerIndex playerIndex) : base(1)
        {
            this.challenge = challenge;
            base.setPlayerOne(playerIndex);

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override GameMode getGameMode()
        {
            return GameplayScreen.GameMode.CHALLENGE_MODE;
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            base.LoadContent();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();

            //define and load fonts
            SpriteFont weaponDisplayFont = content.Load<SpriteFont>("Fonts\\WeaponsDisplayFont");
            SpriteFont messageBoxDisplayFont = content.Load<SpriteFont>("Fonts\\ScoreMessageBoxFont");
            SpriteFont scoreFont = content.Load<SpriteFont>("Fonts\\PhillySansFont");

            //create a new gameboard to use for 1p
            GameBoard gameBoard = new GameBoard(new Vector2(480, 55), TileSet.loadAndGetDefaultTileset(content, challenge.getDifficulty()),challenge.getDifficulty(), 400, 564, (int)playerIndex1);
            gameBoard.setScaleTiles(true);
            gameBoard.getStats().power = this.challenge.startingPower();
            this.addGameplayScreenObject(gameBoard);

            //check to see if this challenge wants the board to speed up or stay constant speed
            int speedUpTime = challenge.isSpeedUpEnabled();
            if (speedUpTime > 0)
            {
                gameBoard.setSpeedUpEnabled(true);
                gameBoard.setSpeedUpTimer(speedUpTime);
            }
            else
            {
                gameBoard.setSpeedUpEnabled(false);
            }
            

            //load all the background imagery
            DetailedSpriteObject infoPanels = new DetailedSpriteObject(content.Load<Texture2D>("Sprites\\BoardComponents\\info_panels_challenges"),
                                                                            new Rectangle(90, 0, 350, 675));

            this.addGameplayScreenObject(infoPanels);

            //define and load the displays
            LevelDisplay levelDisplay = new LevelDisplay(new Vector2(385, 75), scoreFont, gameBoard);
            ChallengeStatusDisplay challengeStatusDisplay = new ChallengeStatusDisplay(new Vector2(115, 180), scoreFont, gameBoard, this.challenge);
            ScoreDisplay scoreDisplay = new ScoreDisplay(new Vector2(200, 75), scoreFont, gameBoard);
            ComplexPowerMeterDisplay powerDisplay = new ComplexPowerMeterDisplay(new Vector2(920, 0), content, messageBoxDisplayFont, gameBoard, false);
            GameMessageBoxDisplay messageBoxDisplay = new GameMessageBoxDisplay(250,
                                                                                300,
                                                                                new Vector2(105, 338),
                                                                                messageBoxDisplayFont,
                                                                                gameBoard,
                                                                                12);

            this.addGameplayScreenObject(levelDisplay);
            this.addGameplayScreenObject(challengeStatusDisplay);
            this.addGameplayScreenObject(scoreDisplay);
            this.addGameplayScreenObject(powerDisplay);
            this.addGameplayScreenObject(messageBoxDisplay);
        }
    }
}
