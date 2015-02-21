using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.GameObjects.Challenges
{
    [Serializable()] 
    class ChallengeSaveData
    {
        private String challengeName;
        private bool isChallengeCompleted;

        public ChallengeSaveData(String name, bool isCompleted)
        {
            this.challengeName = name;
            this.isChallengeCompleted = isCompleted;
        }

        public ChallengeSaveData(Challenge challenge, bool isCompleted)
        {
            this.challengeName = challenge.getName();
            this.isChallengeCompleted = isCompleted;
        }

        public String ChallengeName
        {
            get { return challengeName; }
            set { challengeName = value; }
        }

        public bool IsChallengeCompleted
        {
            get { return isChallengeCompleted; }
            set { isChallengeCompleted = value; }
        }
    }
}
