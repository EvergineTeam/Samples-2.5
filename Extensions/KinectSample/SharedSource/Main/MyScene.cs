#region Using Statements
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Kinect;
#endregion

namespace KinectSample
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);

            this.Load(WaveContent.Scenes.MyScene);

            var kinectService = WaveServices.GetService<KinectService>();

            Texture2D texture = kinectService.ColorTexture;

            Entity sprite = new Entity()
                                .AddComponent(new Transform2D()
                                {
                                    XScale = (float)WaveServices.ViewportManager.VirtualWidth / (float)texture.Width,
                                    YScale = (float)WaveServices.ViewportManager.VirtualHeight / (float)texture.Height,
                                })
                                .AddComponent(new Sprite(texture))
                                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
            EntityManager.Add(sprite);
        }
    }
}
