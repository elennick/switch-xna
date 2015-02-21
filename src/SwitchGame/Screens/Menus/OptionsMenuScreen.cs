using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using Switch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Switch.Menus
{
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry rumbleOnEntry;
        MenuEntry musicOnEntry;
        MenuEntry controlsEntry;
        MenuEntry howToPlayEntry;
        MenuEntry credits;
        MenuEntry stats;

        static bool musicOn = true;
        static bool rumbleOn = true;

        public OptionsMenuScreen()
            : base("Help & Options")
        {
            this.setSubMenuTitleText("Configure Your Junk!");

            musicOnEntry = new MenuEntry(string.Empty);
            rumbleOnEntry = new MenuEntry(string.Empty);
            controlsEntry = new MenuEntry("Controls");
            howToPlayEntry = new MenuEntry("How To Play");
            credits = new MenuEntry("Credits");
            stats = new MenuEntry("Game Stats");

            SetMenuEntryText();

            MenuEntry backMenuEntry = new ExitOrBackMenuEntry("Back To Main Menu...");

            rumbleOnEntry.Selected += RumbleOnMenuEntrySelected;
            musicOnEntry.Selected += MusicOnMenuEntrySelected;
            controlsEntry.Selected += ControlsMenuEntrySelected;
            howToPlayEntry.Selected += HowToPlayMenuEntrySelected;
            credits.Selected += CreditsMenuEntrySelected;
            stats.Selected += StatsMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(rumbleOnEntry);
            MenuEntries.Add(musicOnEntry);
            MenuEntries.Add(controlsEntry);
            MenuEntries.Add(howToPlayEntry);
            MenuEntries.Add(credits);
            MenuEntries.Add(stats);
            MenuEntries.Add(backMenuEntry);
        }

        void SetMenuEntryText()
        {
            musicOnEntry.Text = "Music: " + (musicOn ? "ON" : "OFF");
            rumbleOnEntry.Text = "Rumble: " + (rumbleOn ? "ON" : "OFF");
        }

        void MusicOnMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicOn = !musicOn;
            SetMenuEntryText();
            SoundManager.Instance.setMusicEnabled(musicOn);
        }

        void RumbleOnMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            rumbleOn = !rumbleOn;
            SetMenuEntryText();
            VibrationManager.Instance.setVibrationEnabled(rumbleOn);
        }

        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen(), null);
        }

        void HowToPlayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HowToPlayScreen(), null);
        }

        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen(), null);
        }

        void StatsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new StatsScreen(), null);
        }
    }
}
