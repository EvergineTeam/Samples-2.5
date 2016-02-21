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
using WaveEngine.Framework.Models;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveOculusDemoProject.Entities;
using WaveOculusDemoProject.Layers;
#endregion

namespace WaveOculusDemo
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.CreateLayers();

            // Add asteroidfield
            var asteroidField = new AsteroidFieldDecorator("asteroidField", new Vector3(2000, 2000, 0));
            this.EntityManager.Add(asteroidField);
            WaveServices.SoundPlayer.GetSceneSoundBank(this).MaxConcurrentSounds = 30;
        }

        /// <summary>
        /// Create specific render layers
        /// </summary>
        private void CreateLayers()
        {
            this.RenderManager.RegisterLayerBefore(new StarfieldLayer(this.RenderManager), DefaultLayers.Alpha);
            this.RenderManager.RegisterLayerBefore(new PlanetLayer(this.RenderManager), DefaultLayers.Alpha);

            this.RenderManager.RegisterLayerAfter(new CockpitLayer(this.RenderManager), DefaultLayers.Additive);
            this.RenderManager.RegisterLayerAfter(new CockpitAdditiveLayer(this.RenderManager), typeof(CockpitLayer));

            this.ChangeMaterialLayer(WaveContent.Assets.Materials.StarFieldMat, typeof(StarfieldLayer));
            this.ChangeMaterialLayer(WaveContent.Assets.Materials.StarShineMat1, typeof(StarfieldLayer));
            this.ChangeMaterialLayer(WaveContent.Assets.Materials.StarShineMat2, typeof(CockpitAdditiveLayer));
            this.ChangeMaterialLayer(WaveContent.Assets.Materials.PlanetMat, typeof(PlanetLayer));
            this.ChangeMaterialLayer(WaveContent.Assets.Materials.CockpitMat, typeof(CockpitLayer));
            this.ChangeMaterialLayer(WaveContent.Assets.Materials.Hud.RadarTickMat, typeof(CockpitAdditiveLayer));
        }


        private void ChangeMaterialLayer(string path, Type layerType)
        {
            var material = this.Assets.LoadModel<MaterialModel>(path).Material as StandardMaterial;

            if (material != null)
            {
                material.LayerType = layerType;
            }

        }
    }
}
