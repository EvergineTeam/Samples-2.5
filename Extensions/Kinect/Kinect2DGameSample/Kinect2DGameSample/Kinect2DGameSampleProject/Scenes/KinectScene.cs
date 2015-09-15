#region File Description
//-----------------------------------------------------------------------------
// KinectScene
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Kinect;
using WaveEngine.Kinect.Enums;
#endregion

namespace Kinect2DGameSampleProject.Scenes
{
    /// <summary>
    /// Kinect Game Scene (Background color sensor)
    /// </summary>
    public class KinectScene : Scene
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
            // Creates the 2D camera
            var camera2D = new FixedCamera2D("Camera2D"); // { ClearFlags = ClearFlags.DepthAndStencil }; // Transparent background need this clearFlags.
            this.EntityManager.Add(camera2D);

            // Starts Kinect Service Sensor
            var kinectService = WaveServices.GetService<KinectService>();
            kinectService.StartSensor(KinectSources.Color | KinectSources.Body);

            // Creates Kinect Color Image Entity
            Entity kinectBackground = new Entity()
                .AddComponent(new Transform2D())
                .AddComponent(new Sprite(kinectService.ColorTexture))
                .AddComponent(new SpriteRenderer());
            this.EntityManager.Add(kinectBackground);
        }
    }
}
