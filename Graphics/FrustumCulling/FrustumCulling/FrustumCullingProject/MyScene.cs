#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace FrustumCullingProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            var random = WaveServices.Random;

            var freeCamera = new FreeCamera("FreeCamera", new Vector3(0, 10, -10), Vector3.Zero) { FarPlane = 100 };
            freeCamera.BackgroundColor = Color.CornflowerBlue;
            freeCamera.Entity.AddComponent(new CameraFrustum());
            EntityManager.Add(freeCamera);

            var fixedCamera = new FixedCamera("FixedCamera", new Vector3(20, 50, -20), new Vector3(20, 0, 20));
            fixedCamera.BackgroundColor = Color.CornflowerBlue;

            EntityManager.Add(fixedCamera);
            RenderManager.SetFrustumCullingCamera(freeCamera.Entity);

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

            TextBlock textBlock = new TextBlock("Message");
            textBlock.Text = "Press 1 or 2 to change the camera";
            EntityManager.Add(textBlock);

            AddSceneBehavior(new SelectCamera(freeCamera, fixedCamera), SceneBehavior.Order.PreUpdate);
        }

        private Entity CreateCube(string name, Vector3 position)
        {
            Entity cube = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider())
                .AddComponent(new MaterialsMap(new BasicMaterial(GetRandomColor())))
                .AddComponent(new ModelRenderer());
            return cube;
        }

        private Entity CreateSphere(string name, Vector3 position)
        {
            Entity sphere = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(Model.CreateSphere())
                .AddComponent(new SphereCollider())
                .AddComponent(new MaterialsMap(new BasicMaterial(GetRandomColor())))
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
