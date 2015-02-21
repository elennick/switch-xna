using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.Menus
{
    class ChallengeModeMessageBoxScreen : GameScreen
    {
        string message;
        string title;
        bool inGameInfoDisplay;
        Texture2D gradientTexture;
        Texture2D aButtonTexture;
        Texture2D bButtonTexture;
        Texture2D background;
        private const int buttonImageWidth = 40;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public ChallengeModeMessageBoxScreen(String title, String message, bool inGameInfoDisplay)
        {
            this.title = title;
            this.message = message;
            this.inGameInfoDisplay = inGameInfoDisplay;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
            IsPopup = true;
        }

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("gradient");
            aButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonA");
            bButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");
            background = content.Load<Texture2D>("Sprites\\Title\\challenge-background");
        }

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }

                ExitScreen();
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                {
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                }

                ExitScreen();
            }
        }

        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            SpriteFont littleFont = ScreenManager.LittleFont;
            SpriteFont bigFont = ScreenManager.BigFont;
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);

            Vector2 position = new Vector2(0, 0);

            Color whiteColor = new Color(192, 192, 192, TransitionAlpha);
            Color yellowColor = new Color(255, 255, 0, TransitionAlpha);

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

            // draw the background
            Vector2 backgroundPosition = new Vector2(position.X + (viewport.Width / 2), 0);
            Vector2 backgroundOrigin = new Vector2(background.Width / 2, 0);
            spriteBatch.Draw(background, backgroundPosition, null, whiteColor, 0, backgroundOrigin, Vector2.One, SpriteEffects.None, 0);

            // draw the title
            Vector2 titleStringPosition = Utils.Utils.Instance.getScreenCenterForText(title, font, 185);
            titleStringPosition = new Vector2(titleStringPosition.X + position.X, titleStringPosition.Y);

            spriteBatch.DrawString(font,
                                   title,
                                   titleStringPosition,
                                   yellowColor);

            // draw the description
            Vector2 messagePosition = new Vector2(position.X + 300,
                                                  260);
            spriteBatch.DrawString(littleFont,
                                   message,
                                   messagePosition,
                                   whiteColor);

            // draw the buttons
            String acceptText;
            int acceptButtonYOffset = 0;
            if(this.inGameInfoDisplay) {
                acceptText = "CONTINUE";
                acceptButtonYOffset = 50;
            }
            else {
                acceptText = "ACCEPT CHALLENGE!";
            }

            Vector2 acceptTextPosition = Utils.Utils.Instance.getScreenCenterForText(acceptText,
                                                                               font,
                                                                               500 + acceptButtonYOffset);
            acceptTextPosition.X += (position.X + 35);

            spriteBatch.DrawString(font,
                       acceptText,
                       acceptTextPosition,
                       whiteColor);

            Rectangle aButtonRect = new Rectangle((int)(acceptTextPosition.X - 55),
                                                  (int)acceptTextPosition.Y + 10,
                                                  40,
                                                  40);

            spriteBatch.Draw(aButtonTexture, aButtonRect, whiteColor);

            if (!this.inGameInfoDisplay)
            {
                String denyText = "NO THANKS...";
                Vector2 denyTextPosition = Utils.Utils.Instance.getScreenCenterForText(denyText,
                                                                                       font,
                                                                                       550);
                denyTextPosition.X += (position.X + 35);

                spriteBatch.DrawString(font,
                           denyText,
                           denyTextPosition,
                           whiteColor);

                Rectangle bButtonRect = new Rectangle((int)(denyTextPosition.X - 55),
                                                      (int)denyTextPosition.Y + 10,
                                                      40,
                                                      40);

                spriteBatch.Draw(bButtonTexture, bButtonRect, whiteColor);
            }

            spriteBatch.End();
        }
    }
}
