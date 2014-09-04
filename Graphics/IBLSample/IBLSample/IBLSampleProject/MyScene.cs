// Copyright (C) 2014 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS

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
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
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
        /// Camera
        /// </summary>
        private ViewCamera camera;

        /// <summary>
        /// Dictionary of the different environments maps for the sky
        /// </summary>
        private Dictionary<int, Skybox> skyboxMaps;

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
            this.CreateMaterials();

            this.camera = new ViewCamera("camera", new Vector3(0, 5, 13), new Vector3(0, 5, 0))
            {
                FieldOfView = 0.8f,
                NearPlane = 0.1f
            };
            this.camera.Entity.AddComponent(this.skyboxMaps[this.currentEnv]);
            EntityManager.Add(camera);           

            this.CreateVenus();

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
            this.camera.Entity.RemoveComponent<Skybox>();
            this.camera.Entity.AddComponent(this.skyboxMaps[this.currentEnv]);
            this.camera.Entity.RefreshDependencies();

            this.venus.RemoveComponent<MaterialsMap>();
            this.venus.AddComponent(this.venusMaps[new Tuple<bool, int>(this.isOcclusionEnabled, this.currentEnv)]);
            this.venus.RefreshDependencies();
        }

        /// <summary>
        /// Creates the different materials of the scene.
        /// </summary>
        private void CreateMaterials()
        {
            this.skyboxMaps = new Dictionary<int, Skybox>();
            this.venusMaps = new Dictionary<Tuple<bool, int>, MaterialsMap>();

            for (int i = 0; i < NUMENVIRONMENTS; i++)
            {
                string skyboxTexture = string.Format("Content/environment{0}.wpk", i + 1);
                string venusTexture = string.Format("Content/environment{0}Light.wpk", i + 1);

                this.skyboxMaps.Add(i, new Skybox(skyboxTexture));
                this.venusMaps.Add(new Tuple<bool, int>(true, i), new MaterialsMap(new BasicMaterial("Content/venusLightingMap.wpk", venusTexture) { LightingEnabled = true }));
                this.venusMaps.Add(new Tuple<bool, int>(false, i), new MaterialsMap(new BasicMaterial("Content/white.wpk", venusTexture) { LightingEnabled = true }));
            }
        }
    }
}
