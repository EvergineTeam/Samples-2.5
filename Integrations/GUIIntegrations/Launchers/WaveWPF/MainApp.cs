#region File Description
//-----------------------------------------------------------------------------
// MainApp
//
// Copyright � 2013 Plain Concepts. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using ProjectGame = TeapotSample.Game;
#endregion

namespace WaveWPF
{
    /// <summary>
    /// The MainGame app
    /// </summary>
    public class MainApp : WaveEngine.Adapter.WPFApplication
    {
        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public ProjectGame Game
        {
            get;
            private set;
        }

        private SpriteBatch spriteBatch;
        private Texture2D splashScreen;
        private bool splashState = true;
        private TimeSpan time;
        private Vector2 position;
        private Color backgroundSplashColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainApp"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public MainApp(int width, int height)
            : base(width, height)
        {
        }

        /// <summary>
        /// Perform further custom initialize for this instance.
        /// </summary>
        /// <exception cref="System.InvalidProgramException">License terms not agreed.</exception>
        public override void Initialize()
        {
            this.Game = new ProjectGame();
            this.Game.Initialize(this);

            #region WAVE SOFTWARE LICENSE AGREEMENT
            this.backgroundSplashColor = new Color("#ebebeb");
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

            position = new Vector2();
            position.X = (this.Width / 2.0f) - (this.splashScreen.Width / 2.0f);
            position.Y = (this.Height / 2.0f) - (this.splashScreen.Height / 2.0f);
            #endregion
        }

        /// <summary>
        /// Perform further custom update for this instance.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            if (this.Game != null && !this.Game.HasExited)
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
                    this.Game.UpdateFrame(elapsedTime);
                }
            }
        }

        /// <summary>
        /// Perform further custom draw for this instance.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last draw.</param>
        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.Game != null && !this.Game.HasExited)
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
                    this.Game.DrawFrame(elapsedTime);
                }
            }
        }

        public void AppRender()
        {
            base.Render();
            base.RefreshImageSource();
        }
    }
}