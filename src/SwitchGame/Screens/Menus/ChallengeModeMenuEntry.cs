using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Switch.Menus
{
    class ChallengeModeMenuEntry : MenuEntry
    {
        private String desc;
        private bool isCompleted;
        private Texture2D image;

        public ChallengeModeMenuEntry(String text, String challengeDesc, bool isCompleted)
            : base(text)
        {
            this.text = text;
            this.desc = challengeDesc;
            this.isCompleted = isCompleted;
        }

        public void setImage(Texture2D image)
        {
            this.image = image;
        }

        public bool isChallengeCompleted()
        {
            return this.isCompleted;
        }

        public override void Draw(MenuScreen screen, Vector2 position,
                                 bool isSelected, GameTime gameTime)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Yellow : Color.White;
            //Vector2 scale = isSelected ? new Vector2(1.3f, 1.3f) : Vector2.One;
            Vector2 scale = Vector2.One;

            // Modify the alpha to fade text out during transitions.
            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            Vector2 finalTextPosition = new Vector2(position.X - 20, position.Y);
            Vector2 rightOrigin = Utils.Utils.Instance.getTextStringRightOrigin(text, font);

            spriteBatch.DrawString(font, text, finalTextPosition, color, 0,
                                   rightOrigin, scale, SpriteEffects.None, 0);

            //draw the completion level of the challenge
            Vector2 completedPosition = new Vector2(position.X + 70, position.Y);
            Vector2 imagePosition = new Vector2(position.X + 20, position.Y - 25);
            String completedText;
            Color completedColor;

            if (this.isCompleted)
            {
                completedText = "COMPLETE";
                completedColor = Color.Green;
            }
            else
            {
                completedText = "INCOMPLETE";
                completedColor = Color.Red;
            }

            if(image != null) {
                spriteBatch.Draw(image, imagePosition, Color.White);
            }

            spriteBatch.DrawString(font, completedText, completedPosition, completedColor, 0,
                       origin, Vector2.One, SpriteEffects.None, 0);

            if (isSelected)
            {
                DrawSelectionImageLeft(screen, finalTextPosition, gameTime, text, font);
            }
        }
    }
}
