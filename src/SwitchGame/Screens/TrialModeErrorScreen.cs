using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using Microsoft.Xna.Framework.GamerServices;

namespace Switch.Screens
{
    class TrialModeErrorScreen : GameScreen
    {
        private ContentManager content;
        private Texture2D aButtonImage, bButtonImage, background;

        public TrialModeErrorScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            background = content.Load<Texture2D>("Sprites\\Title\\wide_blank_background");
            aButtonImage = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonA");
            bButtonImage = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Font;
            SpriteFont smallFont = ScreenManager.LittleFont;

            Vector2 position = new Vector2(0, 0);
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
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

            //draw background
            Vector2 backgroundPosition = new Vector2(position.X + (viewport.Width / 2), 0);
            Vector2 backgroundOrigin = new Vector2(background.Width / 2, 0);
            spriteBatch.Draw(background, backgroundPosition, null, titleColor, 0, backgroundOrigin, Vector2.One, SpriteEffects.None, 0);

            //draw title
            String titleString = "Access Denied!";
            Vector2 titleOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(titleString, font);
            Vector2 titleScale = new Vector2(1.3f, 1.3f);
            Vector2 titlePosition = new Vector2((viewport.Width / 2) + position.X, 120);
            spriteBatch.DrawString(font, titleString, titlePosition, yellowColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

            //draw trial explanation text
            String sorryString = "    Sorry, but only EASY single player\n   modes are available until you unlock\nthe full game! Press (A) now to purchase!";
            spriteBatch.DrawString(font, sorryString, new Vector2(position.X + 305, 200), titleColor);

            //draw back button/text
            Vector2 buttonTextScale = new Vector2(1.4f, 1.4f);

            String backString = "Press      To Go Back";
            Vector2 backStringOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(backString, smallFont);
            Vector2 backStringPosition = new Vector2((viewport.Width / 2) + position.X, 575);
            spriteBatch.DrawString(smallFont, backString, backStringPosition, titleColor, 0, backStringOrigin, buttonTextScale, SpriteEffects.None, 0);
            spriteBatch.Draw(bButtonImage, new Rectangle((int)(backStringPosition.X - 63), (int)(backStringPosition.Y - 15), 32, 32), titleColor);

            //draw purchase button/text
            String purchaseString = "Press      To Purchase Full Game";
            Vector2 purchaseStringOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(purchaseString, smallFont);
            Vector2 purchaseStringPosition = new Vector2((viewport.Width / 2) + position.X, 510);
            spriteBatch.DrawString(smallFont, purchaseString, purchaseStringPosition, titleColor, 0, purchaseStringOrigin, buttonTextScale, SpriteEffects.None, 0);
            spriteBatch.Draw(aButtonImage, new Rectangle((int)(purchaseStringPosition.X - 148), (int)(purchaseStringPosition.Y - 15), 32, 32), titleColor);

            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            
            if (input.IsMenuCancel(null, out playerIndex))
            {
                exitScreen();
            }
            else if (input.IsMenuSelect(null, out playerIndex))
            {
                try
                {
                    Guide.ShowMarketplace(playerIndex);
                    exitScreen();
                }
                catch (Exception ex)
                {
                    Guide.ShowSignIn(1, true);
                }
            }
        }

        private void exitScreen()
        {
            SoundManager.Instance.playSound("menu-select2");
            ExitScreen();
        }
    }
}
