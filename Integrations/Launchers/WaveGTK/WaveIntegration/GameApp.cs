#region File Description
//-----------------------------------------------------------------------------
// GameApp
//
// Copyright © 2015 Wave Corporation
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Framework.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Services;
using System.Reflection;
using System.IO;
using WaveEngine.Common.Input;
using ProjectGame = TeapotSample.Game;
#endregion

namespace WaveGTK.WaveIntegration
{
    /// <summary>
    /// GameApp class Wave interop with GTK
    /// </summary>
#if WINDOWS
    public class GameApp : WaveEngine.Adapter.FormApplication
#elif LINUX || MAC
    public class GameApp : WaveEngine.Adapter.BaseApplication
#endif
    {
        ProjectGame game;
        SpriteBatch spriteBatch;
        Texture2D splashScreen;
        bool splashState = true;
        TimeSpan time;
        Vector2 position;
        Color backgroundSplashColor;

        /// <summary>
        /// Occurs when [initialized].
        /// </summary>
        public event EventHandler<ProjectGame> Initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameApp" /> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public GameApp(int width, int height)
            : base(width, height)
        {
        }

        /// <summary>
        /// Perform further custom initialize for this instance.
        /// </summary>
        public override void Initialize()
        {
            this.game = new ProjectGame();
            this.game.Initialize(this);

            #region WAVE SOFTWARE LICENSE AGREEMENT
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

        /// <summary>
        /// Called when updating the main loop.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (this.splashState)
                {
                    #region WAVE SOFTWARE LICENSE AGREEMENT
                    this.time += elapsedTime;
                    if (time > TimeSpan.FromSeconds(2))
                    {
                        this.splashState = false;
                    }
                    #endregion
                }
                else
                {
                    this.game.UpdateFrame(elapsedTime);
                }
            }
        }

        /// <summary>
        /// Called when drawing the main loop.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last draw.</param>
        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (this.splashState)
                {
                    #region WAVE SOFTWARE LICENSE AGREEMENT
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
                this.game.OnActivated();
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
                this.game.OnDeactivated();
            }
        }
    }
}
