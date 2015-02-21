using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyStorage;
using Switch.HighScores;
using Switch.GameObjects.Challenges;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Switch.GameObjects;

namespace Switch
{
    class StorageManager
    {
        public static String SWITCH_CONTAINER_NAME = "Switch";
        public static String HIGH_SCORES_FILE_NAME = "Switch High Scores";
        public static String CHALLENGE_STATUSES_FILE_NAME = "Switch Challenges";
        public static String STATS_FILE_NAME = "Switch Game Stats";
        public static ISaveDevice SaveDevice;
        private static StorageManager instance;
        private bool saveDeviceWasSelected;
        private String playerAccountName;
        private String challengeSaveFileName;
        private GameboardStats gameStats;

        public static StorageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StorageManager();
                }
                return instance;
            }
        }

        private StorageManager()
        {
            gameStats = new GameboardStats();
            saveDeviceWasSelected = false;
            playerAccountName = "Guest";
            setChallengeSaveFileName();
        }

        private void setChallengeSaveFileName()
        {
            challengeSaveFileName = CHALLENGE_STATUSES_FILE_NAME + "" + playerAccountName; 
        }

        public void initialize(PlayerIndex playerIndex)
        {
            //figure out profile name for when we save/load challenge data
            try
            {
                playerAccountName = SignedInGamer.SignedInGamers[(int)playerIndex].Gamertag;
            }
            catch (Exception e)
            {
                playerAccountName = "Guest";
            }
            setChallengeSaveFileName();

            //SaveDevice.Delete(SWITCH_CONTAINER_NAME, CHALLENGE_STATUSES_FILE_NAME);

            //load challenge data
            try
            {
                List<ChallengeSaveData> challengeSaveData = ChallengeManager.Instance.createNewChallengeSaveData();
                if (!SaveDevice.FileExists(SWITCH_CONTAINER_NAME, challengeSaveFileName))
                {
                    saveChallengeStatuses(challengeSaveData);
                }
                else
                {

                        SaveDevice.Load(
                            SWITCH_CONTAINER_NAME,
                            challengeSaveFileName,
                            stream =>
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    while (reader.Peek() >= 0)
                                    {
                                        String challengeName = reader.ReadLine();
                                        bool isChallengeComplete = bool.Parse(reader.ReadLine());
                                        ChallengeManager.Instance.setChallengeCompleteStatus(challengeName, isChallengeComplete);
                                    }
                                }
                            });
                    }
                }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            //load high score data
            try
            {
                if (!SaveDevice.FileExists(SWITCH_CONTAINER_NAME, HIGH_SCORES_FILE_NAME))
                {
                    saveHighScores();
                }
                else
                {
                    SaveDevice.Load(
                        SWITCH_CONTAINER_NAME,
                        HIGH_SCORES_FILE_NAME,
                        stream =>
                        {
                            try
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    List<HighScore> highScores = new List<HighScore>();
                                    while (reader.Peek() >= 0)
                                    {
                                        String name = reader.ReadLine();
                                        int score = int.Parse(reader.ReadLine());
                                        String difficulty = reader.ReadLine();

                                        HighScore highScore = new HighScore(name, score, difficulty);
                                        highScores.Add(highScore);
                                    }
                                    highScores.Sort();
                                    HighScoreManager.Instance.setAllHighScores(highScores);
                                }
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine(e.Message);
                            }
                        });

                }
             }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            //load stats data
            try
            {
                if (!SaveDevice.FileExists(SWITCH_CONTAINER_NAME, STATS_FILE_NAME))
                {
                    saveStatsData(this.gameStats);
                }
                else
                {
                    loadStatsData();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void saveChallengeStatuses(List<ChallengeSaveData> challengeSaveData)
        {
            try
            {
                SaveDevice.Save(
                    SWITCH_CONTAINER_NAME,
                    challengeSaveFileName,
                    stream =>
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            foreach (ChallengeSaveData challenge in challengeSaveData)
                            {
                                writer.WriteLine(challenge.ChallengeName);
                                writer.WriteLine(challenge.IsChallengeCompleted);
                            }
                        }
                    });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void saveHighScores()
        {
            try
            {
                SaveDevice.Save(
                    SWITCH_CONTAINER_NAME,
                    HIGH_SCORES_FILE_NAME,
                    stream =>
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            foreach (HighScore highScore in HighScoreManager.Instance.getAllHighScores())
                            {
                                writer.WriteLine(highScore.name);
                                writer.WriteLine(highScore.score);
                                writer.WriteLine(highScore.getDifficultyAsString());
                            }
                        }
                    });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public GameboardStats getGameStats()
        {
            return gameStats;
        }

        public void addStatsData(GameboardStats stats)
        {
            gameStats.score += stats.score;
            gameStats.numberOfBlocksDestroyed += stats.numberOfBlocksDestroyed;
            gameStats.numberOfBlocksDestroyedByLaser += stats.numberOfBlocksDestroyedByLaser;
            gameStats.numberOfBlocksDestroyedByNuke += stats.numberOfBlocksDestroyedByNuke;
            gameStats.numberOfBulletTimesFired += stats.numberOfBulletTimesFired;
            gameStats.numberOfLasersFired += stats.numberOfLasersFired;
            gameStats.numberOfNukesFired += stats.numberOfNukesFired;
            gameStats.numberOfCapsCompleted += stats.numberOfCapsCompleted;
            gameStats.numberOfMultipliersCapped += stats.numberOfMultipliersCapped;

            saveStatsData(gameStats);
        }

        private void loadStatsData()
        {
            try
            {
                SaveDevice.Load(
                    SWITCH_CONTAINER_NAME,
                    STATS_FILE_NAME,
                    stream =>
                    {
                        try
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                gameStats.score = int.Parse(reader.ReadLine());
                                gameStats.numberOfBlocksDestroyed = int.Parse(reader.ReadLine());
                                gameStats.numberOfBlocksDestroyedByLaser = int.Parse(reader.ReadLine());
                                gameStats.numberOfBlocksDestroyedByNuke = int.Parse(reader.ReadLine());
                                gameStats.numberOfBulletTimesFired = int.Parse(reader.ReadLine());
                                gameStats.numberOfLasersFired = int.Parse(reader.ReadLine());
                                gameStats.numberOfNukesFired = int.Parse(reader.ReadLine());
                                gameStats.numberOfCapsCompleted = int.Parse(reader.ReadLine());
                                gameStats.numberOfMultipliersCapped = int.Parse(reader.ReadLine());
                            }
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                    });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private void saveStatsData(GameboardStats stats)
        {
            try
            {
                SaveDevice.Save(
                    SWITCH_CONTAINER_NAME,
                    STATS_FILE_NAME,
                    stream =>
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.WriteLine(gameStats.score);
                            writer.WriteLine(gameStats.numberOfBlocksDestroyed);
                            writer.WriteLine(gameStats.numberOfBlocksDestroyedByLaser);
                            writer.WriteLine(gameStats.numberOfBlocksDestroyedByNuke);
                            writer.WriteLine(gameStats.numberOfBulletTimesFired);
                            writer.WriteLine(gameStats.numberOfLasersFired);
                            writer.WriteLine(gameStats.numberOfNukesFired);
                            writer.WriteLine(gameStats.numberOfCapsCompleted);
                            writer.WriteLine(gameStats.numberOfMultipliersCapped);
                        }
                    });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
