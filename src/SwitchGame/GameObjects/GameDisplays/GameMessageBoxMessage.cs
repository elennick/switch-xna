using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.GameObjects.GameDisplays
{
    class GameMessageBoxMessage
    {
        public String message;
        public Color color;

        public GameMessageBoxMessage(String message)
        {
            this.message = message;
            this.color = Color.Black;
        }

        public GameMessageBoxMessage(String message, Color color)
        {
            this.message = message;
            this.color = color;
        }
    }
}
