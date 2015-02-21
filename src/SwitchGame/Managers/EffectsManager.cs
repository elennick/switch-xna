using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Switch;

namespace Switch
{
    class EffectsManager
    {
        private static EffectsManager instance;
        private Dictionary<String, Effect> effects;
        private bool effectsLoaded;

        private EffectsManager()
        {
            effects = new Dictionary<String, Effect>();
            effectsLoaded = false;
        }

        public static EffectsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EffectsManager();
                }
                return instance;
            }
        }

        public void addEffect(String name, Effect effect)
        {
            if (!effects.ContainsKey(name))
            {
                effects.Add(name, effect);
            }
        }

        public Effect getEffect(String name)
        {
            return effects[name];
        }

        public void loadEffects(ContentManager content)
        {
            if (!effectsLoaded)
            {
                addEffect("blur", content.Load<Effect>("Effects\\refraction"));
                addEffect("wave", content.Load<Effect>("Effects\\wave"));

                effectsLoaded = true;
            }
        }

        public void drawBlurEffect(SpriteBatch spriteBatch, 
                            GraphicsDevice graphics, 
                            GameTime gameTime, 
                            Texture2D texture,
                            Rectangle destRect)
        {
            Effect blurEffect = effects["wave"];
            Vector2 position = new Vector2(destRect.X, destRect.Y);

            float scaleX = (float)destRect.Width / (float)texture.Width;
            float scaleY = (float)destRect.Height / (float)texture.Height;
            Vector2 scale = new Vector2(scaleX, scaleY);

            // Begin the sprite batch.
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                              SpriteSortMode.Immediate,
                              SaveStateMode.None);

            // Set the displacement texture.
            graphics.Textures[1] = texture;

            // Set an effect parameter to make the
            // displacement texture scroll in a giant circle.
            blurEffect.Parameters["DisplacementScroll"].SetValue(
                                                        MoveInCircle(gameTime, 1.2f));

            // Begin the custom effect.
            blurEffect.Begin();
            blurEffect.CurrentTechnique.Passes[0].Begin();

            // Draw the sprite.
            //spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            // End the sprite batch, then end our custom effect.
            spriteBatch.End();

            blurEffect.CurrentTechnique.Passes[0].End();
            blurEffect.End();
        }

        Vector2 MoveInCircle(GraphicsDevice graphics, 
                             GameTime gameTime, 
                             Texture2D texture, 
                             float speed,
                             Vector2 position)
        {
            Viewport viewport = graphics.Viewport;

            return MoveInCircle(gameTime, speed) * 128 + position;
        }

        static Vector2 MoveInCircle(GameTime gameTime, float speed)
        {
            double time = gameTime.TotalGameTime.TotalSeconds * speed;

            float x = (float)Math.Cos(time);
            float y = (float)Math.Sin(time);

            return new Vector2(x, y);
        }
    }
}
