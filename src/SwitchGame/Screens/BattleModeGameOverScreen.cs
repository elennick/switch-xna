using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using System;
using Switch;
using Switch.GameObjects.Challenges;

namespace Switch.Menus
{
    class BattleModeGameOverScreen : MenuScreen
    {
        private bool acceptingInput;
        private int timeSinceLoaded;
        private int playerIndex1;
        private int playerIndex2;

        public BattleModeGameOverScreen(String message, int playerIndex1, int playerIndex2)
            : base(message)
        {
            VibrationManager.Instance.cancelAllVibrations();

            IsPopup = true;
            this.setShowBackgroundColor(false);
            this.setShowBackgroundDecoration(false);
            this.timeSinceLoaded = 0;
            this.acceptingInput = false;

            this.playerIndex1 = playerIndex1;
            this.playerIndex2 = playerIndex2;

            MenuEntry playAgain = new MenuEntry("Play Again!");
            MenuEntry backToMenu = new MenuEntry("Back To Main Menu");

            playAgain.Selected += playAgainSelected;
            backToMenu.Selected += backToMenuSelected;

            MenuEntries.Add(playAgain);
            MenuEntries.Add(backToMenu);
        }

        void backToMenuSelected(object sender, PlayerIndexEventArgs e)
        {
            if (acceptingInput)
            {
                GameScreen[] screensToLoad = new GameScreen[3];
                screensToLoad[0] = new BackgroundScreen(true);
                screensToLoad[1] = new MainMenuScreen();

                LoadingScreen.Load(ScreenManager, false, null, screensToLoad);
            }
        }

        void playAgainSelected(object sender, PlayerIndexEventArgs e)
        {
            if (acceptingInput)
            {
                GameScreen[] screensToLoad = new GameScreen[3];
                screensToLoad[0] = new BackgroundScreen(true, true);
                screensToLoad[1] = new BattleModeScreen((PlayerIndex)playerIndex1, (PlayerIndex)playerIndex2);

                LoadingScreen.Load(ScreenManager, false, null, screensToLoad);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            VibrationManager.Instance.cancelAllVibrations();
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            timeSinceLoaded += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLoaded > 1000)
            {
                acceptingInput = true;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
