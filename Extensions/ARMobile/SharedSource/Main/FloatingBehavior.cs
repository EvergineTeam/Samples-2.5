using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;

namespace ARMobile
{
    [DataContract]
    public class FloatingBehavior : Behavior
    {
        [RequiredComponent]
        protected Transform3D transform3D;

        [DataMember]
        public float Amplitude { get; set; }

        [DataMember]
        public float SpeedFactor { get; set; }

        private double acc;

        private float initialLocalPositionY;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.Amplitude = 0.5f;
            this.SpeedFactor = 1f;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.initialLocalPositionY = this.transform3D.LocalPosition.Y;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.acc += this.SpeedFactor * gameTime.TotalSeconds;

            var offset = (float)Math.Sin(this.acc) * this.Amplitude;

            var position = this.transform3D.LocalPosition;
            position.Y = this.initialLocalPositionY + offset;
            this.transform3D.LocalPosition = position;
        }
    }
}
