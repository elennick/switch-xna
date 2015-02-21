using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Switch.Menus;
using Switch.GameObjects.Sound;
using Microsoft.Xna.Framework.Input;

namespace Switch
{
    class LogoScreen : GameScreen
    {
        ContentManager content;
        Texture2D logoImage;
        int elapsedTimeSinceLogoWasShown;
        bool logoSoundPlayed;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LogoScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(1.5);
            elapsedTimeSinceLogoWasShown = 0;
            logoSoundPlayed = false;
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            logoImage = content.Load<Texture2D>("Sprites\\Title\\logo");
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
        }

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            elapsedTimeSinceLogoWasShown += gameTime.ElapsedGameTime.Milliseconds;

            if (!logoSoundPlayed && elapsedTimeSinceLogoWasShown >= 2000)
            {
                SoundManager.Instance.playSound("wombat-growl");
                logoSoundPlayed = true;
            }

            if (elapsedTimeSinceLogoWasShown >= 4500)
            {
                loadMainMenu();
            }
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp(TransitionAlpha, 0, 255));

            Vector2 logoOrigin = new Vector2(logoImage.Width / 2, logoImage.Height / 2);
            Rectangle logoRect = new Rectangle((int)logoOrigin.X, 
                                               (int)logoOrigin.Y + 40, 
                                               (int)(viewport.Width * .4f), 
                                               (int)(viewport.Height * .4f));

            spriteBatch.Begin();

            spriteBatch.Draw(logoImage, logoRect, null, color, 0, logoOrigin, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        private void loadMainMenu()
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen());
        }
    }
}
