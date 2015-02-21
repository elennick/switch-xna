using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switch.Menus;
using Switch.Utils;
using System;
using Microsoft.Xna.Framework.GamerServices;
using EasyStorage;

namespace Switch
{
    public class SwitchGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        public static bool DEBUG_MODE = false;

        public SwitchGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            Components.Add(new GamerServicesComponent(this));
            screenManager.AddScreen(new LogoScreen(), null);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }
    }

    static class Program
    {
        static void Main()
        {
            try
            {
                using (SwitchGame game = new SwitchGame())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                using (DebugGame game = new DebugGame(ex))
                {
                    game.Run();
                }
            }
        }
    }
}
