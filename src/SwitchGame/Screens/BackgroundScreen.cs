using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Switch.Utils;
using System.Collections.Generic;
using Switch.Menus;

namespace Switch
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        ContentManager content;
        Texture2D backgroundTexture;
        List<BlurredBackgroundTile> backgroundTiles;
        bool showBackgroundFloaters;
        bool mainMenuLoaded;
        bool tilesAreStatic;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen(bool mainMenuLoaded, bool tilesAreStatic)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.backgroundTiles = new List<BlurredBackgroundTile>();
            this.showBackgroundFloaters = true;
            this.mainMenuLoaded = mainMenuLoaded;
            this.tilesAreStatic = tilesAreStatic;
        }

        public BackgroundScreen(bool mainMenuLoaded) : this(mainMenuLoaded, false) { }

        public BackgroundScreen() : this(false, false) { }

        public void setShowBackgroundFloaters(bool showBackgroundFloaters)
        {
            this.showBackgroundFloaters = showBackgroundFloaters;
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

            Random random = new Random();
            backgroundTiles.Add(new BlurredBackgroundTile(new Vector2(100, 100),
                                                          content.Load<Texture2D>("Sprites\\Title\\BlurredTiles\\blue"),
                                                          100,
                                                          0.1f,
                                                          -1,
                                                          0.005f));

            backgroundTiles.Add(new BlurredBackgroundTile(new Vector2(1100, 200),
                                                          content.Load<Texture2D>("Sprites\\Title\\BlurredTiles\\green"),
                                                          300,
                                                          -0.5f,
                                                          1,
                                                          0.0075f));

            backgroundTiles.Add(new BlurredBackgroundTile(new Vector2(475, 200),
                                                          content.Load<Texture2D>("Sprites\\Title\\BlurredTiles\\orange"),
                                                          1000,
                                                          0.7f,
                                                          1,
                                                          0.0012f));

            backgroundTiles.Add(new BlurredBackgroundTile(new Vector2(650, 450),
                                                          content.Load<Texture2D>("Sprites\\Title\\BlurredTiles\\red"),
                                                          250,
                                                          -0.25f,
                                                          -1,
                                                          0.0015f));

            backgroundTiles.Add(new BlurredBackgroundTile(new Vector2(50, 600),
                                                          content.Load<Texture2D>("Sprites\\Title\\BlurredTiles\\purple"),
                                                          150,
                                                          0.1f,
                                                          -1,
                                                          0.0012f));

            backgroundTiles.Add(new BlurredBackgroundTile(new Vector2(1050, 700),
                                                          content.Load<Texture2D>("Sprites\\Title\\BlurredTiles\\teal"),
                                                          500,
                                                          0.4f,
                                                          -1,
                                                          0.0005f));
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
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

            //update each blurred background piece if they are enabled
            if (this.showBackgroundFloaters)
            {
                foreach (BlurredBackgroundTile tile in backgroundTiles)
                {
                    tile.update(gameTime.ElapsedGameTime.Milliseconds, this.tilesAreStatic);
                }
            }

            if (!mainMenuLoaded)
            {
                if (ControllingPlayer.HasValue)
                {
                    ScreenManager.AddScreen(new MainMenuScreen(), ControllingPlayer);

                }
                else
                {
                    ScreenManager.AddScreen(new PressStartScreen(), null);
                }

                mainMenuLoaded = true;
            }
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin();

            if (this.backgroundTexture != null)
            {
                spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(fade, fade, fade));
            }

            if (this.showBackgroundFloaters)
            {
                foreach (BlurredBackgroundTile tile in backgroundTiles)
                {
                    tile.draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }
}
