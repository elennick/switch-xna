using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Utils.Difficulty;
using Switch.Utils.Difficulty.DifficultyObjects;

namespace Switch.HighScores
{
    public class HighScore : IComparable
    {
        public String name { get; set; }
        public int score { get; set; }
        public Difficulty difficulty { get; set; }

        public HighScore(String name, int score, Difficulty difficulty)
        {
            this.name = name;
            this.score = score;
            this.difficulty = difficulty;
        }

        public HighScore(String name, int score, String difficultyString)
        {
            this.name = name;
            this.score = score;

            if (difficultyString == "Easy")
            {
                this.difficulty = new Easy();
            }
            else if (difficultyString == "Normal")
            {
                this.difficulty = new Normal();
            }
            else if (difficultyString == "Hard")
            {
                this.difficulty = new Hard();
            }
            else
            {
                this.difficulty = new Impossible();
            }
        }

        public String getDifficultyAsString()
        {
            return difficulty.getName();
        }

        public int CompareTo(object obj)
        {
            HighScore highScoreToCompareTo = (HighScore)obj;

            if (this.score > highScoreToCompareTo.score)
            {
                return -1;
            }
            else if (this.score == highScoreToCompareTo.score)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
