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
using WaveEngine.LeapMotion;
using WaveEngine.LeapMotion.Behaviors;
using WaveEngine.LeapMotion.Drawables;
#endregion

namespace LeapMotionSample
{
    public class MyScene : Scene
    {        
        protected override void CreateScene()
        {            
            this.Load(WaveContent.Scenes.MyScene);

            var leapMotionService = WaveServices.GetService<LeapMotionService>();           

            Entity sprite = new Entity()
                                .AddComponent(new Transform2D()
                                {
                                    Scale = new Vector2(0.5f,1f),
                                })
                                .AddComponent(new Sprite(leapMotionService.DepthTexture))
                                .AddComponent(new SpriteRenderer());
            EntityManager.Add(sprite);
        }
    }
}
