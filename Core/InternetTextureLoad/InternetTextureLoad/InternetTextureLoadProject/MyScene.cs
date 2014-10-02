#region Using Statements
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace InternetTextureLoadProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            var camera = new FixedCamera2D("MainCamera");
            this.EntityManager.Add(camera);

            var internetWaveLogo = new Entity()
                        .AddComponent(new Transform2D()
                        {
                            Origin = Vector2.Center,
                            X = WaveServices.ViewportManager.VirtualWidth * 0.5f,
                            Y = WaveServices.ViewportManager.VirtualHeight * 0.1f,
                        })
                        .AddComponent(new SpriteUrl("Content/Loading.png", "https://raw.githubusercontent.com/WaveEngine/Samples/master/Core/InternetTextureLoad/Images/logo.png"))
                        .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            EntityManager.Add(internetWaveLogo);

            var internetWaveBanner = new Entity()
                        .AddComponent(new Transform2D()
                        {
                            Origin = Vector2.Center,
                            X = WaveServices.ViewportManager.VirtualWidth * 0.5f,
                            Y = WaveServices.ViewportManager.VirtualHeight * 0.7f,
                        })
                        .AddComponent(new SpriteUrl("Content/Loading.png", "https://raw.githubusercontent.com/WaveEngine/Samples/master/Core/InternetTextureLoad/Images/banner.png"))
                        .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            EntityManager.Add(internetWaveBanner);
        }
    }
}
