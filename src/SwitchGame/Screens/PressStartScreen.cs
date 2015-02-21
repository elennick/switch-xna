using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switch.Menus;
using Microsoft.Xna.Framework.Content;
using Switch.GameObjects.Sound;
using EasyStorage;
using System.Diagnostics;

namespace Switch
{
    class PressStartScreen : GameScreen
    {
        Texture2D greenTileBackground;
        Texture2D orangeTileBackground;
        Texture2D greenTileForeground;
        Texture2D orangeTileForeground;
        ContentManager content;
        float rotationAngle;
        float selectionFade;
        bool loading;
        bool storageScreenTriggered;

        public PressStartScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            rotationAngle = 0.0f;
            loading = false;
            storageScreenTriggered = false;
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            greenTileBackground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\green");
            orangeTileBackground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\orange");
            greenTileForeground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\crescent");
            orangeTileForeground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\wing_triangle");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Calculate rotation angle based on time elapsed
            float circle = MathHelper.Pi * 2;
            rotationAngle += (elapsed * 3) % circle;

            //to make the text pulsate
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            SpriteFont bigFont = ScreenManager.BigFont;
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);

            Vector2 position = new Vector2(0, 0);

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            if (ScreenState == ScreenState.TransitionOn)
            {
                position.X -= transitionOffset * 256;
            }
            else
            {
                position.X += transitionOffset * 512;
            }

            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * selectionFade;

            spriteBatch.Begin();

            String pressStartString;
            if (!this.loading)
            {
                 pressStartString = "Press Start!";
            }
            else
            {
                 pressStartString = "Loading...";
            }


            Vector2 stringPosition = new Vector2(viewport.Width / 2, viewport.Height / 2);
            Vector2 stringOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(pressStartString, bigFont);
            spriteBatch.DrawString(bigFont, pressStartString, new Vector2(stringPosition.X + position.X, stringPosition.Y), titleColor, 0, stringOrigin, scale, SpriteEffects.None, 0);

            /*
            Vector2 leftImagePosition = new Vector2(stringPosition.X + position.X - 275, stringPosition.Y);
            Vector2 leftImageBackgroundOrigin = new Vector2(greenTileBackground.Width / 2, greenTileBackground.Height / 2);
            spriteBatch.Draw(greenTileBackground, leftImagePosition, null, titleColor, rotationAngle, leftImageBackgroundOrigin, scale, SpriteEffects.None, 0);

            Vector2 leftImageForegroundOrigin = new Vector2(greenTileForeground.Width / 2, greenTileForeground.Height / 2);
            spriteBatch.Draw(greenTileForeground, leftImagePosition, null, titleColor, rotationAngle, leftImageForegroundOrigin, scale, SpriteEffects.None, 0);

            Vector2 rightImagePosition = new Vector2(stringPosition.X + position.X + 275, stringPosition.Y);
            Vector2 rightImageBackgroundOrigin = new Vector2(greenTileBackground.Width / 2, greenTileBackground.Height / 2);
            spriteBatch.Draw(greenTileBackground, rightImagePosition, null, titleColor, rotationAngle, rightImageBackgroundOrigin, scale, SpriteEffects.None, 0);

            Vector2 rightImageForegroundOrigin = new Vector2(greenTileForeground.Width / 2, greenTileForeground.Height / 2);
            spriteBatch.Draw(greenTileForeground, rightImagePosition, null, titleColor, rotationAngle, rightImageForegroundOrigin, scale, SpriteEffects.None, 0);
            */
             
            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            PlayerIndex playerIndex;

            if (input.IsNewButtonPress(Buttons.A, null, out playerIndex)
                || input.IsNewButtonPress(Buttons.B, null, out playerIndex)
                || input.IsNewButtonPress(Buttons.Start, null, out playerIndex))
            {
                SoundManager.Instance.playSound("menu-select2");

#if XBOX
                // get the save device
                try {
                    if(!storageScreenTriggered) {
                        promptForSaveDevice(playerIndex);
                        storageScreenTriggered = true;
                    }
                }
                catch(Exception e) 
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
#endif

#if WINDOWS
                loadMainMenu(playerIndex);
#endif
            }
        }

        private void promptForSaveDevice(PlayerIndex playerIndex)
        {
            SharedSaveDevice sharedSaveDevice = new SharedSaveDevice();
            ScreenManager.Game.Components.Add(sharedSaveDevice);

            // hook an event for when the device is selected
            sharedSaveDevice.DeviceSelected += (s, e) =>
            {
                StorageManager.SaveDevice = (SaveDevice)s;

                //this.loading = true;
                //System.Threading.Thread.Sleep(1500);
                StorageManager.Instance.initialize(playerIndex);
                loadMainMenu(playerIndex);
            };

            // hook two event handlers to force the user to choose a new device if they cancel the
            // device selector or if they disconnect the storage device after selecting it
            sharedSaveDevice.DeviceSelectorCanceled +=
                (s, e) => e.Response = SaveDeviceEventResponse.Force;
            sharedSaveDevice.DeviceDisconnected +=
                (s, e) => e.Response = SaveDeviceEventResponse.Force;

            // prompt for a device on the next Update
            sharedSaveDevice.PromptForDevice();

            // make sure we hold on to the device
            StorageManager.SaveDevice = sharedSaveDevice;
        }

        private void loadMainMenu(PlayerIndex playerIndex)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(), null);
        }
    }
}
