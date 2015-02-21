using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch;
using Switch.Menus;
using Switch.Utils;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges
{
    interface Challenge
    {
        String getName();
        String getDescription();
        Difficulty getDifficulty();
        bool isCompleted(GameboardStats stats);
        String getStatusText(GameboardStats stats);
        int isSpeedUpEnabled();
        int startingPower();
    }
}
