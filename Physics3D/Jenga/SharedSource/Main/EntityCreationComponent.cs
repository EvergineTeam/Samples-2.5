using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Models;
using WaveEngine.Framework.Physics3D;

namespace Jenga
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
                var materialModel = this.Assets.LoadModel<MaterialModel>(WaveContent.Assets.brick_mat);
                var material = materialModel.Material; 

                for (int i = 0; i < 15; i++)
                {
                    bool even = (i % 2 == 0);

                    for (int e = 0; e < 3; e++)
                    {
                        Vector3 size = (even) ? new Vector3(1, 1, 3) : new Vector3(3, 1, 1);
                        Vector3 position = new Vector3((even ? e : 1.0f), i, (even ? 1.0f : e));
                        Entity box = this.CreateBox(material, position, size, 1);

                        this.EntityManager.Add(box);
                    }
                }

                this.init = true;
            }
        }

        private Entity CreateBox(Material material,  Vector3 position, Vector3 scale, float mass)
        {
            Entity primitive = new Entity()
                .AddComponent(new Transform3D() { Position = position, Scale = scale })
                .AddComponent(new MaterialsMap(material))
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider3D())
                .AddComponent(new RigidBody3D() { Mass = mass })
                .AddComponent(new ModelRenderer());

            return primitive;
        }
    }
}
