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
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace LensFlare
{
    public class MyScene : Scene
    {
        /// <summary>
        /// Creates the scene.
        /// </summary>
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            RenderManager.RegisterLayerBefore(new SkyLayer(this.RenderManager), DefaultLayers.Alpha);

            this.CreateSunFlare();

            this.ConfigurePlanet("Earth", 10, Vector3.Zero, 0);
            this.ConfigurePlanet("Moon", 28, Vector3.Zero, 28);

            EntityManager.Find("camera2D").FindComponent<Camera2D>().CenterScreen();
        }

        /// <summary>
        /// Creates the sun flare.
        /// </summary>
        private void CreateSunFlare()
        {            
            var skylight = EntityManager.Find("skylight");

            var flare = new WaveEngine.Framework.Graphics.LensFlare();
           
            flare.Flares = new FlareElement[]
            {
                new FlareElement(0, new Vector2(3), new Color(1f,  1f,  1f), WaveContent.Assets.Textures.Flares.flareTexture_jpg),
                new FlareElement(0.2f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), WaveContent.Assets.Textures.Flares.flare5_png),
                new FlareElement(0.5f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), WaveContent.Assets.Textures.Flares.flare3_png),
                new FlareElement(0.8f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), WaveContent.Assets.Textures.Flares.flare4_png),
                new FlareElement(1.2f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), WaveContent.Assets.Textures.Flares.flare2_png),
                new FlareElement(1.5f, new Vector2(1), new Color(0.2f,  0.2f,  0.2f), WaveContent.Assets.Textures.Flares.flare1_png),
            };

            skylight.AddComponent(flare);
            skylight.AddComponent(new LensFlareBehavior());
            skylight.RefreshDependencies();
            
        }

        /// <summary>
        /// Configures the planet.
        /// </summary>
        /// <param name="planetName">Name of the planet.</param>
        /// <param name="dayTime">The day time.</param>
        /// <param name="orbitCenter">The orbit center.</param>
        /// <param name="yearTime">The year time.</param>
        private void ConfigurePlanet(string planetName, int dayTime, Vector3 orbitCenter, int yearTime)
        {
            Entity planet = EntityManager.Find(planetName);

            planet.AddComponent(new OrbitBehavior(dayTime, orbitCenter, yearTime));
        }
        

       
    }
}
