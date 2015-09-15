#region File Description
//-----------------------------------------------------------------------------
// MyScene
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

using WaveEngine.Kinect;
using WaveEngine.Kinect.Behaviors;
using WaveEngine.Kinect.Drawables;
using WaveEngine.Kinect.Enums;
using WaveEngine.Materials;

namespace KinectSampleProject
{
    /// <summary>
    /// Main Scene
    /// </summary>
    public class MyScene : Scene
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
            // Insert your scene definition here.
            #region Simple test

            // Creates a 3D camera
            var camera3D = new FreeCamera("Camera3D", new Vector3(0, 2, 2), Vector3.Zero) { BackgroundColor = Color.CornflowerBlue };
            this.EntityManager.Add(camera3D);

            var kinectService = WaveServices.GetService<KinectService>();
            kinectService.StartSensor(KinectSources.Color | KinectSources.Depth | KinectSources.Body | KinectSources.Face);

            // Draw cubes
            // Color Sensor Cube
            Entity cube = new Entity()
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(-1, 0, 0) })
                .AddComponent(Model.CreateCube())
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1, 2, 3) / 10 })
                .AddComponent(new MaterialsMap(new BasicMaterial(kinectService.ColorTexture)))
                .AddComponent(new ModelRenderer());
            this.EntityManager.Add(cube);

            // Depth Sensor Cube
            Entity cube2 = new Entity()
                .AddComponent(new Transform3D() { LocalPosition = new Vector3(1, 0, 0) })
                .AddComponent(Model.CreateCube())
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1, 2, 3) / 10 })
                .AddComponent(new MaterialsMap(new BasicMaterial(kinectService.DepthTexture)))
                .AddComponent(new ModelRenderer());
            this.EntityManager.Add(cube2);

            // Create a 2D camera
            var camera2D = new FixedCamera2D("Camera2D") { ClearFlags = ClearFlags.DepthAndStencil }; // Transparent background need this clearFlags.
            this.EntityManager.Add(camera2D);

            // Skeleton Control
            Entity skeletonEntity = new Entity()
                .AddComponent(new Transform2D())
                .AddComponent(new KinectSkeletonsBehavior() { CurrentSource = KinectSources.Color })
                .AddComponent(new KinectSkeletonsDrawable());
            this.EntityManager.Add(skeletonEntity);

            // Face control
            Entity faceEntity = new Entity()
                .AddComponent(new Transform2D())
                .AddComponent(new KinectFaceBehavior() { CurrentSource = KinectSources.Color })
                .AddComponent(new KinectFaceDrawable2D());
            this.EntityManager.Add(faceEntity);
            #endregion
        }
    }
}
