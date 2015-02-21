using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Microsoft.Xna.Framework.Graphics;
using Switch;
using Microsoft.Xna.Framework;

namespace Switch.Menus
{
    class ExitOrBackMenuEntry : MenuEntry
    {
        public ExitOrBackMenuEntry(String text)
            : base(text)
        {
            this.text = text;
        }

        public override void Draw(MenuScreen screen, Vector2 position,
                         bool isSelected, GameTime gameTime)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Yellow : Color.White;
            //Vector2 scale = isSelected ? new Vector2(1.3f, 1.3f) : Vector2.One;
            Vector2 scale = Vector2.One;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            //float pulsate = (float)Math.Sin(time * 6) + 1;
            //float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade text out during transitions.
            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = Utils.Utils.Instance.getTextStringCenterOrigin(text, font);
            Vector2 loweredPosition = new Vector2(position.X, position.Y + 25);

            spriteBatch.DrawString(font, text, loweredPosition, color, 0,
                                   origin, scale, SpriteEffects.None, 0);

            if (isSelected)
            {
                DrawSelectionImages(screen, loweredPosition, gameTime, text, font);
            }
        }
    }
}
