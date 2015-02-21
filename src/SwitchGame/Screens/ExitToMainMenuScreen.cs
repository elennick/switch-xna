using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using System;

namespace Switch.Menus
{
    class ExitToMainMenuScreen : MenuScreen
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ExitToMainMenuScreen(String menuTitleString)
            : base(menuTitleString)
        {
            IsPopup = true;
            this.setShowBackgroundColor(false);
            this.setShowBackgroundDecoration(false);

            MenuEntry quitGameMenuEntry = new MenuEntry("Yes");
            MenuEntry nevermindGameMenuEntry = new MenuEntry("No");

            nevermindGameMenuEntry.Selected += NevermindGameMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(quitGameMenuEntry);
            MenuEntries.Add(nevermindGameMenuEntry);
        }

        void NevermindGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ExitScreen();
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameScreen[] screenToLoad = new GameScreen[3];
            screenToLoad[0] = new BackgroundScreen(true);
            screenToLoad[1] = new MainMenuScreen();

            LoadingScreen.Load(ScreenManager, false, null, screenToLoad);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }
    }
}
