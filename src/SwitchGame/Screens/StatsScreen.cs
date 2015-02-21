using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Switch.GameObjects;

namespace Switch
{
    class StatsScreen : GameScreen
    {
        ContentManager content;
        Texture2D backgroundTexture;
        Texture2D aButtonImage;
        Texture2D bButtonImage;
        GameboardStats stats;

        public StatsScreen()
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

            backgroundTexture = content.Load<Texture2D>("Sprites\\Title\\wide_blank_background");
            aButtonImage = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonA");
            bButtonImage = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");

            stats = StorageManager.Instance.getGameStats();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont littleFont = ScreenManager.LittleFont;
            SpriteFont font = ScreenManager.Font;
            SpriteFont bigFont = ScreenManager.BigFont;
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            Color yellowColor = new Color(255, 255, 0, TransitionAlpha);
            Vector2 position = new Vector2(0, 0);

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

            //draw background image
            Vector2 backgroundOrigin = new Vector2(backgroundTexture.Width / 2, 0);
            Vector2 backgroundPosition = new Vector2((viewport.Width / 2) + position.X, 0);
            spriteBatch.Draw(backgroundTexture, backgroundPosition, null, titleColor, 0, backgroundOrigin, Vector2.One, SpriteEffects.None, 0);

            //draw title
            String titleString = "Lifetime Game Stats";
            Vector2 titleOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(titleString, font);
            Vector2 titleScale = new Vector2(1.4f, 1.4f);
            Vector2 titlePosition = new Vector2((viewport.Width / 2) + position.X, 120);
            spriteBatch.DrawString(font, titleString, titlePosition, yellowColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

            //draw stats
            List<GameboardStats.StatValue> statValues = stats.getValuesForStatsScreen();

            int i = 0;
            foreach (GameboardStats.StatValue statValue in statValues)
            {
                i++;

                String initials = statValue.name;
                String score = "" + statValue.value;
                int heightOffset = (font.LineSpacing - 12) * i;

                Vector2 scoreOrigin = Utils.Utils.Instance.getTextStringRightOrigin(score, font);
                Vector2 scorePosition = new Vector2(position.X + 980, 170 + heightOffset);

                Vector2 initialsPosition = new Vector2(position.X + 300, 170 + heightOffset);
                Vector2 initialsOrigin = new Vector2(0, scoreOrigin.Y);

                spriteBatch.DrawString(font, initials, initialsPosition, titleColor, 0,
                       initialsOrigin, Vector2.One, SpriteEffects.None, 0);

                spriteBatch.DrawString(font, score, scorePosition, titleColor, 0,
                       scoreOrigin, Vector2.One, SpriteEffects.None, 0);
            }

            //draw button image and text
            int xPixelOffset = (viewport.Width - backgroundTexture.Width) / 2;
            Rectangle bButtonRect = new Rectangle((int)position.X + xPixelOffset + 220, 570, 40, 40);
            spriteBatch.Draw(bButtonImage, bButtonRect, Color.White);
            Vector2 backToMenuPosition = new Vector2(bButtonRect.X + 50, bButtonRect.Y);

            spriteBatch.DrawString(littleFont, "Back To Help & Options", backToMenuPosition, yellowColor);

            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuCancel(null, out playerIndex))
            {
                this.ExitScreen();
            }
            else if (input.IsMenuSelect(null, out playerIndex))
            {

            }
        }
    }
}
