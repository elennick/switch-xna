using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;

namespace Switch
{
    class CreditsScreen : GameScreen
    {
        ContentManager content;
        Texture2D background;
        Texture2D aButtonTexture;
        Texture2D bButtonTexture;
        Texture2D evansHeadTexture;
        Texture2D kristiansHeadTexture;
        int currentPage;
        int MAX_PAGES = 5;

        public CreditsScreen()
        {
            currentPage = 0;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            background = content.Load<Texture2D>("Sprites\\Help\\credits");
            aButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonA");
            bButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");
            evansHeadTexture = content.Load<Texture2D>("Sprites\\Help\\evan");
            kristiansHeadTexture = content.Load<Texture2D>("Sprites\\Help\\kristian");
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
            int xPixelOffset = (viewport.Width - background.Width) / 2;
            spriteBatch.Draw(background, new Vector2(position.X + xPixelOffset, 0), titleColor);

            //draw PRESS A TO CONTINUE or PRESS B TO GO BACK text
            Rectangle aButtonRect = new Rectangle((int)position.X + xPixelOffset + 40, 570, 40, 40);
            Rectangle bButtonRect = new Rectangle((int)position.X + xPixelOffset + 300, 570, 40, 40);

            spriteBatch.Draw(aButtonTexture, aButtonRect, Color.White);
            spriteBatch.Draw(bButtonTexture, bButtonRect, Color.White);

            Vector2 moreScoresPosition = new Vector2(aButtonRect.X + 50, aButtonRect.Y);
            Vector2 backToMenuPosition = new Vector2(bButtonRect.X + 50, bButtonRect.Y);

            spriteBatch.DrawString(littleFont, "More Credits!", moreScoresPosition, yellowColor);
            spriteBatch.DrawString(littleFont, "Back To Menu", backToMenuPosition, yellowColor);

            if (currentPage == 0)
            {
                //draw evans head
                Vector2 evansHeadOrigin = new Vector2(evansHeadTexture.Width / 2, evansHeadTexture.Height / 2);
                Vector2 evansHeadPosition = new Vector2(position.X + xPixelOffset + 110, 285);
                spriteBatch.Draw(evansHeadTexture, evansHeadPosition, null, titleColor, 0, evansHeadOrigin, Vector2.One, SpriteEffects.None, 0);

                //draw evans text
                String evansRolesText = "DESIGN / CODE";
                String evansNameText = "Evan Lennick";

                Vector2 evansRolesTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(evansRolesText, font);
                Vector2 evansNameTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(evansNameText, font);
                Vector2 evansCreditsTextPosition = new Vector2(position.X + xPixelOffset + 340, evansHeadPosition.Y - 25);

                spriteBatch.DrawString(font, evansRolesText, evansCreditsTextPosition, titleColor, 0, evansRolesTextOrigin, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, evansNameText, new Vector2(evansCreditsTextPosition.X, evansCreditsTextPosition.Y + 50),
                    titleColor, 0, evansNameTextOrigin, 1, SpriteEffects.None, 0);

                //draw kristians head
                Vector2 kristiansHeadOrigin = new Vector2(kristiansHeadTexture.Width / 2, kristiansHeadTexture.Height / 2);
                Vector2 kristiansHeadPosition = new Vector2(position.X + xPixelOffset + 110, 460);
                spriteBatch.Draw(kristiansHeadTexture, kristiansHeadPosition, null, titleColor, 0, kristiansHeadOrigin, Vector2.One, SpriteEffects.None, 0);

                //draw kristians text
                String kristiansRolesText = "DESIGN / ART";
                String kristiansNameText = "Kristian Correa";

                Vector2 kristiansRolesTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(kristiansRolesText, font);
                Vector2 kristiansNameTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(kristiansNameText, font);
                Vector2 kristiansCreditsTextPosition = new Vector2(position.X + xPixelOffset + 340, kristiansHeadPosition.Y - 25);

                spriteBatch.DrawString(font, kristiansRolesText, kristiansCreditsTextPosition, titleColor, 0, kristiansRolesTextOrigin, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, kristiansNameText, new Vector2(kristiansCreditsTextPosition.X, kristiansCreditsTextPosition.Y + 50),
                    titleColor, 0, kristiansNameTextOrigin, 1, SpriteEffects.None, 0);
            }
            else if (currentPage == 1)
            {
                String musicTitleText = "MUSIC";
                String musicCreditsText = "Christian Andersson (menus)\n" +
                                          "  Roald Strauss (gameplay)";

                Vector2 musicTitleTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(musicTitleText, font);
                Vector2 musicTitleTextPosition = new Vector2(position.X + 640, position.Y + 280);
                spriteBatch.DrawString(font, musicTitleText, musicTitleTextPosition, titleColor, 0, musicTitleTextOrigin, Vector2.One, SpriteEffects.None, 0);

                Vector2 musicCreditsTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(musicCreditsText, font);
                Vector2 musicCreditsPosition = new Vector2(position.X + 640, position.Y + 410);
                spriteBatch.DrawString(font, musicCreditsText, musicCreditsPosition, titleColor, 0, musicCreditsTextOrigin, Vector2.One, SpriteEffects.None, 0);
            }
            else if (currentPage == 2)
            {
                String soundsTitleText = "SOUND EFFECTS";
                String soundsCreditsText = "jobro, Koops, ejfortin, wingz\n" +
                                           "DarkoZL, ljudman, HardPCM\n" +
                                           "broumbroum, Hell's Sound Guy\n" +
                                           "simon.rue, RunnerPack\n" +
                                           "       (via Freesound.org)";

                Vector2 soundsTitleTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(soundsTitleText, font);
                Vector2 soundsTitleTextPosition = new Vector2(position.X + 640, position.Y + 220);
                spriteBatch.DrawString(font, soundsTitleText, soundsTitleTextPosition, titleColor, 0, soundsTitleTextOrigin, Vector2.One, SpriteEffects.None, 0);

                Vector2 soundsCreditsTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(soundsCreditsText, font);
                Vector2 soundsCreditsTextPosition = new Vector2(position.X + 640, position.Y + 405);
                spriteBatch.DrawString(font, soundsCreditsText, soundsCreditsTextPosition, titleColor, 0, soundsCreditsTextOrigin, Vector2.One, SpriteEffects.None, 0);
            }
            else if (currentPage == 3)
            {
                String specialThanksTitle = "SPECIAL THANKS";
                String specialThanksCredits = "Natalie, Maureen, Brandon,\n" +
                                               "Tony, Molly and Dessi.";

                Vector2 specialThanksTitleTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(specialThanksTitle, font);
                Vector2 specialThanksTitleTextPosition = new Vector2(position.X + 640, position.Y + 275);
                spriteBatch.DrawString(font, specialThanksTitle, specialThanksTitleTextPosition, titleColor, 0, specialThanksTitleTextOrigin, Vector2.One, SpriteEffects.None, 0);

                Vector2 specialThanksCreditsTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(specialThanksCredits, font);
                Vector2 specialThanksCreditsTextPosition = new Vector2(position.X + 640, position.Y + 425);
                spriteBatch.DrawString(font, specialThanksCredits, specialThanksCreditsTextPosition, titleColor, 0, specialThanksCreditsTextOrigin, Vector2.One, SpriteEffects.None, 0);
            }
            else
            {
                String copyrightWGGString = "(c) Wombat Gathering Games";
                String websiteString = "www.WombatGathering.com";

                Vector2 copyrightTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(copyrightWGGString, font);
                Vector2 copyrightTextPosition = new Vector2(position.X + 640, position.Y + 335);
                spriteBatch.DrawString(font, copyrightWGGString, copyrightTextPosition, titleColor, 0, copyrightTextOrigin, Vector2.One, SpriteEffects.None, 0);

                Vector2 websiteTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(websiteString, font);
                Vector2 websiteTextPosition = new Vector2(position.X + 640, position.Y + 410);
                spriteBatch.DrawString(font, websiteString, websiteTextPosition, titleColor, 0, websiteTextOrigin, Vector2.One, SpriteEffects.None, 0);

            }

            spriteBatch.End();
        }

        public void nextPage()
        {
            currentPage++;

            if (currentPage >= MAX_PAGES)
            {
                currentPage = 0;
            }
        }

        private void drawCurrentPage(int currentPage, SpriteBatch spriteBatch)
        {
            if (currentPage == 0)
            {
                Vector2 evansHeadPosition = new Vector2();
            }
            else if (currentPage == 1)
            {

            }
            else
            {

            }
        }
    }
}
