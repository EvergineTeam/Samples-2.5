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

            var entity = EntityManager.Find("cube");
            ToggleSwitch lightmapEnabled = new ToggleSwitch()
            {
                Margin = new Thickness(10, 10, 0, 0),
                IsOn = true,
                OnText = "LightMap On",
                OffText = "LightMap Off",
            };

            lightmapEnabled.Toggled += (s, e) =>
            {
                var component = entity.FindComponent<MaterialComponent>();
                var material = component.Material as StandardMaterial;
                if (!string.IsNullOrEmpty(material.Diffuse2Path))
                {
                    material.Diffuse2Path = string.Empty;
                }
                else
                {
                    material.Diffuse2Path = WaveContent.Assets.lightmap_png;
                }
            };

            this.EntityManager.Add(lightmapEnabled);

            ToggleSwitch textureEnabled = new ToggleSwitch()
            {
                Margin = new Thickness(10, 40, 0, 0),
                IsOn = true,
                OnText = "Texture On",
                OffText = "Texture Off",
            };

            textureEnabled.Toggled += (s, o) =>
            {
                var component = entity.FindComponent<MaterialComponent>();
                var material = component.Material as StandardMaterial;

                if (!string.IsNullOrEmpty(material.Diffuse1Path))
                {
                    material.Diffuse1Path = string.Empty;
                }
                else
                {
                    material.Diffuse1Path = WaveContent.Assets.floor_png;
                }
            };

            this.EntityManager.Add(textureEnabled);

            ToggleSwitch normalMapEnabled = new ToggleSwitch()
            {
                Margin = new Thickness(300, 10, 0, 0),
                IsOn = true,
                OnText = "NormalMap On",
                OffText = "NormalMap Off",
            };

            normalMapEnabled.Toggled += (s, o) =>
            {
                var component = entity.FindComponent<MaterialComponent>();
                var material = component.Material as StandardMaterial;
                if (!string.IsNullOrEmpty(material.NormalPath))
                {
                    material.NormalPath = string.Empty;
                }
                else
                {
                    material.NormalPath = WaveContent.Assets.floor_normal_png;
                }
            };

            this.EntityManager.Add(normalMapEnabled);
        }
    }
}
