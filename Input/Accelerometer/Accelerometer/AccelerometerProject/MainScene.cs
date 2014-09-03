#region Using Statements
using AccelerometerProject.Behaviors;
using AccelerometerProject.Factories;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace AccelerometerProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {
            Entity camera = new Entity("MainCamera")
                                  .AddComponent(new Camera3D()
                                  {
                                      Position = Vector3.Up,
                                      LookAt = Vector3.Forward,
                                      BackgroundColor=Color.White,
                                  })
                                  .AddComponent(new CameraBehavior());

            EntityManager.Add(camera);


            var size = 500f;

            EntityManager.Add(EntitiesFactory.CreatePlane("plane1", new Vector3(0, -150, 0), new Vector3(size, 1f, size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane2", new Vector3(0, 150, 0), new Vector3(size, 1f, size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane3", new Vector3(-150, 0, 0), new Vector3(1f, size, size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane4", new Vector3(150, 0, 0), new Vector3(1f, size, size)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane5", new Vector3(0, 0, -150), new Vector3(size, size, 1f)));
            EntityManager.Add(EntitiesFactory.CreatePlane("plane6", new Vector3(0, 0, 150), new Vector3(size, size, 1f)));

            for (int i = 0; i < 100; i++)
            {
                var ball1 = EntitiesFactory.CreateBall("ball" + i);
                EntityManager.Add(ball1);
            }
        }

        protected override void Start()
        {
            base.Start();

            // This method is called after the CreateScene and Initialize methods and before the first Update.
        }
    }
}
