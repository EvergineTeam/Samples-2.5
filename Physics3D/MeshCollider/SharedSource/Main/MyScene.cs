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
#endregion

namespace MeshCollider
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene); int i = 0;
            int j = 0;
            WaveServices.TimerFactory.CreateTimer("CreateRigidBody", TimeSpan.FromSeconds(1f),
                () =>
                {
                    j = ++j % 3;
                    switch (j)
                    {
                        case 0:
                            CreateBox("Box" + i, new Vector3(0.1f, 15, 0));
                            break;
                        case 1:
                            CreateBox("Sphere" + i, new Vector3(-5, 18, 0));
                            break;
                        case 2:
                            CreateCapsule("Capusle" + i, new Vector3(0, 20, 0));
                            break;
                        default:
                            throw new InvalidCastException("Invalid type.");
                    }

                    i++;
                    if (i == 50)
                    {
                        WaveServices.TimerFactory.RemoveTimer("CreateRigidBody");
                    }
                }, true);
        }

        private void CreateBox(string name, Vector3 position)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(new BoxCollider3D())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D())
                .AddComponent(new MaterialsMap() { DefaultMaterialPath = WaveContent.Assets.basicMaterial })
                .AddComponent(new ModelRenderer());

            EntityManager.Add(primitive);
        }

        private void CreateCapsule(string name, Vector3 position)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(new CapsuleCollider3D())
                .AddComponent(Model.CreateCapsule())
                .AddComponent(new RigidBody3D())
                .AddComponent(new MaterialsMap() { DefaultMaterialPath = WaveContent.Assets.basicMaterial })
                .AddComponent(new ModelRenderer());

            EntityManager.Add(primitive);
        }

        private void CreateSphere(string name, Vector3 position)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(new SphereCollider3D())
                .AddComponent(Model.CreateSphere())
                .AddComponent(new RigidBody3D())
                .AddComponent(new MaterialsMap() { DefaultMaterialPath = WaveContent.Assets.basicMaterial })
                .AddComponent(new ModelRenderer());

            EntityManager.Add(primitive);
        }
    }
}
