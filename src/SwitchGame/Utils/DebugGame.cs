using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Switch.Utils
{
    class DebugGame : Game
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly Exception exception;

        public DebugGame(Exception exception)
        {
            this.exception = exception;
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //System.Diagnostics.Debug.Write(this.exception.StackTrace);
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts\\TinyDebuggingFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || GamePad.GetState(PlayerIndex.Two).Buttons.Back == ButtonState.Pressed
                || GamePad.GetState(PlayerIndex.Three).Buttons.Back == ButtonState.Pressed
                || GamePad.GetState(PlayerIndex.Four).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(
               font,
               "**** CRASH LOG ****",
               new Vector2(50f, 50f),
               Color.White);
            spriteBatch.DrawString(
               font,
               "Press Back to Exit",
               new Vector2(50f, 70f),
               Color.White);
            spriteBatch.DrawString(
               font,
               string.Format("Exception: {0}", exception.Message),
               new Vector2(50f, 90f),
               Color.White);
            spriteBatch.DrawString(
               font, string.Format("Stack Trace:\n{0}", exception.StackTrace),
               new Vector2(50f, 110f),
               Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}