#region Using Statements
using CameraRenderTargetProject.Components;
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace CameraRenderTargetProject
{
    public class MyScene : Scene
    {      
         // Cameras
        List<FixedCamera> cameras;
        
        // Camera rendertargets
        Dictionary<FixedCamera, RenderTarget> cameraRenderTargets;
        
        // Current camera Index
        int currentCameraIndex;

        TextBlock textBlock;

        protected override void CreateScene()
        {
            this.CreateUI();

            this.CreateCameras();

            this.CreateStage();

            this.CreateIsis();
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
                Margin = new WaveEngine.Framework.UI.Thickness(20, 100,0,0)
            };

            EntityManager.Add(this.textBlock);
        }

        private void changeCameraBtn_Click(object sender, EventArgs e)
        {
            this.currentCameraIndex = (this.currentCameraIndex + 1) % this.cameras.Count;
            this.SetActiveCamera(this.currentCameraIndex);
        }

        private void CreateCameras()
        {
            this.cameras = new List<FixedCamera>();
            this.cameraRenderTargets = new Dictionary<FixedCamera, RenderTarget>();

            // Free camera
            FreeCamera camera = new FreeCamera("Free Camera", new Vector3(0.519f, 1.822f, 5.789f), new Vector3(1.55f, 1.372f, 0.75f))
                {
                    NearPlane = 0.01f,
                    FarPlane = 15,
                    FieldOfView = MathHelper.ToRadians(80),
                    Speed = 3
                };

            EntityManager.Add(camera);            
            this.cameras.Add(camera);

            // Security Camera 1
            this.CreateSecurityCamera(new Vector3(-2.768f, 2.513f, 0), new Vector3(0.429f, 0.426f, -3.807f), new Vector3(-0.2f, 0.867f, 4.451f), TimeSpan.Zero);

            // Security Camera 2
            this.CreateSecurityCamera(new Vector3(2.272f, 2.513f, -2.657f), new Vector3(-4.51f, 0.112f, -2.086f), new Vector3(0.441f, 0.349f, 0.374f), TimeSpan.FromSeconds(3));

            // Security Camera 3
            this.CreateSecurityCamera(new Vector3(2.844f, 2.513f, 5.561f), new Vector3(1.763f, 0.349f, 0.374f), new Vector3(-2.285f, 0.349f, 4.394f), TimeSpan.FromSeconds(5));
        }

        private void CreateSecurityCamera(Vector3 position, Vector3 startLookAt, Vector3 endLookAt, TimeSpan timeOffset)
        {
            FixedCamera camera = new FixedCamera("securityCam_" + this.cameras.Count, position, startLookAt)
            {
                // Create RenderTarget to the camera
                RenderTarget = WaveServices.GraphicsDevice.RenderTargets.CreateRenderTarget(350, 256),
                FieldOfView = MathHelper.ToRadians(80),
                NearPlane = 0.01f,
                FarPlane = 15,
            };
            camera.Entity.AddComponent(new SecurityCameraBehavior(startLookAt, endLookAt, timeOffset));
            
            // This camera will not render the GUI layer
            camera.LayerMask[DefaultLayers.GUI] = false;

            // Register as secondary camera
            this.RenderManager.AddSecondaryCamera(camera.Entity);
            EntityManager.Add(camera);
            
            this.cameras.Add(camera);
            this.cameraRenderTargets[camera] = camera.RenderTarget;
        }
        
        /// <summary>
        /// Creates the scenery.
        /// </summary>
        private void CreateStage()
        {
            // create materials
            var materialsMap = new Dictionary<string, Material>()
            {
                {"stageWall", new DualTextureMaterial("Content/Textures/stageWall_Diffuse.wpk", "Content/Textures/stageLightingMap.wpk", DefaultLayers.Opaque)},
                {"stageWall2", new DualTextureMaterial("Content/Textures/stageWall2_Diffuse.wpk", "Content/Textures/stageLightingMap.wpk", DefaultLayers.Opaque)},
                {"stageFloor", new DualTextureMaterial("Content/Textures/stageFloor_Diffuse.wpk", "Content/Textures/stageLightingMap.wpk", DefaultLayers.Opaque)},
                {"stageDoor", new DualTextureMaterial("Content/Textures/stageDoor_Diffuse.wpk", "Content/Textures/stageLightingMap.wpk", DefaultLayers.Opaque)},
                {"stageCeiling001", new DualTextureMaterial("Content/Textures/stageCeiling_Diffuse.wpk", "Content/Textures/stageLightingMap.wpk", DefaultLayers.Opaque)},
                {"screen_0", new BasicMaterial(this.cameras[1].RenderTarget, DefaultLayers.Opaque)},
                {"screen_1", new BasicMaterial(this.cameras[2].RenderTarget, DefaultLayers.Opaque)},
                {"screen_2", new BasicMaterial(this.cameras[3].RenderTarget, DefaultLayers.Opaque)},
            };

            Entity stage = new Entity("stage")
            .AddComponent(new Transform3D())
            .AddComponent(new Model("Content/Models/Stage.wpk"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(materialsMap));

            EntityManager.Add(stage);
        }


        /// <summary>
        /// Creates main character.
        /// </summary>
        private void CreateIsis()
        {
            PointLight light = new PointLight("light", new Vector3(-1.783f, 2.503f, 0))
            {
                IsVisible = true,
                Color = new Color("f0f2ff"),
                Attenuation = 20
            };
            EntityManager.Add(light);

            Entity isis = new Entity("Isis")
            .AddComponent(new Transform3D() { Position = Vector3.UnitY * 0.15f, Scale = Vector3.One * 1.14f, Rotation = Vector3.UnitY * MathHelper.ToRadians(-25) })
            .AddComponent(new SkinnedModel("Content/Models/isis.wpk"))
            .AddComponent(new SkinnedModelRenderer())
            .AddComponent(new Animation3D("Content/Models/isis-animations.wpk"))
            .AddComponent(new MaterialsMap(new NormalMappingMaterial("Content/Textures/isis-difuse.wpk", "Content/Textures/isis-normal.wpk", DefaultLayers.Opaque) { AmbientColor = Color.White * 0.6f }));

            EntityManager.Add(isis);
            isis.FindComponent<Animation3D>().PlayAnimation("Idle", true);
        }

        private void SetActiveCamera(int cameraIndex)
        {
            foreach (var camera in this.cameras)
            {
                this.RenderManager.RemoveSecondaryCamera(camera.Entity);
            }
            
            var activeCamera = this.cameras[cameraIndex];
            activeCamera.RenderTarget = null;
            activeCamera.LayerMask[DefaultLayers.GUI] = true;
            this.RenderManager.SetActiveCamera(activeCamera.Entity);

            this.textBlock.Text = string.Format("Active Camera: {0}", activeCamera.Entity.Name);


            for (int i = 0; i < this.cameras.Count; i++)
            {
                var camera = this.cameras[i];
             
                if (cameraIndex != i)
                {
                    if (this.cameraRenderTargets.ContainsKey(camera))
                    {
                        camera.RenderTarget = this.cameraRenderTargets[camera];
                        this.RenderManager.AddSecondaryCamera(camera.Entity);
                        camera.LayerMask[DefaultLayers.GUI] = false;
                    }                    
                }
            }
        }
    }
}
