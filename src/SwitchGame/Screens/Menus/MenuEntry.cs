using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switch.Utils;

namespace Switch.Menus
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    class MenuEntry
    {
        /// <summary>
        /// The text rendered for this entry.
        /// </summary>
        protected string text;
        protected double currentCursorOffset = 0;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        protected float selectionFade;

        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
            {
                Selected(this, new PlayerIndexEventArgs(playerIndex));
            }
        }

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public MenuEntry(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(MenuScreen screen, bool isSelected,
                                                      GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
            {
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            }
            else
            {
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
            }

            //set the current cursor offset... this is just a sin function with the current
            //game time as X... this value is used to make the select cursor(s) bounce back and forth
            currentCursorOffset = Math.Sin(gameTime.TotalGameTime.Milliseconds * (Math.PI / 512)) * 8;
        }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, Vector2 position,
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

            spriteBatch.DrawString(font, text, position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);

            if (isSelected)
            {
                DrawSelectionImages(screen, position, gameTime, text, font);
            }
        }

        public virtual void DrawSelectionImages(MenuScreen screen, Vector2 position, 
                                                GameTime gameTime, String text, SpriteFont font)
        {
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            Texture2D texture = screenManager.SelectorImage;

            Vector2 spriteOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 textSize = font.MeasureString(text);
            Vector2 selectorScale = new Vector2(0.6f, 0.6f);
            int padding = 60;

            Vector2 leftPosition = new Vector2(position.X - (textSize.X / 2) - padding + (int)currentCursorOffset, position.Y);
            float rotationAngleLeft = MathHelper.Pi * 1.5f;

            Vector2 rightPosition = new Vector2(position.X + (textSize.X / 2) + padding - (int)currentCursorOffset, position.Y);
            float rotationAngleRight = MathHelper.Pi * 0.5f;

            spriteBatch.Draw(texture, leftPosition, null, Color.White, rotationAngleLeft, spriteOrigin, selectorScale, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, rightPosition, null, Color.White, rotationAngleRight, spriteOrigin, selectorScale, SpriteEffects.None, 0);
        }

        public virtual void DrawSelectionImageLeft(MenuScreen screen, Vector2 position,
                                                GameTime gameTime, String text, SpriteFont font)
        {
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            Texture2D texture = screenManager.SelectorImage;

            Vector2 spriteOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 textSize = font.MeasureString(text);
            Vector2 selectorScale = new Vector2(0.6f, 0.6f);
            int padding = 60;

            Vector2 leftPosition = new Vector2(position.X - textSize.X - padding + (int)currentCursorOffset, position.Y);
            float rotationAngleLeft = MathHelper.Pi * 1.5f;

            spriteBatch.Draw(texture, leftPosition, null, Color.White, rotationAngleLeft, spriteOrigin, selectorScale, SpriteEffects.None, 0);
        }


        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing - 15;
        }
    }
}
