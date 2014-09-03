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

namespace RimLightingProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 20, -30), Vector3.Zero);
            camera.BackgroundColor = Color.CornflowerBlue;
            EntityManager.Add(camera.Entity);            

            Entity box = new Entity("box")
               .AddComponent(new Transform3D() { Position = new Vector3(-10, 0, 0), Scale = new Vector3(5) })
               .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1, 2, 1) })
               .AddComponent(Model.CreateCube())
               .AddComponent(new MaterialsMap(new RimLightMaterial("Content/BoxTexture.wpk")))
               .AddComponent(new ModelRenderer());

            EntityManager.Add(box);

            Entity Sphere = new Entity("sphere")
               .AddComponent(new Transform3D() { Position = new Vector3(0, 0, 5), Scale = new Vector3(5) })
               .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1, 1, 2) })
               .AddComponent(Model.CreateSphere(1, 16))
               .AddComponent(new MaterialsMap(new RimLightMaterial("Content/SphereTexture.wpk")))
               .AddComponent(new ModelRenderer());

            EntityManager.Add(Sphere);

            Entity torus = new Entity("torus")
                .AddComponent(new Transform3D() { Position = new Vector3(10, 0, 0), Scale = new Vector3(5) })
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(2, 1, 1) })
                .AddComponent(Model.CreateTorus(1, 0.333f, 24))
                .AddComponent(new MaterialsMap(new RimLightMaterial() { DiffuseColor = Color.Purple }))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(torus);
        }    
    }
}
