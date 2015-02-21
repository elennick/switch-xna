using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using System;
using Switch;
using Switch.GameObjects.Challenges;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.Menus
{
    class GameOverBackToMenuScreen : MenuScreen
    {
        private GameplayScreen.GameMode gameMode;
        private long timeSinceLoaded;
        private bool acceptingInput;

        public GameOverBackToMenuScreen(GameplayScreen.GameMode gameMode)
            : base("Game Over")
        {
            IsPopup = true;
            this.gameMode = gameMode;
            this.setAllowBack(false);
            this.setShowBackgroundColor(false);
            this.setShowBackgroundDecoration(false);

            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
            acceptingInput = false;
            timeSinceLoaded = 0;

            MenuEntry backToMenu = new MenuEntry("OK!");

            backToMenu.Selected += backToMenuSelected;

            MenuEntries.Add(backToMenu);

            SoundManager.Instance.playSound("game-over");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            this.setMenuTitleImage(content.Load<Texture2D>("Sprites\\Title\\gameover"));
        }

        void backToMenuSelected(object sender, PlayerIndexEventArgs e)
        {
            GameScreen[] screenToLoad;
            if (this.gameMode == GameplayScreen.GameMode.SCORE_ATTACK)
            {
                screenToLoad = new GameScreen[3];
                screenToLoad[0] = new BackgroundScreen(true);
                screenToLoad[1] = new MainMenuScreen();
                screenToLoad[2] = new ScoreAttackSelectionScreen();
            }
            else if (this.gameMode == GameplayScreen.GameMode.CHALLENGE_MODE)
            {
                screenToLoad = new GameScreen[3];
                screenToLoad[0] = new BackgroundScreen(true);
                screenToLoad[1] = new MainMenuScreen();
                screenToLoad[2] = new ChallengeModeDifficultySelectionScreen();
            }
            else
            {
                screenToLoad = new GameScreen[1];
                screenToLoad[0] = new BackgroundScreen();
            }

            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, screenToLoad);
        }

        public override void Draw(GameTime gameTime)
        {
            VibrationManager.Instance.cancelAllVibrations();
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (timeSinceLoaded > 1000)
            {
                acceptingInput = true;
            }
        }
    }
}
