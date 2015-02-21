using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.Utils
{
    class BlurredBackgroundTile
    {
        public float rotation { get; set; }
        public int spinDirection { get; set; }
        public int movementSpeed { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public float spinSpeed { get; set; }
        private long millisecondsSinceLastUpdate;
        private Vector2 movementDirection;
        private Vector2 startingPosition;
        private Vector2 currentScale;

        public BlurredBackgroundTile(Vector2 position, Texture2D texture, int movementSpeed, float rotation, int spinDirection, float spinSpeed)
        {
            this.position = position;
            this.texture = texture;
            this.movementSpeed = movementSpeed;
            this.rotation = rotation;
            this.spinDirection = spinDirection;
            this.spinSpeed = spinSpeed;

            this.startingPosition = position;
            this.millisecondsSinceLastUpdate = 0;
            this.currentScale = new Vector2(1.0f, 1.0f);

            Random random = new Random();
            movementDirection = new Vector2(0, 0);
            movementDirection.X = random.Next(2);
            movementDirection.Y = random.Next(2);
        }

        public Vector2 getOrigin()
        {
            return new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void update(long milliseconds, bool tileIsStatic)
        {
            if (!tileIsStatic)
            {
                millisecondsSinceLastUpdate += milliseconds;
                if (millisecondsSinceLastUpdate >= movementSpeed)
                {
                    millisecondsSinceLastUpdate = 0;
                    updatePosition();
                }

                if (spinDirection > 0)
                {
                    rotation += spinSpeed;
                }
                else
                {
                    rotation -= spinSpeed;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, getOrigin(), currentScale, SpriteEffects.None, 0);
        }

        private void updatePosition()
        {
            position = new Vector2(position.X + movementDirection.X, position.Y + movementDirection.Y);

            if (position.X > startingPosition.X + 100 || position.X < startingPosition.X - 100)
            {
                movementDirection = new Vector2(movementDirection.X * -1, movementDirection.Y);
            }

            if (position.Y > startingPosition.Y + 100 || position.Y < startingPosition.Y - 100)
            {
                movementDirection = new Vector2(movementDirection.X, movementDirection.Y * -1);
            }
        }
    }
}
