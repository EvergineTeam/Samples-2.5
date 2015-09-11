#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Models;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.ImageEffects;
using WaveEngine.Materials;
#endregion

namespace CameraRengerTarget
{
    public class MyScene : Scene
    {
        // Cameras
        List<Camera3D> cameras;

        // Camera rendertargets
        Dictionary<Camera3D, RenderTarget> cameraRenderTargets;

        // Current camera Index
        int currentCameraIndex;

        TextBlock textBlock;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.cameras = new List<Camera3D>();
            this.cameraRenderTargets = new Dictionary<Camera3D, RenderTarget>();
        }

        protected override void Start()
        {
            base.Start();

            this.CreateUI();

            this.CreateCameras();

        }
        private void CreateUI()
        {
            Button changeCameraBtn = new Button()
            {
                Text = "Change Camera",
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Left,
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Top,
                Margin = new WaveEngine.Framework.UI.Thickness(20),
                Width = 160,
                Height = 60
            };

            changeCameraBtn.Click += changeCameraBtn_Click;

            EntityManager.Add(changeCameraBtn);

            this.textBlock = new TextBlock()
            {
                Text = "",
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Top,
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Left,
                Margin = new WaveEngine.Framework.UI.Thickness(20, 100, 0, 0)
            };

            EntityManager.Add(this.textBlock);

            WaveServices.Layout.PerformLayout();
        }

        private void changeCameraBtn_Click(object sender, EventArgs e)
        {
            this.currentCameraIndex = (this.currentCameraIndex + 1) % this.cameras.Count;
            this.SetActiveCamera(this.currentCameraIndex);
        }

        private void CreateCameras()
        {
            var freeCamera = this.EntityManager.Find("Free Camera")
                                              .FindComponent<Camera3D>();
            freeCamera.Transform.LookAt(new Vector3(1.55f, 1.372f, 0.75f));
            this.cameras.Add(freeCamera);

            ////// Security Camera 1
            this.CreateSecurityCamera(this.EntityManager.Find("securityCam1"), new Vector3(-2.768f, 2.513f, 0), new Vector3(0.429f, 0.426f, -3.807f), new Vector3(-0.2f, 0.867f, 4.451f), TimeSpan.Zero);

            //////// Security Camera 2
            this.CreateSecurityCamera(this.EntityManager.Find("securityCam2"), new Vector3(2.272f, 2.513f, -2.657f), new Vector3(-4.51f, 0.112f, -2.086f), new Vector3(0.441f, 0.349f, 0.374f), TimeSpan.FromSeconds(3));

            //////// Security Camera 3
            this.CreateSecurityCamera(this.EntityManager.Find("securityCam3"), new Vector3(2.844f, 2.513f, 5.561f), new Vector3(1.763f, 0.349f, 0.374f), new Vector3(-2.285f, 0.349f, 4.394f), TimeSpan.FromSeconds(5));

            this.AddCamerasToStageMaterials();
        }

        private void CreateSecurityCamera(Entity entity, Vector3 position, Vector3 startLookAt, Vector3 endLookAt, TimeSpan timeOffset)
        {
            var camera = entity.FindComponent<Camera3D>();
            // Create RenderTarget to the camera
            camera.RenderTarget = WaveServices.GraphicsDevice.RenderTargets.CreateRenderTarget(350, 256);
            camera.Transform.Position = position;
            camera.Transform.LookAt(startLookAt);

            entity.AddComponent(new SecurityCameraBehavior(startLookAt, endLookAt, timeOffset));

            // This camera will not render the GUI layer
            camera.LayerMask[DefaultLayers.GUI] = false;

            // Register as secondary camera
            this.cameras.Add(camera);
            this.cameraRenderTargets[camera] = camera.RenderTarget;
        }

        private void AddCamerasToStageMaterials()
        {
            var materialsMap = EntityManager.Find("Stage").FindComponent<MaterialsMap>();

            for (int i = 0; i < 3; i++)
            {
                var material = (StandardMaterial)materialsMap.Materials["screen_" + i];
                material.LightingEnabled = false;
                material.Diffuse = this.cameras[i + 1].RenderTarget;
            }
        }

        private void SetActiveCamera(int cameraIndex)
        {
            var activeCamera = this.cameras[cameraIndex];
            activeCamera.RenderTarget = null;
            activeCamera.LayerMask[DefaultLayers.GUI] = true;
            this.RenderManager.SetActiveCamera3D(activeCamera.Owner);

            this.textBlock.Text = string.Format("Active Camera: {0}", activeCamera.Owner.Name);

            for (int i = 0; i < this.cameras.Count; i++)
            {
                var camera = this.cameras[i];

                if (cameraIndex != i)
                {
                    if (this.cameraRenderTargets.ContainsKey(camera))
                    {
                        camera.RenderTarget = this.cameraRenderTargets[camera];
                        camera.LayerMask[DefaultLayers.GUI] = false;
                    }
                }
            }
        }
    }
}
