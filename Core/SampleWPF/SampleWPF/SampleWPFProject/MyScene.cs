#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace SampleWPFProject
{
    public class MyScene : Scene
    {
        /// <summary>
        /// The earth entity
        /// </summary>
        public Entity Earth;

        /// <summary>
        /// The moon entity
        /// </summary>
        public Entity Moon;

        /// <summary>
        /// The sun entity
        /// </summary>
        public PointLight Sun;

        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;
            RenderManager.DebugLines = false;
            RenderManager.RegisterLayerBefore(new SkyLayer(this.RenderManager), DefaultLayers.Alpha);

            FreeCamera camera = new FreeCamera("camera", new Vector3(-2.4f, 0, -3), new Vector3(-1.6f, 0, -2.5f))
            {
                Speed = 2,
                NearPlane = 0.1f
            };

            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            this.Sun = this.CreateSunFlare();

            this.Earth = this.CreatePlanet("Earth", Vector3.Zero, 1, 10, Vector3.Zero, 0);
            this.Moon = this.CreatePlanet("Moon", new Vector3(0, 0, 20), 1, 28, Vector3.Zero, 28);
        }

        /// <summary>
        /// Creates the flare.
        /// </summary>
        /// <returns></returns>
        private PointLight CreateSunFlare()
        {
            PointLight skylight = new PointLight("SkyLight", new Vector3(100, 0, 0))
            {
                Attenuation = 1000,
                Color = Color.White,
                IsVisible = true
            };

            LensFlare flare = new LensFlare()
            {
                Scale = 1
            };

            flare.Flares = new FlareElement[]
            {
                new FlareElement(0, new Vector2(3), new Color(1f,  1f,  1f), "Content/flareTexture.wpk"),
                new FlareElement(0.2f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), "Content/flare5.wpk"),
                new FlareElement(0.5f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), "Content/flare3.wpk"),
                new FlareElement(0.8f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), "Content/flare4.wpk"),
                new FlareElement(1.2f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), "Content/flare2.wpk"),
                new FlareElement(1.5f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), "Content/flare1.wpk"),
            };

            skylight.LensFlare = flare;

            EntityManager.Add(skylight);

            return skylight;
        }

        /// <summary>
        /// Creates a planet.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pos">The pos.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="dayTime">The day time in seconds.</param>
        /// <param name="orbitCenter">The orbit center.</param>
        /// <param name="yearTime">The year time in seconds.</param>
        /// <returns>The planet entity</returns>
        private Entity CreatePlanet(string name, Vector3 pos, float scale, float dayTime, Vector3 orbitCenter, float yearTime)
        {
            string diffuse = string.Format("Content/{0}DiffuseMap.wpk", name);
            string normal = string.Format("Content/{0}NormSpecMap.wpk", name);
            Entity planet = new Entity()
                .AddComponent(new Transform3D() { Position = pos, Scale = new Vector3(scale, scale, scale) })
                .AddComponent(new Model("Content/planet.wpk"))
                .AddComponent(new SphereCollider())
                .AddComponent(new ModelRenderer())
                .AddComponent(new OrbitBehavior(dayTime, Vector3.Zero, yearTime))
                .AddComponent(new MaterialsMap(new NormalMappingMaterial(diffuse, normal) { AmbientColor = new Color(0.05f, 0.1f, 0.1f) }));

            this.EntityManager.Add(planet);
            return planet;
        }
    }
}
