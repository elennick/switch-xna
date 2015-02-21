using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.Menus
{
    class ChallengeCompleteScreen : MenuScreen
    {
        public ChallengeCompleteScreen(String menuTitleString)
            : base(menuTitleString)
        {
            IsPopup = true;
            setAllowBack(false);
            setShowBackgroundColor(false);
            
            MenuEntry quitGameMenuEntry = new MenuEntry("Back To Menu");

            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(quitGameMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            this.setMenuTitleImage(content.Load<Texture2D>("Sprites\\Title\\challengecompleted"));
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameScreen[] screensToLoad = new GameScreen[3];

            screensToLoad = new GameScreen[3];
            screensToLoad[0] = new BackgroundScreen(true);
            screensToLoad[1] = new MainMenuScreen();
            screensToLoad[2] = new ChallengeModeDifficultySelectionScreen();

            LoadingScreen.Load(ScreenManager, false, null, screensToLoad);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }
    }
}
