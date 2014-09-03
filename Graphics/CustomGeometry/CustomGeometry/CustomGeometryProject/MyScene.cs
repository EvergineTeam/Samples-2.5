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
using WaveEngine.Materials;
#endregion

namespace CustomGeometryProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            var camera = new FreeCamera("MainCamera", new Vector3(0, 0, 2), Vector3.Zero);
            camera.BackgroundColor = Color.CornflowerBlue;
            EntityManager.Add(camera);

            Entity triangle = new Entity()
                .AddComponent(new Transform3D())
                //.AddComponent(new Spinner() { IncreaseZ = 0.5f })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.White) { VertexColorEnabled = true }))
                .AddComponent(new CustomGeometryRenderer());

            EntityManager.Add(triangle);
        }
    }
}
