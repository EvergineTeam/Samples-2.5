using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;

namespace EntityPicking
{
    [DataContract]
    public class EntityCreationComponent : Behavior
    {
        private bool init;

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (!this.init)
            {
                this.EntityManager.Add(this.CreateBox(new Vector3(0.0f, 15.0f, 0.0f)));
                this.EntityManager.Add(this.CreateBox(new Vector3(0.0f, 20.0f, 0.0f)));
                this.EntityManager.Add(this.CreateBox(new Vector3(0.0f, 30.0f, 0.0f)));
                this.EntityManager.Add(this.CreateBox(new Vector3(0.0f, 10.0f, 0.0f)));

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        this.EntityManager.Add(this.CreateBox(new Vector3(j * 2, i, 5.0f)));
                    }
                }
                this.init = true;
            }
        }

        private Entity CreateBox(Vector3 position)
        {
            Entity box = new Entity()
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(new BoxCollider3D())
                .AddComponent(Model.CreateCube())
                .AddComponent(new MaterialsMap() { DefaultMaterialPath = WaveContent.Assets.basicMaterial })
                .AddComponent(new RigidBody3D() { Mass = 10 })
                .AddComponent(new JointMap3D())
                .AddComponent(new ModelRenderer());
            return box;
        }
    }
}
