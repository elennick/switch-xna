using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switch.GameObjects.Sound;
using Microsoft.Xna.Framework.Content;
using Switch.Utils;

namespace Switch.Menus
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        private List<MenuEntry> menuEntries = new List<MenuEntry>();
        private int selectedEntry = 0;
        private String menuTitle;
        private String subMenuTitleText;
        protected double currentCursorOffset = 0;
        private bool allowBack = true;
        protected ContentManager content;
        private Texture2D menuTitleImage;
        private Texture2D backgroundTexture;
        private Texture2D backgroundDecorationTexture;
        private Texture2D titleBackgroundImage;
        private Texture2D iconImage;
        private bool showBackgroundColor = false;
        private bool showBackgroundDecoration = false;

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(String menuTitle)
        {
            this.menuTitle = menuTitle;
            VibrationManager.Instance.cancelAllVibrations();

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public MenuScreen(Texture2D menuTitleImage)
        {
            this.menuTitleImage = menuTitleImage;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public MenuScreen(String menuTitle, Texture2D icon)
        {
            this.menuTitle = menuTitle;
            this.iconImage = icon;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            backgroundTexture = content.Load<Texture2D>("Sprites\\background-light-black");
            backgroundDecorationTexture = content.Load<Texture2D>("Sprites\\Title\\main-menu-background");
            titleBackgroundImage = content.Load<Texture2D>("Sprites\\Help\\menu_title_background");
        }

        public void setMenuTitleImage(Texture2D texture)
        {
            this.menuTitleImage = texture;
        }

        public void setMenuTitleIconImage(Texture2D texture)
        {
            this.iconImage = texture;
        }

        public void setSubMenuTitleText(String text)
        {
            this.subMenuTitleText = text;
        }

        public void setAllowBack(bool allowBack)
        {
            this.allowBack = allowBack;
        }

        public void setShowBackgroundColor(bool showBackgroundColor)
        {
            this.showBackgroundColor = showBackgroundColor;
        }

        public void setShowBackgroundDecoration(bool showBackgroundDecoration)
        {
            this.showBackgroundDecoration = showBackgroundDecoration;
        }

        public override void HandleInput(InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsMenuUp(ControllingPlayer))
            {
                selectedEntry--;
                SoundManager.Instance.playSound("menu-select");

                if (selectedEntry < 0)
                {
                    selectedEntry = menuEntries.Count - 1;
                }
            }

            // Move to the next menu entry?
            if (input.IsMenuDown(ControllingPlayer))
            {
                selectedEntry++;
                SoundManager.Instance.playSound("menu-select");

                if (selectedEntry >= menuEntries.Count)
                {
                    selectedEntry = 0;
                }
            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                SoundManager.Instance.playSound("menu-select");
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (allowBack && input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                SoundManager.Instance.playSound("menu-select2");
                OnCancel(playerIndex);
            }
        }


        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[selectedEntry].OnSelectEntry(playerIndex);
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }

            //start playing the menu music if it isn't already going
            if (!SoundManager.Instance.isSongPlaying() && !SoundManager.Instance.isMusicPaused())
            {
                SoundManager.Instance.playSong("menu-song");
            }
        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            SpriteFont littleFont = ScreenManager.LittleFont;

            Vector2 position = new Vector2(640, 340);
            if (this.menuTitleImage != null)
            {
                position.Y += 20;
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
            {
                position.X -= transitionOffset * 256;
            }
            else
            {
                position.X += transitionOffset * 512;
            }

            spriteBatch.Begin();

            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            Color greenColor = new Color(50, 205, 50, TransitionAlpha);

            //draw background
            if (this.backgroundTexture != null && this.showBackgroundColor)
            {
                spriteBatch.Draw(this.backgroundTexture, new Rectangle(0, 0, 1280, 720), Color.White);
            }

            //draw background decoration
            if (this.backgroundDecorationTexture != null && this.showBackgroundDecoration)
            {
                Vector2 bgDecTextureOrigin = new Vector2(backgroundDecorationTexture.Width / 2, backgroundDecorationTexture.Height / 2);
                Vector2 bgDecTexturePosition = new Vector2(position.X, 482);
                spriteBatch.Draw(this.backgroundDecorationTexture, bgDecTexturePosition, null, titleColor, 0, bgDecTextureOrigin, Vector2.One, SpriteEffects.None, 0);
            }

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, position, isSelected, gameTime);

                position.Y += menuEntry.GetHeight(this);
            }

            // Draw the menu title.
            Vector2 titlePosition = new Vector2(640, 185);
            titlePosition.Y -= transitionOffset * 100;
            Vector2 titleOrigin;

            if (this.menuTitleImage == null)
            {
                titleOrigin = font.MeasureString(menuTitle) / 2;
                float titleScale = 1.75f;

                spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                       titleOrigin, titleScale, SpriteEffects.None, 0);

                if (this.subMenuTitleText != null)
                {
                    Vector2 subTitleTextPosition = new Vector2(titlePosition.X, titlePosition.Y + 50);
                    Vector2 subTitleTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(subMenuTitleText, littleFont);
                    spriteBatch.DrawString(littleFont, subMenuTitleText, subTitleTextPosition, greenColor, 0,
                       subTitleTextOrigin, 1, SpriteEffects.None, 0);
                }
            }
            else if (this.iconImage == null)
            {
                titleOrigin = new Vector2(menuTitleImage.Width / 2, menuTitleImage.Height / 2);
                spriteBatch.Draw(this.menuTitleImage, titlePosition, null, titleColor, 0, titleOrigin, Vector2.One, SpriteEffects.None, 0);
            }
            else
            {
                titleOrigin = new Vector2(menuTitleImage.Width / 2, menuTitleImage.Height / 2);
                spriteBatch.Draw(this.menuTitleImage, titlePosition, null, titleColor, 0, titleOrigin, Vector2.One, SpriteEffects.None, 0);

                spriteBatch.Draw(this.iconImage, new Vector2(titlePosition.X - 330, titlePosition.Y), null,
                    titleColor, 0, new Vector2(iconImage.Width / 2, iconImage.Height / 2), Vector2.One, SpriteEffects.None, 0);


                titleOrigin = font.MeasureString(menuTitle) / 2;
                float titleScale = 1.30f;
                spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                       titleOrigin, titleScale, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }
    }
}
