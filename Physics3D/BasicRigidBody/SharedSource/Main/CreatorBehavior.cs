using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;

namespace BasicRigidBody
{
    [DataContract]
    public class CreatorBehavior : Behavior
    {
        private TimeSpan creationTime;
        private TimeSpan watchDogTime;
        private int typeIndexer;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.creationTime = TimeSpan.FromSeconds(1f);
            this.watchDogTime = this.creationTime;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.watchDogTime -= gameTime;

            if (this.watchDogTime <= TimeSpan.Zero)
            {
                typeIndexer = ++typeIndexer % 3;
                switch (typeIndexer)
                {
                    case 0:
                        this.CreateModel(new Vector3(0.1f, 15, 0), Model.CreateCube(), new BoxCollider3D());
                        break;
                    case 1:
                        this.CreateModel(new Vector3(0.01f, 18, 0), Model.CreateSphere(), new SphereCollider3D());
                        break;
                    case 2:
                        this.CreateModel(new Vector3(0, 20, 0), Model.CreateCapsule(), new CapsuleCollider3D());
                        break;
                    default:
                        throw new InvalidCastException("Invalid type.");
                }

                this.watchDogTime = this.creationTime;
            }
        }

        /// <summary>
        /// Creates a new Model
        /// </summary>
        /// <param name="position">model initial position</param>
        /// <param name="model">the model mesh</param>
        /// <param name="collider">the model collider</param>
        private void CreateModel(Vector3 position, Model model, Collider3D collider)
        {
            Entity primitive = new Entity()
               .AddComponent(new Transform3D() { Position = position })
               .AddComponent(collider)
               .AddComponent(model)
               .AddComponent(new RigidBody3D())
               .AddComponent(new MaterialsMap() { DefaultMaterialPath = WaveContent.Assets.basicMaterial })
               .AddComponent(new ModelRenderer());

            this.EntityManager.Add(primitive);
        }
    }
}
