#region File Description
//-----------------------------------------------------------------------------
// Game
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Kinect;
using WaveEngine.Framework.Services;
using WaveEngine.Components.Transitions;
using Kinect2DGameSampleProject.Scenes;
#endregion

namespace Kinect2DGameSampleProject
{
    /// <summary>
    /// Game Class
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
            vm.Activate(1920, 1080, ViewportManager.StretchMode.Uniform);

            // Register Kinect Service Wave Engine Extension
            WaveServices.RegisterService(new KinectService());

            ScreenContext screenContext = new ScreenContext(new KinectScene())
                                              {
                                                  Behavior = ScreenContextBehaviors.DrawInBackground 
                                                  | ScreenContextBehaviors.UpdateInBackground
                                              };

            WaveServices.ScreenContextManager.Push(screenContext);
            WaveServices.ScreenContextManager.Push(new ScreenContext(new StartScene()), new CrossFadeTransition(TimeSpan.FromSeconds(3.0f)));
        }
    }
}
