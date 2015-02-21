using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Switch.GameObjects.Challenges;
using Switch.GameObjects.Challenges.ChallengeObjects;
using Switch.Menus;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Switch.Menus
{
    class ChallengeModeSelectionScreen : MenuScreen
    {
        MenuEntry backMenuEntry;
        Challenge currentlySelectedChallenge;
        private Texture2D uncheckedCheckbox;
        private Texture2D checkedCheckbox;

        public ChallengeModeSelectionScreen(Switch.GameObjects.Challenges.ChallengeManager.ChallengeLevel level)
            : base("Select A Challenge!")
        {
            this.setSubMenuTitleText("Challenges " + ChallengeManager.Instance.getPercentOfChallengesCompleted() + "% Completed");

            List<Challenge> challenges = ChallengeManager.Instance.getChallenges(level);
            foreach (Challenge challenge in challenges)
            {
                ChallengeModeMenuEntry menuEntry = new ChallengeModeMenuEntry(challenge.getName(), 
                                                                              challenge.getDescription(), 
                                                                              ChallengeManager.Instance.getChallengeStatus(challenge.getName()));
                menuEntry.Selected += challengeEntrySelected;
                MenuEntries.Add(menuEntry);
            }

            backMenuEntry = new ExitOrBackMenuEntry("Go Back...");
            backMenuEntry.Selected += OnCancel;
            MenuEntries.Add(backMenuEntry);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            uncheckedCheckbox = content.Load<Texture2D>("Sprites\\checkbox");
            checkedCheckbox = content.Load<Texture2D>("Sprites\\checkbox-checked");

            foreach (MenuEntry menuEntry in MenuEntries)
            {
                //set checkbox image based on completion status
                try
                {
                    ChallengeModeMenuEntry cMenuEntry = (ChallengeModeMenuEntry)menuEntry;
                    if (cMenuEntry.isChallengeCompleted())
                    {
                        cMenuEntry.setImage(checkedCheckbox);
                    }
                    else
                    {
                        cMenuEntry.setImage(uncheckedCheckbox);
                    }
                }
                catch (InvalidCastException ice)
                {
                    //swallow this, it happens when the backMenuEntry is iterated over
                    continue;
                }
            }
        }

        void challengeEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string challengeName = ((MenuEntry)sender).Text;
            currentlySelectedChallenge = ChallengeManager.Instance.getChallengeByName(challengeName);
            string message = "Challenge - " + currentlySelectedChallenge.getName() + "\n\n" + currentlySelectedChallenge.getDescription();

            ChallengeModeMessageBoxScreen confirmExitMessageBox = new ChallengeModeMessageBoxScreen(currentlySelectedChallenge.getName(), 
                                                                          currentlySelectedChallenge.getDescription(), false);
            confirmExitMessageBox.Accepted += ConfirmChallengeSelected;
            ScreenManager.AddScreen(confirmExitMessageBox, ControllingPlayer);
        }

        void ConfirmChallengeSelected(object sender, PlayerIndexEventArgs e)
        {
            GameScreen[] screensToLoad = new GameScreen[2];
            screensToLoad[0] = new BackgroundScreen(true, true);
            screensToLoad[1] = new ChallengeModeScreen(currentlySelectedChallenge, e.PlayerIndex);

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
        }
    }
}
