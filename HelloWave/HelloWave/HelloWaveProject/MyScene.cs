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
using WaveEngine.Materials;
#endregion

namespace HelloWaveProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.White;

            var camera = new FreeCamera("Main", new Vector3(0, 2, 4), Vector3.Zero);
            EntityManager.Add(camera);

            Entity cube = new Entity()
                .AddComponent(new Transform3D())
                .AddComponent(Model.CreateCube())
                .AddComponent(new Spinner() { AxisTotalIncreases = Vector3.One / 2 })
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/crate.wpk")))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(cube);
        }
    }
}
