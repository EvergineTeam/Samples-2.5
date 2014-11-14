using System;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace WaveOculusDemo
{
    public class App : WaveEngine.Adapter.Application
    {
        WaveOculusDemoProject.Game game;
        SpriteBatch spriteBatch;
        Texture2D splashScreen;
        bool splashState = true;
        TimeSpan time;
        Vector2 position;
        Color backgroundSplashColor;

        public App()
        {
            this.Width = 1280;
            this.Height = 800;
            this.FullScreen = false;
            this.WindowTitle = "WaveOculusDemo";
        }

        public override void Initialize()
        {
            this.game = new WaveOculusDemoProject.Game();
            this.game.Initialize(this);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (WaveServices.Input.KeyboardState.F10 == ButtonState.Pressed)
                {
                    this.FullScreen = !this.FullScreen;
                }

                if (WaveServices.Input.KeyboardState.Escape == ButtonState.Pressed)
                {
                    WaveServices.Platform.Exit();
                }
                else
                {
                    this.game.UpdateFrame(elapsedTime);
                }
            }
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                this.game.DrawFrame(elapsedTime);
            }
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        public override void OnActivated()
        {
            base.OnActivated();
            if (this.game != null)
            {
                game.OnActivated();
            }
        }

        /// <summary>
        /// Called when [deactivate].
        /// </summary>
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            if (this.game != null)
            {
                game.OnDeactivated();
            }
        }
    }
}

