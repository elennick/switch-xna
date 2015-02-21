using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using Switch.GameObjects.Challenges;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.Menus
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        private Challenge challenge;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(Challenge challenge)
            : base("PAUSED")
        {
            // Flag that there is no need for the game to transition
            // off when the pause menu is on top of it.
            VibrationManager.Instance.cancelAllVibrations();
            IsPopup = true;
            this.challenge = challenge;
            this.setShowBackgroundColor(false);
            this.setShowBackgroundDecoration(false);

            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry challengeInfoEntry = new MenuEntry("Current Challenge Info");
            MenuEntry controlsEntry = new MenuEntry("Controls");
            MenuEntry helpEntry = new MenuEntry("Game Help");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
            
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            challengeInfoEntry.Selected += ChallengeInfoEntrySelected;
            controlsEntry.Selected += ControlsEntrySelected;
            helpEntry.Selected += HelpEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            if (this.challenge != null)
            {
                MenuEntries.Add(challengeInfoEntry);
            }
            MenuEntries.Add(controlsEntry);
            MenuEntries.Add(helpEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            this.setMenuTitleImage(content.Load<Texture2D>("Sprites\\Title\\paused"));
        }

        void ChallengeInfoEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ChallengeModeMessageBoxScreen challengeInfoEntryScreen = new ChallengeModeMessageBoxScreen(this.challenge.getName(),
                                                                          this.challenge.getDescription(), true);
            ScreenManager.AddScreen(challengeInfoEntryScreen, ControllingPlayer);
        }

        void ControlsEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen(), ControllingPlayer);
        }

        void HelpEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HowToPlayScreen(), ControllingPlayer);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are You Sure You Want To Quit?";

            ExitToMainMenuScreen confirmQuitMessageBox = new ExitToMainMenuScreen(message);
            this.ExitScreen();
            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Draws the pause menu screen. This darkens down the gameplay screen
        /// that is underneath us, and then chains to the base MenuScreen.Draw.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }
    }
}
