using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Switch.GameObjects.Tiles
{
    class Rotater : SpriteObject
    {
        private int horizontalPosition;
        private int minHorPos;
        private int maxHorPos;

        public Rotater(Texture2D texture) : base(texture)
        {
            
        }

        public Rotater(Rotater rotater, 
                       int minimumHorizontalPosition, 
                       int maximumHorizontalPosition,
                       int initHorizontalPosition)
                            : base(rotater.getTexture())
        {
            this.minHorPos = minimumHorizontalPosition;
            this.maxHorPos = maximumHorizontalPosition;
            this.horizontalPosition = initHorizontalPosition;
            this.setSpriteSheetDictionary(rotater.getSpriteSheetDictionary());
        }

        public int getHorizontalPosition()
        {
            return this.horizontalPosition;
        }

        public void setHorizontalPosition(int horizontalPosition)
        {
            this.horizontalPosition = horizontalPosition;
        }

        public void moveRight()
        {
            if (this.horizontalPosition < this.maxHorPos)
            {
                this.horizontalPosition++;
            }
        }

        public void moveLeft()
        {
            if (this.horizontalPosition > this.minHorPos)
            {
                this.horizontalPosition--;
            }
        }
    }
}
