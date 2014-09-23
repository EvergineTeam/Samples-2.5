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
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Materials;
#endregion

namespace WallDemoProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            //Insert your scene definition here.

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 10, 20), new Vector3(0, 5, 0))
            {
                BackgroundColor = Color.CornflowerBlue
            };

            camera.Entity.AddComponent(new FireBehavior());

            EntityManager.Add(camera);

            Entity ground = new Entity("Ground")
            .AddComponent(new Transform3D() { Position = new Vector3(0, -1, 0), Scale = new Vector3(100, 1, 100) })
            .AddComponent(new BoxCollider())
            .AddComponent(Model.CreateCube())
            .AddComponent(new RigidBody3D() { IsKinematic = true })
            .AddComponent(new MaterialsMap(new BasicMaterial(Color.White)))
            .AddComponent(new ModelRenderer());
            EntityManager.Add(ground);

            int width = 10;
            int height = 10;
            float blockWidth = 2f;
            float blockHeight = 1f;
            float blockLength = 1f;

            int n = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    n++;
                    var toAdd = CreateBox("box" + n, new Vector3(i * blockWidth + .5f * blockWidth * (j % 2) - width * blockWidth * .5f,
                                                                    blockHeight * .5f + j * (blockHeight),
                                                                    0),
                                                                    new Vector3(blockWidth, blockHeight, blockLength), 10);
                    EntityManager.Add(toAdd);
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            // This method is called after the CreateScene and Initialize methods and before the first Update.
        }

        private Entity CreateBox(string name, Vector3 position, Vector3 scale, float mass)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position, Scale = scale })
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/brickTexture")))
                .AddComponent(new Model("Content/brick"))
                .AddComponent(new BoxCollider())
                .AddComponent(new RigidBody3D() { Mass = mass })
                .AddComponent(new ModelRenderer());
            return primitive;
        }

        private Color GetRandomColor()
        {
            var random = WaveServices.Random;
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
        }
    }
}
