using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Sound;

namespace Switch
{
    class BattleModeControllerSelectScreen : GameScreen
    {
        private ContentManager content;
        private PlayerIndex? playerIndex1;
        private PlayerIndex? playerIndex2;
        private Texture2D background;
        private Texture2D topCap, bottomCap;
        private Texture2D startButton;
        private SpriteFont font;
        private Viewport viewport;
        private int middleOfScreenX;
        private Color color, redColor, greenColor;
        private Vector2 position;
        private float selectionFade;
        private float currentPulseScale;

        public BattleModeControllerSelectScreen(PlayerIndex? playerIndex)
        {
            this.playerIndex1 = playerIndex;
            this.playerIndex2 = null;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public BattleModeControllerSelectScreen() : this(null) { }
            
        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            background = content.Load<Texture2D>("Sprites\\Help\\controller_select");
            topCap = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\cap_top");
            bottomCap = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\cap_bottom");
            startButton = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerStart");

            font = ScreenManager.Font;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            PlayerIndex playerIndex;

            //if both players are ready and start is pressed, start the game
            if ((input.IsNewButtonPress(Buttons.Start, playerIndex1, out playerIndex)
                || input.IsNewButtonPress(Buttons.Start, playerIndex2, out playerIndex))
                && playersReady())
            {
                SoundManager.Instance.playSound("player-select");

                GameScreen[] screensToLoad = new GameScreen[2];
                screensToLoad[0] = new BackgroundScreen(true, true);
                screensToLoad[1] = new BattleModeScreen((PlayerIndex)playerIndex1, (PlayerIndex)playerIndex2);

                LoadingScreen.Load(ScreenManager, true, null, screensToLoad);
            }

            //if a player presses A or START, check to see if they are assigned to a index
            //and if not, assign them to one
            if (input.IsNewButtonPress(Buttons.A, null, out playerIndex)
                || input.IsNewButtonPress(Buttons.Start, null, out playerIndex))
            {
                if (playerIndex != playerIndex1 && playerIndex != playerIndex2)
                {
                    assignControllerToPlayer(playerIndex);
                }
            }

            //if a player presses B, back out to the main menu
            if (input.IsNewButtonPress(Buttons.B, null, out playerIndex))
            {
                SoundManager.Instance.playSound("menu-select2");
                this.ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            viewport = ScreenManager.GraphicsDevice.Viewport;
            middleOfScreenX = (viewport.Width / 2);

            //set up colors
            color = new Color(192, 192, 192, TransitionAlpha);
            redColor = new Color(255, 0, 0, TransitionAlpha);
            greenColor = new Color(0, 255, 0, TransitionAlpha);

            //calculate scale for pulsating text/images
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            currentPulseScale = 1 + pulsate * 0.05f * selectionFade;

            //do cool math for making the screen slide into place
            position = new Vector2(0, 0);
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            if (ScreenState == ScreenState.TransitionOn)
            {
                position.X -= transitionOffset * 256;
            }
            else
            {
                position.X += transitionOffset * 512;
            }

            //draw stuffs!
            spriteBatch.Begin();

            //draw background
            Vector2 backgroundOrigin = new Vector2(background.Width / 2, 0);
            Vector2 backgroundPosition = new Vector2(middleOfScreenX + position.X, 0);
            spriteBatch.Draw(background, backgroundPosition, null, color, 0, backgroundOrigin, Vector2.One, SpriteEffects.None, 0);

            //draw player 1 capper image tiles
            Vector2 topCapOrigin1 = new Vector2(topCap.Width / 2, 0);
            Vector2 topCapPosition1 = new Vector2((middleOfScreenX - 175) + position.X, 200);
            spriteBatch.Draw(topCap, topCapPosition1, null, color, 0, topCapOrigin1, Vector2.One, SpriteEffects.None, 0);

            Vector2 bottomCapOrigin1 = new Vector2(bottomCap.Width / 2, 0);
            Vector2 bottomCapPosition1 = new Vector2((middleOfScreenX - 175) + position.X, 400);
            spriteBatch.Draw(bottomCap, bottomCapPosition1, null, color, 0, bottomCapOrigin1, Vector2.One, SpriteEffects.None, 0);

            //draw player 2 capper image tiles
            Vector2 blurredTileOrigin2 = new Vector2(topCap.Width / 2, 0);
            Vector2 blurredTilePosition2 = new Vector2((middleOfScreenX + 175) + position.X, 200);
            spriteBatch.Draw(topCap, blurredTilePosition2, null, color, 0, blurredTileOrigin2, Vector2.One, SpriteEffects.None, 0);

            Vector2 bottomCapOrigin2 = new Vector2(bottomCap.Width / 2, 0);
            Vector2 bottomCapPosition2 = new Vector2((middleOfScreenX + 175) + position.X, 400);
            spriteBatch.Draw(bottomCap, bottomCapPosition2, null, color, 0, bottomCapOrigin2, Vector2.One, SpriteEffects.None, 0);

            //draw player 1 text
            Vector2 playerOneStuffPosition = new Vector2(middleOfScreenX - 175 + position.X, 320);
            if (playerIndex1 == null)
            {
                drawPressStartStuff(spriteBatch, PlayerIndex.One, playerOneStuffPosition);
            }
            else
            {
                drawReadyStuff(spriteBatch, PlayerIndex.One, playerOneStuffPosition);
            }

            //draw player 2 text
            Vector2 playerTwoStuffPosition = new Vector2(middleOfScreenX + 175 + position.X, 320);
            if (playerIndex2 == null)
            {
                drawPressStartStuff(spriteBatch, PlayerIndex.Two, playerTwoStuffPosition);
            }
            else
            {
                drawReadyStuff(spriteBatch, PlayerIndex.Two, playerTwoStuffPosition);
            }

            //draw "press start to play" text if both players are ready
            if(playerIndex1 != null && playerIndex2 != null) 
            {
                String pressStartToPlay = "Press     To Begin!";

                Vector2 pressStartToPlayOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(pressStartToPlay, font);
                Vector2 pressStartToPlayPosition = new Vector2(middleOfScreenX + position.X, 565);
                Vector2 pressStartToPlayScale = new Vector2(1.05f + currentPulseScale, 1.05f + currentPulseScale);

                spriteBatch.DrawString(font, pressStartToPlay, pressStartToPlayPosition, greenColor, 
                    0, pressStartToPlayOrigin, pressStartToPlayScale, SpriteEffects.None, 0);

                Vector2 startButtonImageOrigin = new Vector2(startButton.Width / 2, startButton.Height / 2);
                Vector2 startButtonImagePosition = new Vector2((int)(middleOfScreenX + position.X - 65), 555);
                Vector2 startButtonImageScale = new Vector2(currentPulseScale, currentPulseScale);

                spriteBatch.Draw(startButton, startButtonImagePosition, null, color, 0, startButtonImageOrigin, startButtonImageScale, SpriteEffects.None, 0);
            }

            //done drawing stuffs! yay!
            spriteBatch.End();
        }

        private void assignControllerToPlayer(PlayerIndex playerIndex)
        {
            if (playerIndex1 == null && !controllerIsAlreadyAssigned(playerIndex))
            {
                SoundManager.Instance.playSound("player-select");
                playerIndex1 = playerIndex;
            }
            else if (playerIndex2 == null && !controllerIsAlreadyAssigned(playerIndex))
            {
                SoundManager.Instance.playSound("player-select");
                playerIndex2 = playerIndex;
            }
        }

        private bool controllerIsAlreadyAssigned(PlayerIndex playerIndex)
        {
            if (playerIndex1 == playerIndex || playerIndex2 == playerIndex)
            {
                return true;
            }
                
            return false;
        }

        private bool playersReady()
        {
            if (playerIndex1 != null && playerIndex2 != null)
            {
                return true;
            }
                
            return false;
        }

        private void drawPressStartStuff(SpriteBatch spriteBatch, PlayerIndex playerIndex, Vector2 stuffPosition)
        {
            String playerText = "Player " + playerIndex.ToString();
            String pressStartText = "Press     To Play!";

            Vector2 playerTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(playerText, font);
            Vector2 playerTextPosition = stuffPosition;
            spriteBatch.DrawString(font, playerText, playerTextPosition, redColor, 0, playerTextOrigin, new Vector2(1.2f, 1.2f), SpriteEffects.None, 0);

            Vector2 pressStartTextOrigin = Utils.Utils.Instance.getTextStringCenterOrigin(pressStartText, font);
            Vector2 pressStartTextPosition = new Vector2(stuffPosition.X, stuffPosition.Y + 60);
            spriteBatch.DrawString(font, pressStartText, pressStartTextPosition, color, 0, pressStartTextOrigin, Vector2.One, SpriteEffects.None, 0);

            Vector2 startButtonImageOrigin = new Vector2(startButton.Width / 2, startButton.Height / 2);
            Rectangle startButtonImageRect = new Rectangle((int)(stuffPosition.X - 47), (int)(stuffPosition.Y + 30), 48, 48);
            spriteBatch.Draw(startButton, startButtonImageRect, color);
        }

        private void drawReadyStuff(SpriteBatch spriteBatch, PlayerIndex playerIndex, Vector2 stuffPosition)
        {
            String playerText1 = "Player " + playerIndex.ToString();
            String playerText2 = "Ready!";

            Vector2 playerText1Origin = Utils.Utils.Instance.getTextStringCenterOrigin(playerText1, font);
            Vector2 playerText1Position = stuffPosition;
            spriteBatch.DrawString(font, playerText1, playerText1Position, greenColor, 0, playerText1Origin, new Vector2(1.2f, 1.2f), SpriteEffects.None, 0);

            Vector2 playerText2Origin = Utils.Utils.Instance.getTextStringCenterOrigin(playerText2, font);
            Vector2 playerText2Position = new Vector2(stuffPosition.X, stuffPosition.Y + 60);
            spriteBatch.DrawString(font, playerText2, playerText2Position, color, 0, playerText2Origin, new Vector2(currentPulseScale, currentPulseScale), SpriteEffects.None, 0);
        }
    }
}
