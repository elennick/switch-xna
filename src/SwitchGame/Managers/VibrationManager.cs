using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Switch
{
    class VibrationManager
    {
        private bool vibeEnabled;
        private static VibrationManager instance;
        private Dictionary<PlayerIndex, int> currentVibrations;
        private PlayerIndex[] playerIndexes = { PlayerIndex.One,
                                                PlayerIndex.Two,
                                                PlayerIndex.Three,
                                                PlayerIndex.Four };

        private VibrationManager()
        {
            currentVibrations = new Dictionary<PlayerIndex, int>();
            this.vibeEnabled = true;
        }

        public static VibrationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VibrationManager();
                }
                return instance;
            }
        }

        public void setVibrationEnabled(bool vibeEnabled)
        {
            this.vibeEnabled = vibeEnabled;
        }

        public void vibrateController(PlayerIndex playerIndex, int millisecondsToVibeFor)
        {
            if (currentVibrations.ContainsKey(playerIndex))
            {
                currentVibrations[playerIndex] = millisecondsToVibeFor;
            }
            else
            {
                currentVibrations.Add(playerIndex, millisecondsToVibeFor);
            }
        }

        public void cancelAllVibrations()
        {
            currentVibrations.Clear();
            foreach (PlayerIndex playerIndex in playerIndexes)
            {
                GamePad.SetVibration(playerIndex, 0, 0);
            }
        }

        public void update(int timeElapsed)
        {
            if (!vibeEnabled)
            {
                return;
            }

            foreach (PlayerIndex playerIndex in playerIndexes)
            {
                if (currentVibrations.ContainsKey(playerIndex))
                {
                    int vibeTimeLeft = currentVibrations[playerIndex] - timeElapsed;

                    if (vibeTimeLeft <= 0)
                    {
                        currentVibrations.Remove(playerIndex);
                        GamePad.SetVibration(playerIndex, 0, 0);
                    }
                    else
                    {
                        currentVibrations[playerIndex] = vibeTimeLeft;
                        GamePad.SetVibration(playerIndex, 1, 1);
                    }
                }
                else
                {
                    GamePad.SetVibration(playerIndex, 0, 0);
                }
            }          
        }
    }
}
