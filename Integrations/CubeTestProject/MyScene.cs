#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace CubeTestProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);

            FreeCamera camera = new FreeCamera("camera", new Vector3(0, 0, 2.5f), Vector3.Zero);
            camera.BackgroundColor = Color.CornflowerBlue;
            camera.ClearFlags = ClearFlags.Target | ClearFlags.DepthAndStencil;
            
            EntityManager.Add(camera);

            Entity cube = new Entity()
                            .AddComponent(new Transform3D())
                            .AddComponent(new MaterialsMap())
                            .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1, 2, 3) })
                            .AddComponent(Model.CreateTeapot())
                            .AddComponent(new ModelRenderer());
            EntityManager.Add(cube);

            this.AddSceneBehavior(new MySceneBehavior(), SceneBehavior.Order.PostUpdate);
        }
    }
}
