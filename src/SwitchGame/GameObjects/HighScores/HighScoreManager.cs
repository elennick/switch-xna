using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Utils.Difficulty;
using Switch.Utils.Difficulty.DifficultyObjects;

namespace Switch.HighScores
{
    class HighScoreManager
    {
        private static HighScoreManager instance;
        private List<HighScore> highScores;
        public static int MAX_NUMBER_OF_HIGH_SCORES = 9;

        private HighScoreManager() 
        {
            highScores = new List<HighScore>();
        }

        public static HighScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HighScoreManager();
                }
                return instance;
            }
        }

        public List<HighScore> getHighScores(String difficulty) 
        {
            List<HighScore> scoresOfSpecificDifficulty = new List<HighScore>();

            foreach (HighScore highScore in highScores)
            {
                if (highScore.getDifficultyAsString().Equals(difficulty))
                {
                    scoresOfSpecificDifficulty.Add(highScore);
                }
            }

            return scoresOfSpecificDifficulty;
        }

        public void addHighScore(HighScore highScore)
        {
            this.highScores.Add(highScore);
            trimHighScoreList(highScore.getDifficultyAsString());
            StorageManager.Instance.saveHighScores();
        }

        public void setAllHighScores(List<HighScore> highScores)
        {
            this.highScores = highScores;
        }

        public List<HighScore> getAllHighScores()
        {
            return this.highScores;
        }

        private void trimHighScoreList(String difficulty)
        {
            //sort the existing high scores
            List<HighScore> highScoresOfThisDifficulty = getHighScores(difficulty);
            highScoresOfThisDifficulty.Sort();

            //remove any from the list that arent in the top max
            if (highScoresOfThisDifficulty.Count > MAX_NUMBER_OF_HIGH_SCORES)
            {
                for (int i = 0; i < highScoresOfThisDifficulty.Count; i++)
                {
                    if (i >= MAX_NUMBER_OF_HIGH_SCORES)
                    {
                        highScores.Remove(highScoresOfThisDifficulty[i]);
                    }
                }
            }

            //sort the master list
            highScores.Sort();
        }
    }
}
