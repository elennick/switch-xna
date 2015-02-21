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
    abstract class GameDisplay
    {
        protected Vector2 position;
        protected SpriteFont font;
        protected GameBoard gameBoard;

        public GameDisplay(Vector2 position, SpriteFont font, GameBoard gameBoard)
        {
            this.position = position;
            this.font = font;
            this.gameBoard = gameBoard;
        }

        public abstract void draw(SpriteBatch spriteBatch, GameTime gameTime);

        public virtual void update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public void setFont(SpriteFont font)
        {
            this.font = font;
        }

        public SpriteFont getFont()
        {
            return this.font;
        }
    }
}
