using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Switch.GameObjects
{
    /* This is the superclass for all sprite objects */
    class SpriteObject
    {
        private Texture2D texture;
        private Texture2D backgroundTexture;
        private Dictionary<String, SpriteSheet> animations;
        private SpriteSheet activeAnimation;
        private String activeAnimationName;
        private int activeAnimationFrameChangeTime;
        private int timeSinceLastFrameUpdate;
        private int currentFrame;
        private Rectangle destinationRect;
        private bool loopCurrentAnimation;
        private Color color;

        public SpriteObject(Texture2D texture)
        {
            this.texture = texture;
            animations = new Dictionary<String, SpriteSheet>();
            activeAnimation = null;
            loopCurrentAnimation = false;
            color = Color.White;
        }

        public SpriteObject(Texture2D texture, Texture2D backgroundTexture)
        {
            this.texture = texture;
            this.backgroundTexture = backgroundTexture;
            animations = new Dictionary<String, SpriteSheet>();
            activeAnimation = null;
            loopCurrentAnimation = false;
            color = Color.White;
        }

        public Color getColor()
        {
            return this.color;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public String getCurrentAnimation()
        {
            return activeAnimationName;
        }

        public Rectangle getDestinationRect()
        {
            return this.destinationRect;
        }

        public void setDestinationRect(Rectangle rect)
        {
            this.destinationRect = rect;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public Texture2D getStaticTexture()
        {
            return this.texture;
        }


        public Texture2D getTexture()
        {
            Texture2D textureToReturn;

            if (activeAnimation == null)
            {
                textureToReturn = this.texture;
            }
            else
            {
                textureToReturn = this.activeAnimation.getSpriteSheet();
            }

            return textureToReturn;
        }

        public void setBackgroundTexture(Texture2D texture)
        {
            this.backgroundTexture = texture;
        }

        public Texture2D getBackgroundTexture()
        {
            return this.backgroundTexture;
        }

        public void addAnimation(String animationName, SpriteSheet spriteSheet)
        {
            animations.Add(animationName, spriteSheet);
        }

        public void startAnimation(String animationName, int framesPerSecond, bool looping)
        {
            if (animations.ContainsKey(animationName))
            {
                SpriteSheet animation = animations[animationName];
                activeAnimation = animation;
                activeAnimationName = animationName;
                loopCurrentAnimation = looping;
                activeAnimationFrameChangeTime = (1000 / framesPerSecond);
                currentFrame = 0;
            }
        }

        public void startAnimation(String animationName, int framesPerSecond)
        {
            this.startAnimation(animationName, framesPerSecond, false);
        }

        public virtual void updateGameTime(int elapsedGameTime)
        {
            timeSinceLastFrameUpdate += elapsedGameTime;

            if (activeAnimation != null && timeSinceLastFrameUpdate >= activeAnimationFrameChangeTime)
            {
                currentFrame++;
                timeSinceLastFrameUpdate = 0;
                if (currentFrame >= activeAnimation.getNumberOfFrames())
                {
                    if (!loopCurrentAnimation)
                    {
                        activeAnimation = null;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }
            }
        }

        public virtual void updateGameTime(int elapsedGameTime, int currentGameSpeed, int currentTilePixelHeight)
        {
            this.updateGameTime(elapsedGameTime);
        }

        public void setSpriteSheetDictionary(Dictionary<String, SpriteSheet> animations)
        {
            this.animations = animations;
        }

        public Dictionary<String, SpriteSheet> getSpriteSheetDictionary()
        {
            return this.animations;
        }

        public Rectangle getCurrentCelRect()
        {
            Rectangle rect;

            if (!this.isAnimating())
            {
                rect = new Rectangle(0, 0, this.texture.Width, this.texture.Height);
            }
            else
            {
                int widthOfFrame = activeAnimation.getSpriteSheet().Width / activeAnimation.getNumberOfFrames();
                rect = new Rectangle(currentFrame * widthOfFrame, 0, widthOfFrame, this.texture.Height);
            }

            return rect;
        }

        public bool isAnimating()
        {
            if (activeAnimation == null)
            {
                return false;
            }

            return true;
        }
    }
}
