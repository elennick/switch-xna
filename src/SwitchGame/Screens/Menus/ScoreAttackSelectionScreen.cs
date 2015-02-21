using Microsoft.Xna.Framework;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Microsoft.Xna.Framework.GamerServices;
using Switch.Screens;

namespace Switch.Menus
{
    class ScoreAttackSelectionScreen : MenuScreen
    {
        MenuEntry easyEntry;
        MenuEntry normalEntry;
        MenuEntry hardEntry;
        MenuEntry impossibleEntry;
        MenuEntry backMenuEntry;

        public ScoreAttackSelectionScreen()
            : base("Score Attack Difficulty")
        {
            this.setSubMenuTitleText("Go For The High Score!");

            easyEntry = new MenuEntry("Easy");
            normalEntry = new MenuEntry("Normal");
            hardEntry = new MenuEntry("Hard");
            impossibleEntry = new MenuEntry("Impossible");
            backMenuEntry = new ExitOrBackMenuEntry("Back To Main Menu...");

            easyEntry.Selected += easyEntrySelected;
            normalEntry.Selected += normalEntrySelected;
            hardEntry.Selected += hardEntrySelected;
            impossibleEntry.Selected += impossibleEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(easyEntry);
            MenuEntries.Add(normalEntry);
            MenuEntries.Add(hardEntry);
            MenuEntries.Add(impossibleEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void easyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Easy easyDiff = new Easy();

            GameScreen[] screensToLoad = new GameScreen[2];
            screensToLoad[0] = new BackgroundScreen(true, true);
            screensToLoad[1] = new ScoreAttackScreen(easyDiff, e.PlayerIndex);

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
        }

        void normalEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (!Guide.IsTrialMode)
            {
                Normal normalDiff = new Normal();

                GameScreen[] screensToLoad = new GameScreen[2];
                screensToLoad[0] = new BackgroundScreen(true, true);
                screensToLoad[1] = new ScoreAttackScreen(normalDiff, e.PlayerIndex);

                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
            }
            else
            {
                ScreenManager.AddScreen(new TrialModeErrorScreen(), null);
            }
        }

        void hardEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (!Guide.IsTrialMode)
            {
                Hard hardDiff = new Hard();

                GameScreen[] screensToLoad = new GameScreen[2];
                screensToLoad[0] = new BackgroundScreen(true, true);
                screensToLoad[1] = new ScoreAttackScreen(hardDiff, e.PlayerIndex);

                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
            }
            else
            {
                ScreenManager.AddScreen(new TrialModeErrorScreen(), null);
            }
        }

        void impossibleEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (!Guide.IsTrialMode)
            {
                Impossible impDiff = new Impossible();

                GameScreen[] screensToLoad = new GameScreen[2];
                screensToLoad[0] = new BackgroundScreen(true, true);
                screensToLoad[1] = new ScoreAttackScreen(impDiff, e.PlayerIndex);

                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
            }
            else
            {
                ScreenManager.AddScreen(new TrialModeErrorScreen(), null);
            }
        }
    }
}
