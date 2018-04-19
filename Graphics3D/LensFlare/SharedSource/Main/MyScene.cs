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
using WaveEngine.ImageEffects;
#endregion

namespace LensFlare
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {   
            this.Load(WaveContent.Scenes.MyScene);

            this.CreateSunFlare(); 
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
                new FlareElement(0, new Vector2(3), new Color(1f,  1f,  1f), WaveContent.Assets.Textures.Flares.flareTexture_png),
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
    }
}

