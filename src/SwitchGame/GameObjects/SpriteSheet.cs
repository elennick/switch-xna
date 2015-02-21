using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Switch.GameObjects
{
    class SpriteSheet
    {
        private Texture2D spriteSheet;
        private int numberOfFrames;

        public SpriteSheet(Texture2D spriteSheet, int numberOfFrames)
        {
            this.spriteSheet = spriteSheet;
            this.numberOfFrames = numberOfFrames;
        }

        public Texture2D getSpriteSheet()
        {
            return this.spriteSheet;
        }

        public int getNumberOfFrames()
        {
            return this.numberOfFrames;
        }
    }
}
