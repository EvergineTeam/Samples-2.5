using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace Billboard
{
    [DataContract]
    public class FlareBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;

        private float initY;

        [DataMember]
        public double Angle { get; set; }

        [DataMember]
        public float Speed { get; set; }

        public FlareBehavior()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            initY = transform.LocalPosition.Y;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.Angle += gameTime.TotalSeconds * this.Speed;

            this.transform.Position = new Vector3(
                (float)Math.Sin(this.Angle),
                this.initY,
                (float)Math.Cos(this.Angle));
        }
    }
}