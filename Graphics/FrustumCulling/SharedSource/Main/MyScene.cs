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
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace FrustumCulling
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");           
        }

        protected override void Start()
        {
            base.Start();


            var fixedCamera = EntityManager.Find("FixedCamera");
            fixedCamera.FindComponent<Transform3D>().LookAt(new Vector3(20, 0, 20));
            fixedCamera.FindComponent<Camera3D>().Viewport = new Viewport(0, 0.6f, 0.4f, 0.4f);

            RenderManager.SetFrustumCullingCamera(EntityManager.Find("FreeCamera"));

            var random = WaveServices.Random;
            
            int num = 15;

            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    if (random.NextBool())
                    {
                        Entity cube = CreateCube("Cube" + (j + (i * num)), new Vector3(j * 3, 0, i * 3));
                        EntityManager.Add(cube);
                    }
                    else
                    {
                        Entity sphere = CreateSphere("Sphere" + (j + (i * num)), new Vector3(j * 3, 0, i * 3));
                        EntityManager.Add(sphere);

                    }
                }
            }
        }

        private Entity CreateCube(string name, Vector3 position)
        {
            Entity cube = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider3D())
                .AddComponent(new MaterialsMap(new StandardMaterial(GetRandomColor(), DefaultLayers.Opaque)))
                .AddComponent(new ModelRenderer());
            return cube;
        }

        private Entity CreateSphere(string name, Vector3 position)
        {
            Entity sphere = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(Model.CreateSphere())
                .AddComponent(new SphereCollider3D())
                .AddComponent(new MaterialsMap(new StandardMaterial(GetRandomColor(), DefaultLayers.Opaque)))
                .AddComponent(new ModelRenderer());
            return sphere;
        }

        private Color GetRandomColor()
        {
            var random = WaveServices.Random;
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
        }
    }
}
