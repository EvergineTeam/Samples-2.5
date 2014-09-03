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

namespace EntityPickingProject
{
    public class MyScene : Scene
    {     
        private long instances = 0;

        protected override void CreateScene()
        {            
            RenderManager.DebugLines = true;

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 10, -10), Vector3.Zero)
            {
                BackgroundColor = Color.CornflowerBlue,
            };
            EntityManager.Add(camera.Entity);
            camera.Entity.AddComponent(new PickingBehavior());            

            EntityManager.Add(this.CreateGround());
            EntityManager.Add(this.CreateBox(new Vector3(0.0f, 10.0f, 0.0f)));
            EntityManager.Add(this.CreateBox(new Vector3(0.0f, 15.0f, 0.0f)));
            EntityManager.Add(this.CreateBox(new Vector3(0.0f, 20.0f, 0.0f)));
            EntityManager.Add(this.CreateBox(new Vector3(0.0f, 30.0f, 0.0f)));

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    EntityManager.Add(this.CreateBox(new Vector3(j*2, i, 5.0f)));
                }
            }
        }

        private Entity CreateGround()
        {
            Entity ground = new Entity("ground")
                .AddComponent(new Transform3D() { Position = new Vector3(0f, -1.0f, 0f)})
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreatePlane(Vector3.Up, 50))
                .AddComponent(new MaterialsMap())
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new ModelRenderer());
            return ground;
        }

        private Entity CreateBox(Vector3 position)
        {
            Entity box = new Entity("Box" + this.instances++)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new MaterialsMap())
                .AddComponent(new RigidBody3D() { Mass = 10 })
                .AddComponent(new JointMap3D())
                .AddComponent(new ModelRenderer());
            return box;
        }
    }
}
