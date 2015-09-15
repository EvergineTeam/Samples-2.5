#region File Description
//-----------------------------------------------------------------------------
// Kinect Sample Project
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Kinect;
#endregion

namespace KinectSampleProject
{
    /// <summary>
    /// Game class
    /// </summary>
    public class Game : WaveEngine.Framework.Game
    {
        /// <summary>
        /// Initializes the game according to the passed application (thus adapter).
        ///             The adapter implementation depends on the while-running platform.
        ///             Such method acts as the bridge between the game and the final hardware.
        /// </summary>
        /// <param name="application">The application (adapter).</param>
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            // ViewportManager is used to automatically adapt resolution to fit screen size
            ViewportManager vm = WaveServices.ViewportManager;
            vm.Activate(1280, 720, ViewportManager.StretchMode.Uniform);

            WaveServices.RegisterService(new KinectService());

            ScreenContext screenContext = new ScreenContext(new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
