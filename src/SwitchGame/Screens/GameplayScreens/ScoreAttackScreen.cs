using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Switch.GameObjects;
using Switch.GameObjects.Tiles;
using Switch.GameObjects.Sound;
using Switch.GameObjects.GameDisplays;
using Switch;
using Switch.Menus;
using Switch.Utils.Difficulty;

namespace Switch
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class ScoreAttackScreen : GameplayScreen
    {
        Random random = new Random();
        private Difficulty difficulty;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScoreAttackScreen(Difficulty difficulty, PlayerIndex playerIndex) : base(1)
        {
            this.difficulty = difficulty;
            base.setPlayerOne(playerIndex);

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override GameMode getGameMode()
        {
            return GameplayScreen.GameMode.SCORE_ATTACK;
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

            //create a new gameboard to use for 1p
            GameBoard gameBoard = new GameBoard(new Vector2(480, 55), TileSet.loadAndGetDefaultTileset(content, difficulty), difficulty, 400, 564, (int)playerIndex1);
            gameBoard.setScaleTiles(true);
            this.addGameplayScreenObject(gameBoard);

            //load all the background imagery
            DetailedSpriteObject infoPanels = new DetailedSpriteObject(content.Load<Texture2D>("Sprites\\BoardComponents\\info_panels_score_attack"),
                                                                            new Rectangle(90, 0, 350, 675));

            this.addGameplayScreenObject(infoPanels);

            //define and load fonts
            SpriteFont weaponDisplayFont = content.Load<SpriteFont>("Fonts\\WeaponsDisplayFont");
            SpriteFont messageBoxDisplayFont = content.Load<SpriteFont>("Fonts\\ScoreMessageBoxFont");
            SpriteFont scoreFont = content.Load<SpriteFont>("Fonts\\PhillySansFont");

            //define and load the displays
            LevelDisplay levelDisplay = new LevelDisplay(new Vector2(385, 106), scoreFont, gameBoard);
            ScoreDisplay scoreDisplay = new ScoreDisplay(new Vector2(200, 106), scoreFont, gameBoard);
            ComplexPowerMeterDisplay powerDisplay = new ComplexPowerMeterDisplay(new Vector2(920, 0), content, messageBoxDisplayFont, gameBoard, false);
            GameMessageBoxDisplay messageBoxDisplay = new GameMessageBoxDisplay(250,
                                                                                300,
                                                                                new Vector2(105, 230),
                                                                                messageBoxDisplayFont,
                                                                                gameBoard,
                                                                                16);

            this.addGameplayScreenObject(levelDisplay);
            this.addGameplayScreenObject(scoreDisplay);
            this.addGameplayScreenObject(powerDisplay);
            this.addGameplayScreenObject(messageBoxDisplay);
        }
    }
}
