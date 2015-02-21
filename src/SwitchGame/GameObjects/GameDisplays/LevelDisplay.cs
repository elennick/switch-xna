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
    class LevelDisplay : GameDisplay
    {
        public LevelDisplay(Vector2 position, SpriteFont font, GameBoard gameBoard)
            : base(position, font, gameBoard)
        {

        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            String lvlLabel = "Level";
            Vector2 labelOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(lvlLabel, this.font);
            spriteBatch.DrawString(this.font, lvlLabel, this.position, new Color(217, 217, 217), 0,
                                   labelOrigin, Vector2.One, SpriteEffects.None, 0);

            String lvlValueString = "" + this.gameBoard.getStats().level;
            Vector2 lvlValueStringOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(lvlValueString, this.font);
            Vector2 lvlvalueStringPosition = new Vector2(this.position.X, this.position.Y + font.LineSpacing - 10);
            spriteBatch.DrawString(this.font, lvlValueString, lvlvalueStringPosition, new Color(217, 217, 217), 0,
                                   lvlValueStringOrigin, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
