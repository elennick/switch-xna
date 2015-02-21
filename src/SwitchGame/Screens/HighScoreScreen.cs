using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Switch.HighScores;
using Switch.Utils.Difficulty.DifficultyObjects;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Switch
{
    class HighScoreScreen : GameScreen
    {
        private List<HighScore> currentlyDisplayedHighScores;
        private String currentlyDisplayedDifficulty;
        private ContentManager content;
        private Texture2D aButtonTexture;
        private Texture2D bButtonTexture;
        private Texture2D backgroundTexture;

        public HighScoreScreen()
        {
            currentlyDisplayedDifficulty = (new Easy()).getName();
            currentlyDisplayedHighScores = HighScoreManager.Instance.getHighScores(currentlyDisplayedDifficulty);

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            SpriteFont bigFont = ScreenManager.BigFont;
            SpriteFont font = ScreenManager.Font;
            SpriteFont littleFont = ScreenManager.LittleFont;

            Vector2 position = new Vector2(viewport.Width / 2, 0);

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
            Vector2 backgroundTextureOrigin = new Vector2(backgroundTexture.Width / 2, 0);
            spriteBatch.Draw(backgroundTexture, position, null, titleColor, 0,
                                   backgroundTextureOrigin, Vector2.One, SpriteEffects.None, 0);

            //draw the difficultly being shown right under the title
            String difficultyString = "(" + currentlyDisplayedDifficulty + " Difficulty)";
            Vector2 difficultyStringOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(difficultyString, littleFont);
            Vector2 difficultyStringPosition = new Vector2(position.X, 185);

            spriteBatch.DrawString(littleFont, difficultyString, difficultyStringPosition, yellowColor, 0,
                                   difficultyStringOrigin, Vector2.One, SpriteEffects.None, 0);

            //if there are no scores, print a message saying so
            if (currentlyDisplayedHighScores.Count <= 0)
            {
                String noScores = "No Scores Recorded";
                Vector2 noScoresPosition = new Vector2(position.X, 365);
                Vector2 noScoresOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(noScores, font);

                spriteBatch.DrawString(font, noScores, noScoresPosition, titleColor, 0,
                                        noScoresOrigin, Vector2.One, SpriteEffects.None, 0);
            }

            //show the high scores
            for (int i = 0; i < currentlyDisplayedHighScores.Count; i++)
            {
                HighScore highScore = currentlyDisplayedHighScores[i];

                String initials = highScore.name;
                String score = "" + highScore.score;
                int heightOffset = (font.LineSpacing - 20) * i;

                Vector2 scoreOrigin = Utils.Utils.Instance.getTextStringRightOrigin(score, font);
                Vector2 scorePosition = new Vector2(position.X + 250, 230 + heightOffset);

                Vector2 initialsPosition = new Vector2(position.X - 235, 230 + heightOffset);
                Vector2 initialsOrigin = new Vector2(0, scoreOrigin.Y);

                spriteBatch.DrawString(font, initials, initialsPosition, titleColor, 0,
                       initialsOrigin, Vector2.One, SpriteEffects.None, 0);

                spriteBatch.DrawString(font, score, scorePosition, titleColor, 0,
                       scoreOrigin, Vector2.One, SpriteEffects.None, 0);
            }

            //show the buttons -> A = next list, B = back to main menu
            Rectangle aButtonRect = new Rectangle((int)position.X - 230, 570, 40, 40);
            Rectangle bButtonRect = new Rectangle((int)position.X - 30, 570, 40, 40);

            spriteBatch.Draw(aButtonTexture, aButtonRect, Color.White);
            spriteBatch.Draw(bButtonTexture, bButtonRect, Color.White);

            Vector2 moreScoresPosition = new Vector2(aButtonRect.X + 50, aButtonRect.Y);
            Vector2 backToMenuPosition = new Vector2(bButtonRect.X + 50, bButtonRect.Y);

            spriteBatch.DrawString(littleFont, "More Scores", moreScoresPosition, yellowColor);
            spriteBatch.DrawString(littleFont, "Back To Main Menu", backToMenuPosition, yellowColor);

            spriteBatch.End();
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            aButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonA");
            bButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");
            backgroundTexture = content.Load<Texture2D>("Sprites\\Help\\high_scores");
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            // Look up inputs for the active player profile.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(null, out playerIndex))
            {
                currentlyDisplayedDifficulty = getNextDifficultyLevel(currentlyDisplayedDifficulty);
                currentlyDisplayedHighScores = HighScoreManager.Instance.getHighScores(currentlyDisplayedDifficulty);
            }
            else if (input.IsMenuCancel(null, out playerIndex))
            {
                this.ExitScreen();
            }
        }

        private String getNextDifficultyLevel(String currentDiffLevel)
        {
            String easy = new Easy().getName();
            String normal = new Normal().getName();
            String hard = new Hard().getName();
            String impossible = new Impossible().getName();

            if (currentDiffLevel == easy)
            {
                return normal;
            }
            else if (currentDiffLevel == normal)
            {
                return hard;
            }
            else if (currentDiffLevel == hard)
            {
                return impossible;
            }
            else
            {
                return easy;
            }
        }
    }
}
