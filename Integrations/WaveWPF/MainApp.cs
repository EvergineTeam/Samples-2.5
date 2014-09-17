#region File Description
//-----------------------------------------------------------------------------
// MainApp
//
// Copyright © 2013 Plain Concepts. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace WaveWPF
{
    /// <summary>
    /// The MainGame app
    /// </summary>
    public class MainApp : WaveEngine.Adapter.WPFApplication
    {
        /// <summary>
        /// The game
        /// </summary>
        private CubeTestProject.Game game;

        /// <summary>
        /// Gets the wave game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public CubeTestProject.Game Game
        {
            get
            {
                return this.game;
            }
        }

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
            this.game = new CubeTestProject.Game();
            this.game.Initialize(this);
        }

        /// <summary>
        /// Perform further custom update for this instance.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (WaveServices.Input.KeyboardState.F10 == ButtonState.Pressed)
                {
                    this.FullScreen = !this.FullScreen;
                }

                this.game.UpdateFrame(elapsedTime);
            }
        }

        /// <summary>
        /// Perform further custom draw for this instance.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last draw.</param>
        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                this.game.DrawFrame(elapsedTime);
            }
        }
    }
}