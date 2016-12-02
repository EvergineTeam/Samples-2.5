using NavigationFlow.Navigation;
using System;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace NavigationFlow
{
    public class App : WaveEngine.Adapter.Application
    {
        NavigationFlow.Game game;
        SpriteBatch spriteBatch;
        Texture2D splashScreen;
        bool splashState = true;
        TimeSpan time;
        Vector2 position;
        Color backgroundSplashColor;
        KeyboardState lastKeyboardState;

        public App()
        {
            this.Width = 1280;
            this.Height = 720;
            this.FullScreen = false;
            this.WindowTitle = "NavigationFlow";
            this.HasVideoSupport = true;
        }

        public override void Initialize()
        {
            this.game = new NavigationFlow.Game();
            this.game.Initialize(this);

            #region DEFAULT SPLASHSCREEN
            this.backgroundSplashColor = new Color("#ebebeb");
            this.spriteBatch = new SpriteBatch(WaveServices.GraphicsDevice);

            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string name = string.Empty;

            foreach (string item in resourceNames)
            {
                if (item.Contains("SplashScreen.wpk"))
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
                this.splashScreen = WaveServices.Assets.Global.LoadAsset<Texture2D>(name, stream);
            }

            position = new Vector2();
            position.X = (this.Width / 2.0f) - (this.splashScreen.Width / 2.0f);
            position.Y = (this.Height / 2.0f) - (this.splashScreen.Height / 2.0f);
            #endregion
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                var keyboardState = WaveServices.Input.KeyboardState;

                if (keyboardState.IsKeyReleased(Keys.F10) &&
                    this.lastKeyboardState.IsKeyPressed(Keys.F10))
                {
                    this.FullScreen = !this.FullScreen;
                }

                if (this.splashState)
                {
                    #region DEFAULT SPLASHSCREEN
                    this.time += elapsedTime;
                    if (time > TimeSpan.FromSeconds(2))
                    {
                        this.splashState = false;
                    }
                    #endregion
                }
                else
                {
                    if (keyboardState.IsKeyReleased(Keys.Escape) &&
                        this.lastKeyboardState.IsKeyPressed(Keys.Escape))
                    {
                        var navService = WaveServices.GetService<NavigationService>();

                        if (navService.CanNavigate(NavigateCommands.Back))
                        {
                            navService.Navigate(NavigateCommands.Back);
                        }
                    }
                    else
                    {
                        this.game.UpdateFrame(elapsedTime);
                    }
                }

                this.lastKeyboardState = keyboardState;
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
                    this.spriteBatch.Draw(this.splashScreen, this.position, Color.White);
                    this.spriteBatch.Render();
                    #endregion
                }
                else
                {
                    this.game.DrawFrame(elapsedTime);
                }
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

