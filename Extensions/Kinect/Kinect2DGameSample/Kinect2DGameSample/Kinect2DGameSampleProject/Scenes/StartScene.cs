#region File Description
//-----------------------------------------------------------------------------
// StartScene
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Common.Graphics;
using Kinect2DGameSampleProject.Behaviors;
using WaveEngine.Components.UI;
using WaveEngine.Framework.UI;
#endregion

namespace Kinect2DGameSampleProject.Scenes
{
    /// <summary>
    /// Kinect Game Scene
    /// </summary>
    public class StartScene : Scene
    {
        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all 
        /// <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            // Scene behaviors
            this.AddSceneBehavior(new StartSceneBehavior(), SceneBehavior.Order.PostUpdate);

            // Creates a 2D camera
            var camera2D = new FixedCamera2D("Camera2D") { ClearFlags = ClearFlags.DepthAndStencil }; // Transparent background need this clearFlags.
            this.EntityManager.Add(camera2D);

            // "Catch to Start" Text
            var startTextBlock = new TextBlock("StartEntity")
            {
                Width = 885,
                Height = 65,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                FontPath = "Content/PFDarkBigFont",
                Text = "Catch this to START",
                Foreground = Color.Red,
            };
            startTextBlock.Entity.AddComponent(new BlinkBehavior(TimeSpan.FromSeconds(0.2f))
                            {
                                MinOpacity = 0.4f
                            });
            this.EntityManager.Add(startTextBlock);
        }
    }
}
