using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using Switch.Menus;
using Microsoft.Xna.Framework.GamerServices;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Switch.Screens;

namespace Switch.Menus
{
    class MainMenuScreen : MenuScreen
    {
        private bool gameIsInTrialMode;
        private MenuEntry purchase;

        public MainMenuScreen()
            : base("awesome switch logo goes here !")
        {
            this.setShowBackgroundDecoration(false);

            // Create our menu entries.
            MenuEntry scoreAttack = new MenuEntry("1-Player Score Attack");
            MenuEntry challengeMode = new MenuEntry("1-Player Challenges");
            MenuEntry battleMode = new MenuEntry("2-Player Battle");
            MenuEntry highScores = new MenuEntry("High Scores");
            MenuEntry options = new MenuEntry("Help & Options");
            purchase = new MenuEntry("Purchase And Unlock The Full Game");
            MenuEntry exit = new ExitOrBackMenuEntry("Exit");

            // Hook up menu event handlers.
            scoreAttack.Selected += scoreAttackSelected;
            challengeMode.Selected += challengeModeSelected;
            battleMode.Selected += battleModeSelected;
            highScores.Selected += highScoresSelected;
            options.Selected += optionsSelected;
            purchase.Selected += purchaseSelected;
            exit.Selected += exitSelected;

            // Add entries to the menu.
            MenuEntries.Add(scoreAttack);
            MenuEntries.Add(challengeMode);
            MenuEntries.Add(battleMode);
            MenuEntries.Add(highScores);
            MenuEntries.Add(options);
#if XBOX
            if (Guide.IsTrialMode)
            {
                gameIsInTrialMode = Guide.IsTrialMode;
                MenuEntries.Add(purchase);
            }
#endif
            MenuEntries.Add(exit);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            this.setMenuTitleImage(content.Load<Texture2D>("Sprites\\Title\\switch_logo"));
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!Guide.IsTrialMode && gameIsInTrialMode)
            {
                gameIsInTrialMode = false;
                MenuEntries.Remove(purchase);
            }
        }

        void scoreAttackSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ScoreAttackSelectionScreen(), null);
        }

        void battleModeSelected(object sender, PlayerIndexEventArgs e)
        {
            if (!Guide.IsTrialMode)
            {
                ScreenManager.AddScreen(new BattleModeControllerSelectScreen(), null);
            }
            else
            {
                ScreenManager.AddScreen(new TrialModeErrorScreen(), null);
            }
        }

        void challengeModeSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeDifficultySelectionScreen(), null);
        }

        void highScoresSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HighScoreScreen(), null);
        }

        void optionsSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), null);
        }

        void purchaseSelected(object sender, PlayerIndexEventArgs e)
        {
            try
            {
                Guide.ShowMarketplace(e.PlayerIndex);
            }
            catch (Exception ex)
            {
                Guide.ShowSignIn(1, true);
            }
        }

        void exitSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            //override the default method so that nothing happens when B is pressed
            //from the top level main menu here
        }
    }
}
