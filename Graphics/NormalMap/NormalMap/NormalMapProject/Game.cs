// Copyright (C) 2012-2013 Weekend Game Studio
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
// IN THE SOFTWARE.

#region Using Statements
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Materials;
using System.Linq;
using WaveEngine.Components.Cameras;
#endregion

namespace NormalMapProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            WaveServices.ScreenContextManager.To(new ScreenContext(new MyScene()));
        }
    }

    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;
            //RenderManager.DebugLines = true;

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 4, 12), new Vector3(0, 4, 0)) { Speed = 5 };
            EntityManager.Add(camera.Entity);

            RenderManager.SetActiveCamera(camera.Entity);

            PointLight Light0 = new PointLight("Light0", new Vector3(-5, 8, 10))
            {
                Color = Color.White,
                Attenuation = 20,
            };

            Light0.Entity.AddComponent(new MoveBehavior() { Speed = 0.2f });

            EntityManager.Add(Light0.Entity);

            float value = 0.0f;
            Color c = new Color(value, value, value, 1);

            Entity cubeModel = new Entity("Cube")
                .AddComponent(new Transform3D() { Scale = new Vector3(6) })
                //.AddComponent(new Spinner() { IncreaseY = 0.02f })
                .AddComponent(new BoxCollider())
                .AddComponent(new Model("Content/Cube.wpk"))
                .AddComponent(new MaterialsMap(new NormalMappingMaterial("Content/floor.wpk", "Content/floor_normal_spec.wpk", "Content/lightmap.wpk") { AmbientColor = c }))  // "Content/lightmap.wpk"
                .AddComponent(new ModelRenderer());

            EntityManager.Add(cubeModel);

            #region UI
            Slider slider1 = new Slider()
            {
                Margin = new Thickness(10, 90, 0, 0),
                Width = 500,
                Minimum = 4,
                Maximum = 40,
                Value = 30
            };

            slider1.RealTimeValueChanged += (s, e) =>
            {
                var entity = EntityManager.Find("Light0");
                var component = entity.FindComponent<PointLightProperties>();
                component.Attenuation = e.NewValue;
            };

            EntityManager.Add(slider1);

            Slider slider2 = new Slider()
            {
                Margin = new Thickness(10, 140, 0, 0),
                Width = 500,
                Minimum = 1,
                Maximum = 100
            };

            slider2.RealTimeValueChanged += (s, e) =>
            {
                var entity = EntityManager.Find("Cube");
                var component = entity.FindComponent<MaterialsMap>();
                var material = (component.DefaultMaterial as NormalMappingMaterial);

                float v = (float)e.NewValue / 100.0f;
                Color c1 = new Color(v, v, v, 1);
                material.AmbientColor = c1;
            };

            EntityManager.Add(slider2);

            ToggleSwitch lightmapEnabled = new ToggleSwitch()
            {
                Margin = new Thickness(10, 10, 0, 0),
                IsOn = true,
                OnText = "LightMap On",
                OffText = "LightMap Off",
            };

            lightmapEnabled.Toggled += (s, e) =>
            {
                var entity = EntityManager.Find("Cube");
                var component = entity.FindComponent<MaterialsMap>();
                var material = (component.DefaultMaterial as NormalMappingMaterial);
                material.LightMapEnabled = !material.LightMapEnabled;
            };

            EntityManager.Add(lightmapEnabled);

            ToggleSwitch textureEnabled = new ToggleSwitch()
            {
                Margin = new Thickness(10, 40, 0, 0),
                IsOn = true,
                OnText = "Texture On",
                OffText = "Texture Off"
            };

            textureEnabled.Toggled += (s, o) =>
            {
                var entity = EntityManager.Find("Cube");
                var component = entity.FindComponent<MaterialsMap>();
                var material = (component.DefaultMaterial as NormalMappingMaterial);
                material.TextureEnabled = !material.TextureEnabled;
            };

            EntityManager.Add(textureEnabled);

            ToggleSwitch normalMapEnabled = new ToggleSwitch()
            {
                Margin = new Thickness(300, 10, 0, 0),
                IsOn = true,
                OnText = "NormalMap On",
                OffText = "NormalMap Off"
            };

            normalMapEnabled.Toggled += (s, o) =>
            {
                var entity = EntityManager.Find("Cube");
                var component = entity.FindComponent<MaterialsMap>();
                var material = (component.DefaultMaterial as NormalMappingMaterial);
                material.NormalMappingEnabled = !material.NormalMappingEnabled;
            };

            EntityManager.Add(normalMapEnabled);
            #endregion
        }
    }
}
