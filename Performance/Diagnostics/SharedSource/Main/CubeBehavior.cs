using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace Diagnostics
{
    [DataContract]
    public class CubeBehavior : Behavior
    {
        [RequiredService]
        protected Clock clock;

        [RequiredComponent]
        protected Transform3D Transform;

        private Vector3 initPosition;

        [DataMember]
        public float Frequency { get; set; }

        [DataMember]
        public Vector3 Amplitude { get; set; }

        private float currentAngle;

        public CubeBehavior() { }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.Frequency = 1;
            this.Amplitude = Vector3.UnitX;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.initPosition = this.Transform.Position;
        }

        protected override void Update(TimeSpan gameTime)
        {
            Timers.BeginTimer("Cube Behavior Update");

            currentAngle = (float)(clock.TotalTime.TotalSeconds) * this.Frequency;
            float sinAngle = (float)Math.Sin(currentAngle);
            float cosAngle = (float)Math.Cos(currentAngle);

            Vector3 aux;
            aux.X = initPosition.X + (sinAngle * this.Amplitude.X);
            aux.Y = initPosition.Y + (cosAngle * this.Amplitude.Y);
            aux.Z = initPosition.Z + (sinAngle * this.Amplitude.Z);

            this.Transform.Position = aux;

            Timers.EndTimer("Cube Behavior Update");
            Labels.Add("First cube position", this.Transform.Position);
        }
    }
}
