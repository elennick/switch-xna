using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects;

namespace Switch.GameObjects.GameDisplays
{
    class ScoreDisplay : GameDisplay
    {
        private int scoreToDisplay;

        public ScoreDisplay(Vector2 position, SpriteFont font, GameBoard gameBoard)
            : base(position, font, gameBoard)
        {
            this.scoreToDisplay = 0;
        }

        public void updateScore(int score)
        {
            this.scoreToDisplay = score;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.updateScore(this.gameBoard.getScore());
            String scoreLabel = "Score";

            Vector2 labelOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(scoreLabel, this.font);
            spriteBatch.DrawString(this.font, scoreLabel, this.position, new Color(217, 217, 217), 0,
                                   labelOrigin, Vector2.One, SpriteEffects.None, 0);

            String scoreString = "" + scoreToDisplay;
            Vector2 scoreOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(scoreString, this.font);
            Vector2 scoreStringPosition = new Vector2(this.position.X, this.position.Y + font.LineSpacing - 10);
            spriteBatch.DrawString(this.font, scoreString, scoreStringPosition, new Color(217, 217, 217), 0,
                                   scoreOrigin, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
