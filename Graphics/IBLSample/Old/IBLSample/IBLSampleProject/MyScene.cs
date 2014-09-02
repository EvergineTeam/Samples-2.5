#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace IBLSampleProject
{
    public class MyScene : Scene
    {
        /// <summary>
        /// The amount of environments
        /// </summary>
        private const int NUMENVIRONMENTS = 3;

        /// <summary>
        /// Venus entity.
        /// </summary>
        private Entity venus;

        /// <summary>
        /// Sky entity.
        /// </summary>
        private Entity sky;

        /// <summary>
        /// Dictionary of the different environments maps for the sky
        /// </summary>
        private Dictionary<int, MaterialsMap> skyboxMaps;

        /// <summary>
        /// Dictionary of the different environments maps for the venus statue
        /// </summary>
        private Dictionary<Tuple<bool, int>, MaterialsMap> venusMaps;

        /// <summary>
        /// The current environment
        /// </summary>
        private int currentEnv = 1;

        /// <summary>
        /// If the ambient occlusion is enabled.
        /// </summary>
        private bool isOcclusionEnabled = true;

        protected override void CreateScene()
        {
            RenderManager.RegisterLayerBefore(new SkyLayer(this.RenderManager), DefaultLayers.Alpha);

            ViewCamera camera = new ViewCamera("camera", new Vector3(0, 5, 13), new Vector3(0, 5, 0))
            {
                FieldOfView = 0.8f,
                NearPlane = 0.1f
            };
            EntityManager.Add(camera);

            RenderManager.SetActiveCamera(camera.Entity);

            this.CreateMaterials();

            this.CreateVenus();

            this.CreateSky();

            this.CreateUI();

            this.UpdateMaterials();
        }

        /// <summary>
        /// Creates the UI Interface
        /// </summary>
        private void CreateUI()
        {
            Button changeSkyButton = new Button("changeSky")
            {
                Text = "Change environment",
                Margin = new WaveEngine.Framework.UI.Thickness(10),
                Width = 200
            };
            EntityManager.Add(changeSkyButton);

            CheckBox ambientCheckBox = new CheckBox("ambientCheckBox")
            {
                Text = "Ambient Occlusion",
                Margin = new WaveEngine.Framework.UI.Thickness(10, 70, 0, 0),
                IsChecked = this.isOcclusionEnabled
            };
            EntityManager.Add(ambientCheckBox);

            changeSkyButton.Click += (o, e) => { this.ChangeEnvironment(); };
            ambientCheckBox.Checked += (o, e) => { this.ChangeAmbientOcclusion(e.Value); };
        }

        /// <summary>
        /// Creates the skybox
        /// </summary>
        private void CreateSky()
        {
            this.sky = new Entity("sky")
               .AddComponent(new Transform3D() { Scale = new Vector3(-1) })
               .AddComponent(new SkyboxBehavior())
               .AddComponent(new BoxCollider())
               .AddComponent(Model.CreateCube(1))
               .AddComponent(new ModelRenderer());
            EntityManager.Add(this.sky);
        }

        /// <summary>
        /// Creates the Venus statue model.
        /// </summary>
        private void CreateVenus()
        {
            this.venus = new Entity("venus")
                .AddComponent(new Transform3D())
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(0, 0.5f, 0) })
                .AddComponent(new Model("Content/venus.wpk"))
                .AddComponent(new ModelRenderer());
            EntityManager.Add(this.venus);
        }

        /// <summary>
        /// Changes the environment of the scene.
        /// </summary>
        private void ChangeEnvironment()
        {
            this.currentEnv = (this.currentEnv + 1) % NUMENVIRONMENTS;
            this.UpdateMaterials();
        }

        /// <summary>
        /// Changes the ambient occlusion.
        /// </summary>
        /// <param name="occlusionEnabled">if the ambient occlusion is enabled</param>
        private void ChangeAmbientOcclusion(bool occlusionEnabled)
        {
            this.isOcclusionEnabled = occlusionEnabled;
            this.UpdateMaterials();
        }

        private void UpdateMaterials()
        {
            this.sky.RemoveComponent<MaterialsMap>();
            this.sky.AddComponent(this.skyboxMaps[this.currentEnv]);
            this.sky.RefreshDependencies();

            this.venus.RemoveComponent<MaterialsMap>();
            this.venus.AddComponent(this.venusMaps[new Tuple<bool, int>(this.isOcclusionEnabled, this.currentEnv)]);
            this.venus.RefreshDependencies();
        }

        /// <summary>
        /// Creates the different materials of the scene.
        /// </summary>
        private void CreateMaterials()
        {
            this.skyboxMaps = new Dictionary<int, MaterialsMap>();
            this.venusMaps = new Dictionary<Tuple<bool, int>, MaterialsMap>();

            for (int i = 0; i < NUMENVIRONMENTS; i++)
            {
                string skyboxTexture = string.Format("Content/environment{0}.wpk", i + 1);
                string venusTexture = string.Format("Content/environment{0}Light.wpk", i + 1);

                this.skyboxMaps.Add(i, new MaterialsMap(new SkyboxMaterial(skyboxTexture, typeof(SkyLayer))));
                this.venusMaps.Add(new Tuple<bool, int>(true, i), new MaterialsMap(new BasicMaterial("Content/venusLightingMap.wpk", venusTexture) { LightingEnabled = true }));
                this.venusMaps.Add(new Tuple<bool, int>(false, i), new MaterialsMap(new BasicMaterial("Content/white.wpk", venusTexture) { LightingEnabled = true }));
            }
        }
    }
}
