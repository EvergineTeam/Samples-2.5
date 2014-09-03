using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;

namespace RenderTargetProject
{
    public class MyScene2 : Scene
    {
        protected override void CreateScene()
        {            
            ViewCamera camera = new ViewCamera("MainCamera", new Vector3(0, 2, 4), Vector3.Zero);
            camera.BackgroundColor = Color.Black;
            EntityManager.Add(camera.Entity);            

            DirectionalLight skylight = new DirectionalLight("SkyLight", new Vector3(1));
            EntityManager.Add(skylight);

            Entity primitive = new Entity("Primitive")
                .AddComponent(new Transform3D())
                .AddComponent(new BoxCollider())
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1f, 1f, 1f) })
                .AddComponent(Model.CreateCube())
                .AddComponent(new MaterialsMap(new BasicMaterial((WaveServices.ScreenContextManager.CurrentContext[0] as MyScene).SmallTarget) { LightingEnabled = true }))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(primitive);
        }
    }
}
