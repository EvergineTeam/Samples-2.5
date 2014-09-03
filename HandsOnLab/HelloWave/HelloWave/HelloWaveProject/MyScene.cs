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

namespace HelloWaveProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            var camera = new FreeCamera("Main", new Vector3(0, 2, 4), Vector3.Zero)
            {
                BackgroundColor = Color.White
            };
            EntityManager.Add(camera);

            Entity cube = new Entity()
                .AddComponent(new Transform3D())
                .AddComponent(Model.CreateCube())
                .AddComponent(new Spinner() { AxisTotalIncreases = Vector3.One / 2 })
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/crate.wpk")))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(cube);
        }

        protected override void Start()
        {
            base.Start();

            // This method is called after the CreateScene and Initialize methods and before the first Update.
        }
    }
}
