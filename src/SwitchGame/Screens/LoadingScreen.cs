using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Switch.GameObjects.Sound;
using Microsoft.Xna.Framework.Content;

namespace Switch
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    class LoadingScreen : GameScreen
    {
        bool loadingIsSlow;
        bool otherScreensAreGone;
        bool showLoadingImage;
        ContentManager content;
        Texture2D loadingImage;
        Texture2D backgroundImage;
        float scale;

        GameScreen[] screensToLoad;

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;
            this.showLoadingImage = true;
            this.scale = 0.7f;
            
            TransitionOnTime = TimeSpan.FromSeconds(0.75);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer,
                                params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
            {
                screen.ExitScreen();
            }

            // Create and activate the loading screen.
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                                            loadingIsSlow,
                                                            screensToLoad);

            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            loadingImage = content.Load<Texture2D>("Sprites\\loading");
            backgroundImage = content.Load<Texture2D>("blank");
        }

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (otherScreensAreGone)
            {
                System.Threading.Thread.Sleep(500);
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Color color = new Color(255, 255, 255, TransitionAlpha);

                float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
                if (ScreenState == ScreenState.TransitionOn)
                {
                    scale += (transitionOffset * 0.025f);
                }

                spriteBatch.Begin();

                if (showLoadingImage)
                {
                    Vector2 loadingImageOrigin = new Vector2(loadingImage.Width / 2, loadingImage.Height / 2);
                    Rectangle loadingImageRect = new Rectangle(viewport.Width / 2, viewport.Height / 2, (int)(539 * scale), (int)(164 * scale));
                    Rectangle backgroundBlackRect = new Rectangle(0, 0, viewport.Width, viewport.Height);

                    spriteBatch.Draw(backgroundImage, backgroundBlackRect, Color.Black);
                    spriteBatch.Draw(loadingImage, loadingImageRect, null, color, 0, loadingImageOrigin, SpriteEffects.None, 0);
                }
                else
                {
                    String message = "Loading...";
                    Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                    Vector2 textSize = font.MeasureString(message);
                    Vector2 textPosition = (viewportSize - textSize) / 2;

                    spriteBatch.DrawString(font, message, textPosition, color);
                }
                
                spriteBatch.End();
            }
        }
    }
}
