using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;

namespace Entity3DCollision
{
    [DataContract]
    public class CreationBehavior : Behavior
    {
        [RequiredComponent]
        private DrawableLines drawableLines;
        
        private TimeSpan generationTime;
        private TimeSpan remainingTime;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.generationTime = TimeSpan.FromSeconds(1);
            this.remainingTime = this.generationTime;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.remainingTime -= gameTime;

            if (this.remainingTime <= TimeSpan.Zero)
            {
                this.CreateSphere(Vector3.Up * 5 + Vector3.Forward * (float)WaveServices.Random.NextDouble() + Vector3.Left * (float)WaveServices.Random.NextDouble());
                this.remainingTime = this.generationTime;
            }
        }

        private void CreateSphere(Vector3 position)
        {
            RigidBody3D rigidBody;
            Entity primitive = new Entity()
                .AddComponent(new Transform3D() { Position = position, Scale = Vector3.One / 2 })
                .AddComponent(new SphereCollider3D())
                .AddComponent(Model.CreateSphere())
                .AddComponent(rigidBody = new RigidBody3D() { KineticFriction = 10, Restitution = 1 })
                .AddComponent(new MaterialsMap() { DefaultMaterialPath = WaveContent.Assets.basicMaterial })
                .AddComponent(new TimeAliveBehavior())
                .AddComponent(new ModelRenderer());

            this.EntityManager.Add(primitive);

            rigidBody.OnPhysic3DCollision += this.rigidBody_OnPhysic3DCollision;
        }

        private void rigidBody_OnPhysic3DCollision(object sender, Physic3DCollisionEventArgs args)
        {
            this.drawableLines.AddNewLine(args.Position, args.Normal);
        }
    }
}
