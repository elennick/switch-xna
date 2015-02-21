using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Sound;

namespace Switch
{
    class ControlsScreen : GameScreen
    {
        ContentManager content;
        Texture2D controllerImage;
        Texture2D bButtonImage;
        SpriteFont font;

        public ControlsScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            controllerImage = content.Load<Texture2D>("Sprites\\Help\\help_controls");
            bButtonImage = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");

            font = content.Load<SpriteFont>("Fonts\\ScoreMessageBoxFont");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            Vector2 position = new Vector2(0, 0);
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            if (ScreenState == ScreenState.TransitionOn)
            {
                position.X -= transitionOffset * 256;
            }
            else
            {
                position.X += transitionOffset * 512;
            }

            spriteBatch.Begin();

            spriteBatch.Draw(controllerImage, new Vector2(position.X + 98, -10), titleColor);

            spriteBatch.DrawString(font, "Press      To Go Back", new Vector2(position.X + 930, 575), titleColor);
            spriteBatch.Draw(bButtonImage, new Rectangle((int)position.X + 992, 578, 32, 32), titleColor);

            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            PlayerIndex playerIndex;

            if (input.IsMenuSelect(null, out playerIndex) || input.IsMenuCancel(null, out playerIndex))
            {
                SoundManager.Instance.playSound("menu-select2");
                ExitScreen();
            }
        }
    }
}
