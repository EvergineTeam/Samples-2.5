#region Using Statements
using System;
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
using WaveEngine.Framework.UI;
using WaveEngine.Materials;
#endregion

namespace NormalMap
{
    public class MyScene : Scene
    {
        
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

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
                component.Intensity = e.NewValue;
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
                var material = (component.DefaultMaterial as DualMaterial);

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
                var material = (component.DefaultMaterial as DualMaterial);
                if (!string.IsNullOrEmpty(material.Diffuse2Path))
                {
                    material.Diffuse2Path = string.Empty;
                }
                else
                {
                    material.Diffuse2Path = WaveContent.Assets.lightmap_png;
                }
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
                var material = (component.DefaultMaterial as DualMaterial);

                if (!string.IsNullOrEmpty(material.Diffuse1Path))
                {
                    material.Diffuse1Path = string.Empty;
                }
                else
                {
                    material.Diffuse1Path = WaveContent.Assets.floor_png;
                }
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
                var material = (component.DefaultMaterial as DualMaterial);
                if (!string.IsNullOrEmpty(material.NormalPath))
                {
                    material.NormalPath = string.Empty;
                }
                else
                {
                    material.NormalPath = WaveContent.Assets.floor_normal_png;
                }
            };

            EntityManager.Add(normalMapEnabled);
        }
    }
}
