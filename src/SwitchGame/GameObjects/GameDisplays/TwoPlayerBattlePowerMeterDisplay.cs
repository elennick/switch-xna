using System;
using System.Collections.Generic;
using Switch.GameObjects.GameDisplays;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Switch.GameObjects;
using Switch;
using Switch.Utils;
using Switch.Menus;
using Switch.GameObjects.Sound;

namespace SwitchGame.GameObjects.GameDisplays
{
    class TwoPlayerBattlePowerMeterDisplay : ComplexPowerMeterDisplay
    {
        private GameBoard gameBoard1;
        private GameBoard gameBoard2;
        private int power1;
        private int power2;
        private Texture2D competitionPowerBarLeft;
        private Texture2D competitionPowerBarRight;
        private bool playerTwoAboutToWin;
        private bool playerOneAboutToWin;
        public bool playerTwoWins;
        public bool playerOneWins;
        private float selectionFade;
        private int timeAgoWarningSoundWasLastPlayed;
        private SpriteFont tinyScoreFont;
        private const int MAX_POWER_DIFFERENCE = 62;

        public TwoPlayerBattlePowerMeterDisplay(Vector2 position, ContentManager content, SpriteFont font, GameBoard gameBoard1, GameBoard gameBoard2)
            : base(position, content, font, gameBoard1, false)
        {
            this.gameBoard1 = gameBoard1; //TODO: i should really refactor the GameDisplay super class to be able to handle more than 1 gameboard
            this.gameBoard2 = gameBoard2; //but for now this component will just deal with it itself since it is special and the only one that needs to

            this.powerbarScaffolding = content.Load<Texture2D>("Sprites\\BoardComponents\\twoplayerbattle_powers");
            this.competitionPowerBarLeft = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\2-player-scorebar");
            this.competitionPowerBarRight = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\2-player-scorebar");
            this.font = content.Load<SpriteFont>("Fonts\\WeaponsDisplayFont");
            this.tinyScoreFont = content.Load<SpriteFont>("Fonts\\TinyDebuggingFont");

            this.spriteEffect = SpriteEffects.None;
            this.playerOneAboutToWin = false;
            this.playerTwoAboutToWin = false;
            this.playerOneWins = false;
            this.playerTwoWins = false;
            this.timeAgoWarningSoundWasLastPlayed = 0;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.setPower(this.gameBoard1.getPower(), PlayerIndex.One);
            this.setPower(this.gameBoard2.getPower(), PlayerIndex.Two);

            //draw the background scaffolding
            Rectangle scaffoldingRect = new Rectangle((int)this.position.X, (int)this.position.Y, 250, 675);
            spriteBatch.Draw(this.powerbarScaffolding, scaffoldingRect, Color.White);

            //draw stuff for gameboard 1
            this.drawPowerBar(spriteBatch, this.power1, this.position, false);
            this.drawPowerBar(spriteBatch, this.power2, new Vector2(this.position.X + 220, this.position.Y), true);

            //draw competition bar
            this.drawCompetitionBar(spriteBatch, this.gameBoard1.getScore(), this.gameBoard2.getScore());

            //draw the bullet time power bar
            this.drawBulletTimePowerBar(spriteBatch, this.position, this.gameBoard1, false);
            this.drawBulletTimePowerBar(spriteBatch, new Vector2(this.position.X + 220, this.position.Y), this.gameBoard2, true);

            //draw the "someone is about to win" message (if necessary)
            if (this.playerOneAboutToWin || this.playerTwoAboutToWin)
            {
                if (timeAgoWarningSoundWasLastPlayed >= 10000)
                {
                    SoundManager.Instance.playSound("2p-alarm");
                    timeAgoWarningSoundWasLastPlayed = 0;
                }

                this.drawAboutToWinMessage(spriteBatch, gameTime);
            }
        }

        public override void update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
 	        base.update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            this.setMaxBulletTime(this.gameBoard.getMaxBulletTime());

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            selectionFade = Math.Min(selectionFade + fadeSpeed, 1);

            timeAgoWarningSoundWasLastPlayed += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void setPower(int power, PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                this.power1 = power;
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                this.power2 = power;
            }

            if (this.power1 >= MAX_POWER)
            {
                this.power1 = MAX_POWER;
            }

            if (this.power2 >= MAX_POWER)
            {
                this.power2 = MAX_POWER;
            }
        }

        private void drawBulletTimePowerBar(SpriteBatch spriteBatch, Vector2 thisPosition, GameBoard thisGameBoard, bool flipped)
        {
            float scaleX = (float)thisGameBoard.getBulletTimeLeft() / (float)this.maxBulletTime;

            Vector2 barPosition;
            if (!flipped)
            {
                barPosition = new Vector2(thisPosition.X + 39, thisPosition.Y + 544);
            }
            else
            {
                barPosition = new Vector2(thisPosition.X - 42, thisPosition.Y + 544);
            }

            spriteBatch.Draw(bulletTimeLeftBar,
                             barPosition,
                             null,
                             Color.White,
                             0,
                             new Vector2(0, 2), //origin
                             new Vector2(scaleX * .68f, 1.0f), //scale
                             SpriteEffects.None,
                             0);
        }

        private void drawAboutToWinMessage(SpriteBatch spriteBatch, GameTime gameTime)
        {
            String aboutToWinMessage = "";
            if (this.playerOneAboutToWin)
            {
                aboutToWinMessage += "  Player 1 ";
            }
            else
            {
                aboutToWinMessage += "  Player 2 ";
            }

            aboutToWinMessage += "Is\nAbout To Win!";

            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 0.8f + pulsate * 0.05f * selectionFade;

            Vector2 aboutToWinMsgPosition = new Vector2(640, 175);
            Vector2 aboutToWinMsgOrigin = Utils.Instance.getTextStringCenterOrigin(aboutToWinMessage, font);
            spriteBatch.DrawString(font, aboutToWinMessage, aboutToWinMsgPosition, Color.Yellow, 0, aboutToWinMsgOrigin, scale, SpriteEffects.None, 0);
        }

        private void drawPowerBar(SpriteBatch spriteBatch, int powerLevel, Vector2 powerBarPosition, bool flipped)
        {
            int flippedOffset = 0;
            if (flipped)
            {
                flippedOffset = -80;
            }

            //draw icons
            Rectangle nukeIconRect = new Rectangle((int)(powerBarPosition.X + 36 + flippedOffset), (int)(powerBarPosition.Y + 50), 38, 38);
            if (powerLevel >= POWER_FOR_NUKE)
            {
                spriteBatch.Draw(nukeReady, nukeIconRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(nukeDisabled, nukeIconRect, Color.White);
            }

            Rectangle laserIconRect = new Rectangle((int)(powerBarPosition.X + 36 + flippedOffset), (int)(powerBarPosition.Y + 347), 38, 38);
            if (powerLevel >= POWER_FOR_LASERS)
            {
                spriteBatch.Draw(laserReady, laserIconRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(laserDisabled, laserIconRect, Color.White);
            }

            Rectangle bulletTimeIconRect = new Rectangle((int)(powerBarPosition.X + 36 + flippedOffset), (int)(powerBarPosition.Y + 498), 38, 40);
            if (powerLevel >= POWER_FOR_BULLET_TIME)
            {
                spriteBatch.Draw(clockReady, bulletTimeIconRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(clockDisabled, bulletTimeIconRect, Color.White);
            }

            //draw powerbars
            float twoPlayerScaleX = 0.74f;

            float bulletTimeBarScaleY = 0.96f;
            float scaleY = (float)((MathHelper.Clamp(powerLevel, 0, 25) * 4) / 100) * bulletTimeBarScaleY;
            spriteBatch.Draw(bulletTimePowerBar,
                             new Vector2(powerBarPosition.X + 2, powerBarPosition.Y + 620),
                             null,
                             Color.White,
                             0,
                             new Vector2(0, 132), //origin
                             new Vector2(twoPlayerScaleX, scaleY), //scale
                             SpriteEffects.None,
                             0);

            float laserBarScaleY = 1.03f;
            int amountOfLaserPower = (int)(MathHelper.Clamp(powerLevel, 25, 50) - 25);
            scaleY = (float)((MathHelper.Clamp(amountOfLaserPower, 0, 25) * 4) / 100) * laserBarScaleY;
            spriteBatch.Draw(laserPowerBar,
                             new Vector2(powerBarPosition.X + 2, powerBarPosition.Y + 479),
                             null,
                             Color.White,
                             0,
                             new Vector2(0, 132), //origin
                             new Vector2(twoPlayerScaleX, scaleY), //scale
                             SpriteEffects.None,
                             0);

            float nukeBarBarScaleY = 0.99f;
            int amountOfNukePower = (int)(MathHelper.Clamp(powerLevel, 50, 100) - 50);
            scaleY = (float)((MathHelper.Clamp(amountOfNukePower, 0, 50) * 2) / 100) * nukeBarBarScaleY;
            spriteBatch.Draw(nukePowerBar,
                             new Vector2(powerBarPosition.X + 2, powerBarPosition.Y + 328),
                             null,
                             Color.White,
                             0,
                             new Vector2(0, 285), //origin
                             new Vector2(twoPlayerScaleX, scaleY), //scale
                             SpriteEffects.None,
                             0);

            //draw scores
            String scoreString;
            Vector2 scorePosition;
            Vector2 scoreOrigin;

            if (!flipped)
            {
                scoreString = gameBoard1.getScore().ToString();
                scorePosition = new Vector2(powerBarPosition.X + 50, powerBarPosition.Y + 605);
                scoreOrigin = Utils.Instance.getTextStringLeftOrigin(scoreString, tinyScoreFont);
            }
            else
            {
                scoreString = gameBoard2.getScore().ToString();
                scoreOrigin = Utils.Instance.getTextStringRightOrigin(scoreString, tinyScoreFont);
                scorePosition = new Vector2(powerBarPosition.X - 20, powerBarPosition.Y + 605);
            }

            spriteBatch.DrawString(tinyScoreFont, scoreString, scorePosition, Color.Yellow, 0, scoreOrigin, Vector2.One, SpriteEffects.None, 0);
        }

        private void drawCompetitionBar(SpriteBatch spriteBatch, int powerPlayer1, int powerPlayer2)
        {
            //make sure the score bar hasn't gone too far to either side
            int powerDifference = (int)((powerPlayer2 - powerPlayer1) * .05);
            if (powerDifference >= MAX_POWER_DIFFERENCE)
            {
                powerDifference = MAX_POWER_DIFFERENCE;
                this.playerTwoWins = true;
            }
            else if (powerDifference <= -MAX_POWER_DIFFERENCE)
            {
                powerDifference = -MAX_POWER_DIFFERENCE;
                this.playerOneWins = true;
            }

            //if one player is past 75% of the way to winning, show a message indicating so
            if (powerDifference >= MAX_POWER_DIFFERENCE * 0.75)
            {
                this.playerTwoAboutToWin = true;
            }
            else
            {
                this.playerTwoAboutToWin = false;
            }

            if (powerDifference <= MAX_POWER_DIFFERENCE * -0.75)
            {
                this.playerOneAboutToWin = true;
            }
            else
            {
                this.playerOneAboutToWin = false;
            }

            //draw the actual yellow tug-of-war bar that shows the score comparison
            spriteBatch.Draw(this.competitionPowerBarLeft,
                 new Vector2(this.position.X + 125, this.position.Y + 627),
                 null,
                 Color.White,
                 0,
                 new Vector2(2, 0), //origin
                 new Vector2((powerDifference * -1), 1), //scale
                 SpriteEffects.None,
                 0);

            spriteBatch.Draw(this.competitionPowerBarRight,
                 new Vector2(this.position.X + 127, this.position.Y + 627),
                 null,
                 Color.White,
                 0,
                 new Vector2(0, 0), //origin
                 new Vector2(powerDifference, 1), //scale
                 SpriteEffects.None,
                 0);
        }
    }
}
