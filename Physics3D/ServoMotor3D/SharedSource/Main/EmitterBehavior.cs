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

namespace ServoMotor3D
{
    [DataContract]
    public class EmitterBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform;

        [DataMember]
        public TimeSpan Interval
        {
            get;
            set;
        }

        [DataMember]
        public Vector3 PositionRandom
        {
            get;
            set;
        }

        private TimeSpan timeTotal;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.timeTotal = TimeSpan.Zero;
            this.Interval = TimeSpan.FromSeconds(1);

            this.PositionRandom = new Vector3(2, 0, 2);
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.timeTotal += gameTime;

            if (this.timeTotal > this.Interval)
            {
                this.EmitEntity();
                this.timeTotal = TimeSpan.Zero;
            }

        }

        private void EmitEntity()
        {
            var random = WaveServices.Random;
            Vector3 offset = new Vector3(
                (float)(0.5f * random.NextDouble() - 0.5f) * this.PositionRandom.X,
                (float)(0.5f * random.NextDouble() - 0.5f) * this.PositionRandom.Y,
                (float)(0.5f * random.NextDouble() - 0.5f) * this.PositionRandom.Z);

            Entity primitive = new Entity()
               .AddComponent(new Transform3D() { Position = this.transform.Position + offset })
               .AddComponent(new SphereCollider3D())
               .AddComponent(Model.CreateSphere())
               .AddComponent(new RigidBody3D())
               .AddComponent(new MaterialsMap() { DefaultMaterialPath = WaveContent.Assets.basicMaterial })
               .AddComponent(new ModelRenderer());

            this.EntityManager.Add(primitive);
        }
    }
}
