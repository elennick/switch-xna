using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.GameObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Switch
{
    class AnimationManager
    {
        private static AnimationManager instance;
        private Dictionary<String, SpriteSheet> animations;
        private List<DetailedSpriteObject> activeAnimations;
        private bool animationsLoaded;

        private AnimationManager()
        {
            animations = new Dictionary<string, SpriteSheet>();
            activeAnimations = new List<DetailedSpriteObject>();
            animationsLoaded = false;
        }

        public static AnimationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnimationManager();
                }
                return instance;
            }
        }

        public void addAnimation(String animationName, SpriteSheet spriteSheet)
        {
            animations.Add(animationName, spriteSheet);
        }

        public void startAnimation(String animationName, int framesPerSecond, Rectangle rect)
        {
            if (animations.ContainsKey(animationName))
            {
                SpriteSheet spriteSheet = animations[animationName];
                DetailedSpriteObject animation = new DetailedSpriteObject(spriteSheet.getSpriteSheet(), new Vector2(rect.X, rect.Y));
                animation.setDestinationRect(rect);

                animation.addAnimation(animationName, spriteSheet);
                animation.startAnimation(animationName, framesPerSecond);
                activeAnimations.Add(animation);
            }
        }

        public void startAnimation(String animationName, int framesPerSecond, Vector2 position)
        {
            if (animations.ContainsKey(animationName))
            {
                SpriteSheet spriteSheet = animations[animationName];
                DetailedSpriteObject animation = new DetailedSpriteObject(spriteSheet.getSpriteSheet(), position);

                animation.addAnimation(animationName, spriteSheet);
                animation.startAnimation(animationName, framesPerSecond);
                activeAnimations.Add(animation);
            }
        }

        public void updateGameTime(int elapsedGameTime)
        {
            foreach (DetailedSpriteObject animation in activeAnimations)
            {
                animation.updateGameTime(elapsedGameTime);
            }
        }

        public void drawAnimations(SpriteBatch spriteBatch)
        {
            //draw any animations that are still going
            foreach (DetailedSpriteObject animation in activeAnimations)
            {
                if (animation.isAnimating())
                {
                    if (animation.getDestinationRect() == null)
                    {
                        spriteBatch.Draw(animation.getTexture(), animation.getPosition(), animation.getCurrentCelRect(), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(animation.getTexture(), animation.getDestinationRect(), animation.getCurrentCelRect(), Color.White);
                    }
                }
            }

            //remove any animations that are done
            DetailedSpriteObject[] activeAnimationsArray = activeAnimations.ToArray();
            for (int i = 0; i < activeAnimationsArray.Length; i++)
            {
                if (!activeAnimationsArray[i].isAnimating())
                {
                    activeAnimations.Remove(activeAnimationsArray[i]);
                }
            }
        }

        public void loadAnimations(ContentManager content)
        {
            if (!animationsLoaded)
            {
                this.addAnimation("laser", new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\laser-spritesheet"), 9));
                this.addAnimation("tile-explode", new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\explosion-spritesheet"), 6));

                animationsLoaded = true;
            }
        }

        public void clearAllAnimations()
        {
            activeAnimations.Clear();
        }

        public bool areAnyAnimationsActive()
        {
            if (activeAnimations.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
