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
    class WeaponDisplay : GameDisplay
    {
        private String fireText;
        private String availableText;
        private String unavailableText;
        private Texture2D buttonSprite;
        private bool available;
        private int availableAtPower;

        public WeaponDisplay(String fireText, String availableText, String unavailableText, Texture2D buttonSprite, int availableAtPower, Vector2 position, SpriteFont font, GameBoard gameBoard) 
            : base(position, font, gameBoard)
        {
            this.fireText = fireText;
            this.availableText = availableText;
            this.unavailableText = unavailableText;
            this.buttonSprite = buttonSprite;
            this.available = false;
            this.availableAtPower = availableAtPower;
        }

        public String getFireText()
        {
            return this.fireText;
        }

        public String getAvailabeText()
        {
            return this.availableText;
        }

        public String getUnavailableText()
        {
            return this.unavailableText;
        }

        public Texture2D getButtonSprite()
        {
            return this.buttonSprite;
        }

        public bool isAvailable()
        {
            return this.available;
        }

        public void setAvailable(bool available)
        {
            this.available = available;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 buttonImagePosition = new Vector2(this.position.X, this.position.Y + 5);
            Vector2 availabilityTextPosition = new Vector2(this.position.X + 55, this.position.Y + 35);
            Vector2 fireTextPosition = new Vector2(this.position.X + 55, this.position.Y);

            Rectangle buttonRect = new Rectangle();
            buttonRect.X = (int)buttonImagePosition.X;
            buttonRect.Y = (int)buttonImagePosition.Y;
            buttonRect.Width = 50;
            buttonRect.Height = 50;

            spriteBatch.Draw(buttonSprite, buttonRect, Color.White);

            spriteBatch.DrawString(font,
                fireText,
                fireTextPosition,
                Color.Black);

            if (gameBoard.getPower() < this.availableAtPower)
            {
                this.available = false;
                spriteBatch.DrawString(font,
                    unavailableText,
                    availabilityTextPosition,
                    Color.Red);
            }
            else 
            {
                this.available = true;
                spriteBatch.DrawString(font,
                    availableText,
                    availabilityTextPosition,
                    Color.Green);
            }

        }
    }
}
