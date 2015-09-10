// Copyright (C) 2015 Weekend Game Studio
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
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace IBLSample
{
    public class MyScene : Scene
    {
        /// <summary>
        /// The amount of environments
        /// </summary>
        private const int NUMENVIRONMENTS = 3;

        /// <summary>
        /// The current environment
        /// </summary>
        private int currentEnv = 1;

        /// <summary>
        /// Dictionary of the different environments maps for the sky
        /// </summary>
        private Dictionary<int, string> skyboxMaps;

        /// <summary>
        /// Camera
        /// </summary>
        private Entity camera;

        /// <summary>
        /// Venus entity.
        /// </summary>
        private Entity venus;


        /// <summary>
        /// Dictionary for the different materials for venus
        /// </summary>
        private Dictionary<int, string> venusMaps;

        /// <summary>
        /// Creates the scene.
        /// </summary>
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.camera = EntityManager.Find("camera3D");

            this.venus = EntityManager.Find("venus");

            this.CreateUI();

            this.CreateMaterials();
        }

        /// <summary>
        /// Creates the UI.
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

            changeSkyButton.Click += (o, e) => { this.ChangeEnvironment(); };
            
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
        /// Updates the materials to the current environment
        /// </summary>
        private void UpdateMaterials()
        {
            var skybox = this.camera.FindComponent<Skybox>();
            skybox.CubemapPath = this.skyboxMaps[currentEnv];            

            var material = this.venus.FindComponent<MaterialsMap>();
            material.DefaultMaterialPath = this.venusMaps[currentEnv];
        }

        /// <summary>
        /// Creates the different materials of the scene.
        /// </summary>
        private void CreateMaterials()
        {
            this.skyboxMaps = new Dictionary<int, string>();
            this.skyboxMaps.Add(0, WaveContent.Assets.Environment1_cubemap);
            this.skyboxMaps.Add(1, WaveContent.Assets.Environment2_cubemap);
            this.skyboxMaps.Add(2, WaveContent.Assets.Environment3_cubemap);


            this.venusMaps = new Dictionary<int, string>();
            this.venusMaps.Add(0, WaveContent.Assets.Materials.Environment1);
            this.venusMaps.Add(1, WaveContent.Assets.Materials.Environment2);
            this.venusMaps.Add(2, WaveContent.Assets.Materials.Environment3);
        }
    }
}
