using System;
using System.Collections.Generic;
using WaveEngine.Framework.Services;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;

namespace Lines
{
    public class App : WaveEngine.Adapter.Application
    {
        Lines.Game game;
        SpriteBatch spriteBatch;
        Texture2D splashScreen;
        bool splashState = true;
        TimeSpan time;
        Vector2 position;
        Color backgroundSplashColor;
        bool lastKeyF10Pressed = false;

        public App()
        {
            this.Width = 1280;
            this.Height = 720;
            this.FullScreen = false;
            this.WindowTitle = "Lines";
        }

        public override void Initialize()
        {
            this.game = new Lines.Game();
            this.game.Initialize(this);

            #region DEFAULT SPLASHSCREEN
            this.backgroundSplashColor = Color.White;
            this.spriteBatch = new SpriteBatch(WaveServices.GraphicsDevice);

            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string name = string.Empty;

            foreach (string item in resourceNames)
            {
                if (item.Contains("SplashScreen.png"))
                {
                    name = item;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidProgramException("License terms not agreed.");
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                this.splashScreen = Texture2D.FromFile(WaveServices.GraphicsDevice, stream);
            }
            #endregion
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                bool keyF10Pressed = WaveServices.Input.KeyboardState.F10 == ButtonState.Pressed;
                if (keyF10Pressed && !this.lastKeyF10Pressed)
                {
                    this.FullScreen = !this.FullScreen;
                }

                this.lastKeyF10Pressed = keyF10Pressed;

                if (this.splashState)
                {
                    #region DEFAULT SPLASHSCREEN
                    position.X = (this.Width / 2.0f) - (this.splashScreen.Width / 2.0f);
                    position.Y = (this.Height / 2.0f) - (this.splashScreen.Height / 2.0f);
                    this.time += elapsedTime;
                    if (time > TimeSpan.FromSeconds(2))
                    {
                        this.splashState = false;
                    }
                    #endregion
                }
                else
                {
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
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (this.splashState)
                {
                    #region DEFAULT SPLASHSCREEN
                    WaveServices.GraphicsDevice.RenderTargets.SetRenderTarget(null);
                    WaveServices.GraphicsDevice.Clear(ref this.backgroundSplashColor, ClearFlags.Target, 1);
                    this.spriteBatch.Draw (this.splashScreen, this.position, Color.White);
                    this.spriteBatch.Render ();
                    #endregion
                }
                else
                {
                    this.game.DrawFrame(elapsedTime);
                }
            }
        }
    }
}

