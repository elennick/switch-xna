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
    class GameMessageBoxDisplay : GameDisplay
    {
        private int pixelWidth;
        private int pixelHeight;
        private List<GameMessageBoxMessage> messageQueue;
        private int maxNumOfMessages;

        public GameMessageBoxDisplay(int pixelWidth, int pixelHeight, Vector2 position, SpriteFont font, GameBoard gameBoard, int maxNumOfMessages)
            : base(position, font, gameBoard)
        {
            this.pixelWidth = pixelWidth;
            this.pixelHeight = pixelHeight;
            this.messageQueue = new List<GameMessageBoxMessage>();
            this.gameBoard.setMessageBoxDisplay(this);
            this.maxNumOfMessages = maxNumOfMessages;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for(int i = 0; i < messageQueue.Count; i++) {
                int msgPosX = (int)(this.position.X + 10);
                int msgPosY = (int)(this.position.Y + 10 + (i * 25));
                Vector2 thisMsgPos = new Vector2(msgPosX, msgPosY);

                GameMessageBoxMessage gameMessage = messageQueue[i];
                spriteBatch.DrawString(font,
                    gameMessage.message,
                    thisMsgPos,
                    gameMessage.color);
            }
        }

        public void addMessage(String message)
        {
            addMessage(message, Color.Black);
        }

        public void addMessage(String message, Color color)
        {
            this.messageQueue.Add(new GameMessageBoxMessage(message, color));

            if (this.messageQueue.Count > maxNumOfMessages)
            {
                this.messageQueue.RemoveAt(0);
            }
        }
    }
}
