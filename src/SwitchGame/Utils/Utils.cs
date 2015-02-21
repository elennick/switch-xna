using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.Utils
{
    class Utils
    {
        private static Utils instance;

        public static Utils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utils();
                }
                return instance;
            }
        }

        public Vector2 getScreenCenterForText(String text, SpriteFont spriteFont, float yPosition) 
        {
            Vector2 textSize = spriteFont.MeasureString(text);
            float xPosition = ((1280 / 2) - (textSize.X / 2));
            Vector2 textCenter = new Vector2(xPosition, yPosition);

            return textCenter;
        }

        public Vector2 getScreenAlignRightForText(String text, SpriteFont spriteFont, float yPosition)
        {
            Vector2 textSize = spriteFont.MeasureString(text);
            float xPosition = ((1280 / 2) - textSize.X);
            Vector2 textAlignedRight = new Vector2(xPosition, yPosition);

            return textAlignedRight;
        }

        public Vector2 getTextStringCenterOrigin(String text, SpriteFont spriteFont)
        {
            Vector2 textSize = spriteFont.MeasureString(text);

            float xOrigin = textSize.X / 2;
            float yOrigin = textSize.Y / 2;

            return new Vector2(xOrigin, yOrigin);
        }

        public Vector2 getTextStringRightOrigin(String text, SpriteFont spriteFont)
        {
            Vector2 textSize = spriteFont.MeasureString(text);

            float xOrigin = textSize.X;
            float yOrigin = textSize.Y / 2;

            return new Vector2(xOrigin, yOrigin);
        }

        public Vector2 getTextStringLeftOrigin(String text, SpriteFont spriteFont)
        {
            Vector2 textSize = spriteFont.MeasureString(text);

            float xOrigin = 0;
            float yOrigin = textSize.Y / 2;

            return new Vector2(xOrigin, yOrigin);
        }

        public Texture2D copyTexture2D(GraphicsDevice gd, Texture2D image, Rectangle source)
        {
            Rectangle destination = new Rectangle(0, 0, source.Width, source.Height);

            // Create a new render target the size of the cropping region.
            RenderTarget2D target = new RenderTarget2D(gd, source.Width, source.Height, 1, SurfaceFormat.Color);

            // Make it the current render target.
            gd.SetRenderTarget(0, target);

            // Render the selected portion of the source image into the render target.
            SpriteBatch sb = new SpriteBatch(gd);
            sb.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            sb.Draw(image, destination, source, Color.White);
            sb.End();

            // Resolve the render target.  This copies the target's buffer into a texture buffer.
            gd.SetRenderTarget(0, null);

            // Finally, return the target.
            return target.GetTexture();
        }
    }
}
