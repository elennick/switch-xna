using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Switch.GameObjects
{
    class DetailedSpriteObject : SpriteObject
    {
        private Vector2 position;
        protected Rectangle destRect;

        public DetailedSpriteObject(Texture2D texture, Vector2 position)
            : base(texture)
        {
            this.position = position;
        }

        public DetailedSpriteObject(Texture2D texture, Rectangle destRect) 
            : base(texture)
        {
            this.destRect = destRect;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public void setDestinationRect(Rectangle rect)
        {
            this.destRect = rect;
        }

        public Rectangle getDestinationRect()
        {
            return this.destRect;
        }
    }
}
