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
using Switch.Utils.Difficulty.DifficultyObjects;
using SwitchGame.GameObjects.GameDisplays;

namespace Switch
{
    class BattleModeScreen : GameplayScreen
    {
        Random random = new Random();
        private TwoPlayerBattlePowerMeterDisplay powerMeterDisplay;
        private bool gameOver;

        public BattleModeScreen(PlayerIndex playerIndex1, PlayerIndex playerIndex2) : base(2)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            gameOver = false;
            base.setPlayerOne(playerIndex1);
            base.setPlayerTwo(playerIndex2);
        }

        public override GameMode getGameMode()
        {
            return GameplayScreen.GameMode.BATTLE_MODE;
        }

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

            Normal difficulty = new Normal();
            //define and load fonts
            SpriteFont scoreFont = content.Load<SpriteFont>("Fonts\\PhillySansFont");
            SpriteFont weaponDisplayFont = content.Load<SpriteFont>("Fonts\\WeaponsDisplayFont");

            GameBoard gameBoard1 = new GameBoard(new Vector2(100, 55), TileSet.loadAndGetDefaultTileset(content, difficulty), new Normal(), 400, 564, (int)playerIndex1);
            gameBoard1.setScaleTiles(true);
            this.addGameplayScreenObject(gameBoard1);

            GameBoard gameBoard2 = new GameBoard(new Vector2(780, 55), TileSet.loadAndGetDefaultTileset(content, difficulty), new Normal(), 400, 564, (int)playerIndex2);
            gameBoard2.setScaleTiles(true);
            this.addGameplayScreenObject(gameBoard2);

            //create gamedisplays
            powerMeterDisplay = new TwoPlayerBattlePowerMeterDisplay(new Vector2(515, 0),
                                                                      content,
                                                                      weaponDisplayFont,
                                                                      gameBoard1,
                                                                      gameBoard2);

            //load all the background imagery
            this.addGameplayScreenObject(powerMeterDisplay);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (powerMeterDisplay.playerOneWins && !gameOver)
            {
                ScreenManager.AddScreen(new BattleModeGameOverScreen("Player 1 Wins!", (int)playerIndex1, (int)playerIndex2), null);
                gameOver = true;
            }
            else if (powerMeterDisplay.playerTwoWins && !gameOver)
            {
                ScreenManager.AddScreen(new BattleModeGameOverScreen("Player 2 Wins!", (int)playerIndex1, (int)playerIndex2), null);
                gameOver = true;
            }
        }
    }
}
