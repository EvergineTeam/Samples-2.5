#region Using Statements
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace FrustumCulling
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            var freeCamera = EntityManager.Find("freeCamera");
            RenderManager.SetFrustumCullingCamera(freeCamera);

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
                .AddComponent(new CubeMesh())
                .AddComponent(new BoxCollider3D())
                .AddComponent(new MaterialComponent()
                {
                    Material = new StandardMaterial() { DiffuseColor = GetRandomColor() }
                })
                .AddComponent(new MeshRenderer());
            return cube;
        }

        private Entity CreateSphere(string name, Vector3 position)
        {
            Entity sphere = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(new SphereMesh())
                .AddComponent(new BoxCollider3D())
                .AddComponent(new MaterialComponent()
                {
                    Material = new StandardMaterial() { DiffuseColor = GetRandomColor() }
                })
                .AddComponent(new MeshRenderer());
            return sphere;
        }

        private Color GetRandomColor()
        {
            var random = WaveServices.Random;
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
        }
    }
}
