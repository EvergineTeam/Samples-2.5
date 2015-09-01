#region Using Statements
using System;
using AccelerometerProject.Factories;
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

namespace Accelerometer
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            for (int i = 0; i < 100; i++)
            {
                var ball1 = EntitiesFactory.CreateBall("ball" + i);
                EntityManager.Add(ball1);
            }
        }
    }
}
