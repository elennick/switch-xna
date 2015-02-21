using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Sound;

namespace Switch
{
    class HowToPlayScreen : GameScreen
    {
        ContentManager content;
        List<Texture2D> helpImages;
        Texture2D aButtonTexture;
        Texture2D bButtonTexture;
        int currentPage;
        double textBrightness;

        public HowToPlayScreen()
        {
            helpImages = new List<Texture2D>();
            currentPage = 0;
            textBrightness = 0;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            helpImages.Add(content.Load<Texture2D>("Sprites\\Help\\help-movement"));
            helpImages.Add(content.Load<Texture2D>("Sprites\\Help\\help-scoring"));
            helpImages.Add(content.Load<Texture2D>("Sprites\\Help\\help-2p-overview"));
            helpImages.Add(content.Load<Texture2D>("Sprites\\Help\\help-2p-scoring"));
            helpImages.Add(content.Load<Texture2D>("Sprites\\Help\\help_powers"));

            aButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonA");
            bButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            PlayerIndex playerIndex;

            if (input.IsMenuCancel(null, out playerIndex))
            {
                SoundManager.Instance.playSound("menu-select2");
                ExitScreen();
            }
            else if (input.IsMenuSelect(null, out playerIndex))
            {
                SoundManager.Instance.playSound("menu-select");
                this.nextPage();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            textBrightness = Math.Sin(gameTime.TotalGameTime.Milliseconds * (Math.PI / 512)) * 8;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            SpriteFont bigFont = ScreenManager.BigFont;
            SpriteFont font = ScreenManager.Font;
            SpriteFont littleFont = ScreenManager.LittleFont;

            Vector2 position = new Vector2(0, 0);

            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            //Color yellowColor = new Color(255, 255, 0, (float)(TransitionAlpha * textBrightness));
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

            //draw help image
            int xPixelOffset = (viewport.Width - helpImages[currentPage].Width) / 2;
            spriteBatch.Draw(helpImages[currentPage], new Vector2(position.X + xPixelOffset, 0), titleColor);

            //draw PRESS A TO CONTINUE or PRESS B TO GO BACK text
            Rectangle aButtonRect = new Rectangle((int)position.X + 1100, 600, 32, 32);
            Rectangle bButtonRect = new Rectangle((int)position.X + 1100, 635, 32, 32);

            spriteBatch.Draw(aButtonTexture, aButtonRect, Color.White);
            spriteBatch.Draw(bButtonTexture, bButtonRect, Color.White);

            Vector2 moreScoresPosition = new Vector2(aButtonRect.X + 35, aButtonRect.Y - 5);
            Vector2 backToMenuPosition = new Vector2(bButtonRect.X + 35, bButtonRect.Y - 5);

            spriteBatch.DrawString(littleFont, "Next", moreScoresPosition, yellowColor);
            spriteBatch.DrawString(littleFont, "Back", backToMenuPosition, yellowColor);

            spriteBatch.End();
        }

        public void nextPage()
        {
            currentPage++;

            if (currentPage >= helpImages.Count)
            {
                currentPage = 0;
            }
        }
    }
}
